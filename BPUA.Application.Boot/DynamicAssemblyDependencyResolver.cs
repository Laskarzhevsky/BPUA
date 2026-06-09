using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Resolves dependencies for dynamically loaded BPUA assemblies.
    /// The resolver searches both the plugin assembly folder and the host build folder.
    /// </summary>
    internal static class DynamicAssemblyDependencyResolver
    {
        #region Fields
        static readonly object SyncRoot = new object();
        static readonly List<string> ProbingFolders = new List<string>();
        static bool ResolverWasRegistered;
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a plugin assembly path as a source for dependency probing.
        /// </summary>
        /// <param name="pathToDynamicAssembly">Path to the dynamic assembly being loaded.</param>
        public static void RegisterDynamicAssemblyPath(string pathToDynamicAssembly)
        {
            if (string.IsNullOrWhiteSpace(pathToDynamicAssembly))
            {
                return;
            }

            EnsureResolverRegistered();

            string? pluginFolder = Path.GetDirectoryName(pathToDynamicAssembly);
            AddProbingFolder(pluginFolder);

            string applicationBaseFolder = AppContext.BaseDirectory;
            AddProbingFolder(applicationBaseFolder);

            string? buildFolder = TryFindBuildFolder(pluginFolder);
            AddProbingFolder(buildFolder);
        }
        #endregion

        #region Private Methods
        static void EnsureResolverRegistered()
        {
            lock (SyncRoot)
            {
                if (!ResolverWasRegistered)
                {
                    AssemblyLoadContext.Default.Resolving += ResolveAssembly;
                    ResolverWasRegistered = true;
                }
            }
        }

        static void AddProbingFolder(string? folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return;
            }

            string fullPath = Path.GetFullPath(folderPath);
            if (!Directory.Exists(fullPath))
            {
                return;
            }

            lock (SyncRoot)
            {
                bool alreadyExists = false;
                int index = 0;
                while (index < ProbingFolders.Count)
                {
                    if (string.Equals(ProbingFolders[index], fullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        alreadyExists = true;
                        break;
                    }

                    index = index + 1;
                }

                if (!alreadyExists)
                {
                    ProbingFolders.Add(fullPath);
                }
            }
        }

        static Assembly? ResolveAssembly(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            Assembly? alreadyLoadedAssembly = TryFindAlreadyLoadedAssembly(assemblyName);
            if (alreadyLoadedAssembly != null)
            {
                return alreadyLoadedAssembly;
            }

            string assemblyFileName = assemblyName.Name + ".dll";
            string? assemblyPath = TryFindAssemblyFile(assemblyFileName);
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                return null;
            }

            return assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        }

        static Assembly? TryFindAlreadyLoadedAssembly(AssemblyName assemblyName)
        {
            Assembly? result = null;
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            int index = 0;
            while (index < loadedAssemblies.Length)
            {
                Assembly loadedAssembly = loadedAssemblies[index];
                AssemblyName loadedAssemblyName = loadedAssembly.GetName();
                if (string.Equals(loadedAssemblyName.Name, assemblyName.Name, StringComparison.OrdinalIgnoreCase))
                {
                    result = loadedAssembly;
                    break;
                }

                index = index + 1;
            }

            return result;
        }

        static string? TryFindAssemblyFile(string assemblyFileName)
        {
            List<string> foldersSnapshot = GetProbingFoldersSnapshot();

            string? result = TryFindAssemblyFileInKnownFolders(assemblyFileName, foldersSnapshot);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            result = TryFindAssemblyFileRecursively(assemblyFileName, foldersSnapshot);
            return result;
        }

        static string? TryFindAssemblyFileInKnownFolders(string assemblyFileName, List<string> foldersSnapshot)
        {
            string? result = null;
            int index = 0;
            while (index < foldersSnapshot.Count)
            {
                string candidatePath = Path.Combine(foldersSnapshot[index], assemblyFileName);
                if (File.Exists(candidatePath))
                {
                    result = candidatePath;
                    break;
                }

                index = index + 1;
            }

            return result;
        }

        static string? TryFindAssemblyFileRecursively(string assemblyFileName, List<string> foldersSnapshot)
        {
            string? result = null;
            int index = 0;
            while (index < foldersSnapshot.Count)
            {
                string folder = foldersSnapshot[index];
                try
                {
                    string[] files = Directory.GetFiles(folder, assemblyFileName, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        result = files[0];
                        break;
                    }
                }
                catch
                {
                    // Ignore folders that cannot be searched.
                }

                index = index + 1;
            }

            return result;
        }

        static List<string> GetProbingFoldersSnapshot()
        {
            List<string> snapshot = new List<string>();
            lock (SyncRoot)
            {
                int index = 0;
                while (index < ProbingFolders.Count)
                {
                    snapshot.Add(ProbingFolders[index]);
                    index = index + 1;
                }
            }

            return snapshot;
        }

        static string? TryFindBuildFolder(string? pluginFolder)
        {
            if (string.IsNullOrWhiteSpace(pluginFolder))
            {
                return null;
            }

            DirectoryInfo? currentDirectory = new DirectoryInfo(pluginFolder);
            while (currentDirectory != null)
            {
                if (string.Equals(currentDirectory.Name, "PluginFolder", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentDirectory.Parent != null)
                    {
                        return currentDirectory.Parent.FullName;
                    }

                    return null;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return null;
        }
        #endregion
    }
}
