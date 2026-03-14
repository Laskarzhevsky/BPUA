using BPUA.Application.Contracts;
using BPUA.Core;

using System.Collections.Generic;

namespace BPUA.Application.Context
{
    /// <summary>
    /// Provides list of request metadata functionality
    /// </summary>
    internal class ListOfRequestMetadata : List<IRequestMetadata>
    {
        #region Public Methods
        /// <summary>
        /// Adds request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public void AddRequestMetadata(IBPUAIdentifier bpuaIdentifier)
        {
            AddRequestMetadata(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName, bpuaIdentifier.Breadcrumbs);
        }

        /// <summary>
        /// Adds request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="requestMetadata">Request metadata</param>
        public void AddRequestMetadata(IRequestMetadata requestMetadata)
        {
            AddRequestMetadata(requestMetadata.DomainName, requestMetadata.UseCaseName, requestMetadata.ApplicationLayerName, requestMetadata.StateName, requestMetadata.TransitionName, requestMetadata.Breadcrumbs);
        }

        /// <summary>
        /// Adds request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        public void AddRequestMetadata(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs = null)
        {
            IRequestMetadata requestMetadata = new RequestMetadata();

            requestMetadata.DomainName = domainName;
            requestMetadata.UseCaseName = useCaseName;
            requestMetadata.ApplicationLayerName = applicationLayerName;
            requestMetadata.StateName = stateName;
            requestMetadata.Breadcrumbs = breadcrumbs;

            Add(requestMetadata);
        }

        /// <summary>
        /// Gets BPUA identifier
        /// </summary>
        /// <returns>BPUA identifier</returns>
        public IBPUAIdentifier GetBPUAIdentifier()
        {
            return GetRequestMetadata();
        }

        /// <summary>
        /// Gets request metadta
        /// </summary>
        /// <returns>Request metadta</returns>
        public IRequestMetadata GetRequestMetadata()
        {
            if (Count > 0)
            {
                return this[Count - 1];
            }

            return new RequestMetadata();
        }

        /// <summary>
        /// Removes current request metadata
        /// </summary>
        public void RemoveCurrentRequestMetadata()
        {
            IRequestMetadata currentRequestMetadata = GetRequestMetadata();

            // Copy current request metadata
            BPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.ApplicationLayerName = currentRequestMetadata.ApplicationLayerName;
            bpuaIdentifier.DomainName = currentRequestMetadata.DomainName;
            bpuaIdentifier.StateName = currentRequestMetadata.StateName;
            bpuaIdentifier.TransitionName = currentRequestMetadata.TransitionName;
            bpuaIdentifier.UseCaseName = currentRequestMetadata.UseCaseName;

            // Propagate current use case matadata to previous for BL layer
            if (bpuaIdentifier.ApplicationLayerName == ApplicationLayersNames.BL)
            {
                currentRequestMetadata = GetRequestMetadata();
                if (currentRequestMetadata.DomainName != bpuaIdentifier.DomainName)
                {
                    currentRequestMetadata.DomainName = bpuaIdentifier.DomainName;
                }

                if (currentRequestMetadata.StateName != bpuaIdentifier.StateName)
                {
                    currentRequestMetadata.StateName = bpuaIdentifier.StateName;
                }

                if (currentRequestMetadata.TransitionName != bpuaIdentifier.TransitionName)
                {
                    currentRequestMetadata.TransitionName = bpuaIdentifier.TransitionName;
                }

                if (currentRequestMetadata.UseCaseName != bpuaIdentifier.UseCaseName)
                {
                    currentRequestMetadata.UseCaseName = bpuaIdentifier.UseCaseName;
                }
            }
        }
        #endregion
    }
}
