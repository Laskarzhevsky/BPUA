using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks an assembly as loadable by the BPUA platform (technical concern).
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class LoadBPUAAssemblyAttribute : Attribute
    {
        #region Constractors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoadBPUAAssemblyAttribute()
        {
        }

        /// <summary>
        /// Creates an instance by using module name
        /// </summary>
        /// <param name="moduleName">Module name</param>
        public LoadBPUAAssemblyAttribute(string moduleName)
        {
            ModuleName = moduleName;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets optional logical module name (for diagnostics or filtering).
        /// </summary>
        public string? ModuleName
        {
            get;
        }
        #endregion
    }
}
