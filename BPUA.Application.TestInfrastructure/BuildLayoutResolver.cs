using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BPUA.Application.TestInfrastructure
{
    public static class BuildLayoutResolver
    {
        public static string ResolvePluginFolderForProject(string projectFileNameWithoutExtension)
        {
//            string solutionRoot = FindBuildFolder();
            string buildFolder = ResolveBuildFolder();
            string projectFilePath = ResolveProjectFile(projectFileNameWithoutExtension);
            string projectFolder = Path.Combine(buildFolder, projectFileNameWithoutExtension);
//            string projectFilePath = Path.Combine(projectFolder, projectFileNameWithoutExtension + ".csproj");

            if (!File.Exists(projectFilePath))
            {
                throw new FileNotFoundException("Project file was not found.", projectFilePath);
            }

            string sharedBuildSettingsPath = Path.Combine(buildFolder, "SharedBuildSettings.props");
            if (!File.Exists(sharedBuildSettingsPath))
            {
                throw new FileNotFoundException("SharedBuildSettings.props was not found.", sharedBuildSettingsPath);
            }

            ProjectLayoutSettings sharedSettings = ReadLayoutSettings(sharedBuildSettingsPath);
            ProjectLayoutSettings projectSettings = ReadLayoutSettings(projectFilePath);

            string domainName = FirstNonEmpty(projectSettings.DomainName, sharedSettings.DomainName);
            string pluginFolderSubdir = FirstNonEmpty(projectSettings.PluginFolderSubdir, sharedSettings.PluginFolderSubdir);
            string useCaseName = projectSettings.UseCaseName;

            if (string.IsNullOrWhiteSpace(domainName))
            {
                throw new InvalidOperationException("DomainName was not found in project or shared build settings.");
            }

            if (string.IsNullOrWhiteSpace(useCaseName))
            {
                throw new InvalidOperationException("UseCaseName was not found in project file.");
            }

            string pluginFolderName = domainName + "." + useCaseName;

            if (string.IsNullOrWhiteSpace(pluginFolderSubdir))
            {
                return Path.Combine(buildFolder, "PluginFolder", pluginFolderName);
            }

            return Path.Combine(buildFolder, "PluginFolder", NormalizeDirectorySeparators(pluginFolderSubdir), pluginFolderName);
        }

        public static string FindSharedBuildSettingsFile()
        {
            string current = AppContext.BaseDirectory;

            while (!string.IsNullOrWhiteSpace(current))
            {
                string propsFilePath = Path.Combine(current, "SharedBuildSettings.props");

                if (File.Exists(propsFilePath))
                {
                    return propsFilePath;
                }

                DirectoryInfo? parent = Directory.GetParent(current);
                current = parent == null ? string.Empty : parent.FullName;
            }

            throw new DirectoryNotFoundException("Solution root containing SharedBuildSettings.props and Build folder was not found.");
        }

        public static string FindProjectFilePath(string projectFileName)
        {
            string current = AppContext.BaseDirectory;

            while (!string.IsNullOrWhiteSpace(current))
            {
                string projectDirectory = Path.Combine(current, projectFileName);

                if (Directory.Exists(projectDirectory))
                {
                    string projectFilePath = Path.Combine(projectDirectory, projectFileName + ".csproj");
                    return projectFilePath;
                }

                DirectoryInfo? parent = Directory.GetParent(current);
                current = parent == null ? string.Empty : parent.FullName;
            }

            throw new DirectoryNotFoundException($"Solution root containing {projectFileName} was not found.");
        }

        static string ResolveProjectFile(string projectFileName)
        {
            string projectFilePath = FindProjectFilePath(projectFileName);
            if (!File.Exists(projectFilePath))
            {
                throw new FileNotFoundException("Project file was not found.", projectFilePath);
            }

            return projectFilePath;
        }

        static string ResolveBuildFolder()
        {
            string sharedBuildSettingsFilePath = FindSharedBuildSettingsFile();
            ProjectLayoutSettings sharedSettings = ReadLayoutSettings(sharedBuildSettingsFilePath);
            string buildFolderRelativePath = FirstNonEmpty(sharedSettings.BPUAOutDir, "Build");
            string normalized = NormalizeDirectorySeparators(buildFolderRelativePath);
            return Path.GetFullPath(Path.Combine(sharedBuildSettingsFilePath, normalized));
        }

        static ProjectLayoutSettings ReadLayoutSettings(string xmlFilePath)
        {
            XDocument document = XDocument.Load(xmlFilePath);
            return new ProjectLayoutSettings
            {
                DomainName = ReadFirstElementValue(document, "DomainName"),
                UseCaseName = ReadFirstElementValue(document, "UseCaseName"),
                PluginFolderSubdir = ReadFirstElementValue(document, "PluginFolderSubdir"),
                BPUAOutDir = ReadFirstElementValue(document, "BPUAOutDir")
            };
        }

        static string ReadFirstElementValue(XDocument document, string elementLocalName)
        {
            XElement? element = document
                .Descendants()
                .FirstOrDefault(delegate (XElement currentElement)
                {
                    return string.Equals(currentElement.Name.LocalName, elementLocalName, StringComparison.Ordinal);
                });

            return element == null ? string.Empty : (element.Value ?? string.Empty).Trim();
        }

        static string FirstNonEmpty(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                {
                    return values[i];
                }
            }

            return string.Empty;
        }

        static string NormalizeDirectorySeparators(string path)
        {
            return path
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar)
                .Trim();
        }

        sealed class ProjectLayoutSettings
        {
            public string DomainName { get; set; } = string.Empty;

            public string UseCaseName { get; set; } = string.Empty;

            public string PluginFolderSubdir { get; set; } = string.Empty;

            public string BPUAOutDir { get; set; } = string.Empty;
        }
    }
}
