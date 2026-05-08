namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Releases resources
        /// </summary>
        void ReleaseResources()
        {
            LoadedAssembly = null;
            PathToDynamicAssembly = default!;
            PathToFolderWithDynamicAssemblies = default!;
            ServiceRegistry = default!;
            ListOfLoadedAssemblies = default!;
            ListOfAssemblyProcessors = default!;
        }
        #endregion
    }
}
