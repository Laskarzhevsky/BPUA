namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Loads static assemblies
        /// </summary>
        void LoadStaticAssemblies()
        {
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.BusinessLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.DataAccessLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.DataProcessingLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.Orchestration.AssemblyReference).Assembly);
            //            ListOfLoadedAssemblies.Add(typeof(BPUA.SqlServer.EventHandlers.AssemblyReference).Assembly);
        }
        #endregion
    }
}
