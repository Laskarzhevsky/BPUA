namespace BPUA.Core
{
    /// <summary>
    /// Provides extension methods for BPU identifier
    /// </summary>
    public static partial class BpuIdentifierExtensionMethods
    {
        #region Public Methods
        /// <summary>
        /// Clones BPU identifier
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier to clone</param>
        /// <returns>Cloned BPU identifier</returns>
        public static IBpuIdentifier? Clone(this IBpuIdentifier? bpuIdentifier)
        {
            if (bpuIdentifier == null)
            {
                return null;
            }

            IBpuIdentifier clonedBpuIdentifier = new BpuIdentifier();

            clonedBpuIdentifier.DomainName = bpuIdentifier.DomainName;
            clonedBpuIdentifier.UseCaseName = bpuIdentifier.UseCaseName;
            clonedBpuIdentifier.ApplicationLayerName = bpuIdentifier.ApplicationLayerName;
            clonedBpuIdentifier.StateName = bpuIdentifier.StateName;
            clonedBpuIdentifier.TransitionName = bpuIdentifier.TransitionName;
            clonedBpuIdentifier.Breadcrumbs = bpuIdentifier.Breadcrumbs;
            clonedBpuIdentifier.RequestName = bpuIdentifier.RequestName;

            return clonedBpuIdentifier;
        }
        #endregion
    }
}
