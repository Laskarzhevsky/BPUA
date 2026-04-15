namespace BPUA.Core
{
    /// <summary>
    /// Provides extension methods for BPUA identifier
    /// </summary>
    public static partial class BPUAIdentifierExtensionMethods
    {
        #region Public Methods
        /// <summary>
        /// Clones BPUA identifier
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier to clone</param>
        /// <returns>Cloned BPUA identifier</returns>
        public static IBPUAIdentifier? Clone(this IBPUAIdentifier? bpuaIdentifier)
        {
            if (bpuaIdentifier == null)
            {
                return null;
            }

            IBPUAIdentifier clonedBpuaIdentifier = new BPUAIdentifier();

            clonedBpuaIdentifier.DomainName = bpuaIdentifier.DomainName;
            clonedBpuaIdentifier.UseCaseName = bpuaIdentifier.UseCaseName;
            clonedBpuaIdentifier.ApplicationLayerName = bpuaIdentifier.ApplicationLayerName;
            clonedBpuaIdentifier.StateName = bpuaIdentifier.StateName;
            clonedBpuaIdentifier.TransitionName = bpuaIdentifier.TransitionName;
            clonedBpuaIdentifier.Breadcrumbs = bpuaIdentifier.Breadcrumbs;
            clonedBpuaIdentifier.RequestName = bpuaIdentifier.RequestName;

            return clonedBpuaIdentifier;
        }
        #endregion
    }
}
