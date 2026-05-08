using System.Collections.Generic;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Properties
        /// <summary>
        /// Gets or sets list of assembly processors
        /// </summary>
        List<IBpuaAssemblyProcessor> ListOfAssemblyProcessors
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets loaded assembly
        /// </summary>
        Assembly? LoadedAssembly
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets list of loaded assemblies
        /// </summary>
        List<Assembly> ListOfLoadedAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to dynamic assembly
        /// </summary>
        string PathToDynamicAssembly
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to folder with dynamic assemblies
        /// </summary>
        string PathToFolderWithDynamicAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets service registry
        /// </summary>
        IServiceRegistry ServiceRegistry
        {
            get; set;
        } = default!;
        #endregion
    }
}
