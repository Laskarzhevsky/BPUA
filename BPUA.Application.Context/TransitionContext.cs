using BPUA.Core;
using BPUA.Application.Contracts;
using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Context
{
    /// <summary>
    /// Provides transition context functionality
    /// </summary>
    public class TransitionContext : ITransitionContext
    {
        #region Data Fields
        /// <summary>
        /// Holds list of request metadata
        /// </summary>
        ListOfRequestMetadata _listOfRequestMetadata = new ListOfRequestMetadata();

        /// <summary>
        /// Holds list of transition metadata
        /// </summary>
        ListOfTransitionMetadata _listOfTransitionMetadata = new ListOfTransitionMetadata();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates instance of TransitionContext class
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public TransitionContext(IDataSet dataSet, IBPUAIdentifier bpuaIdentifier)
        {
            DataSet = dataSet;
            _listOfRequestMetadata.AddRequestMetadata(bpuaIdentifier);
        }

        /// <summary>
        /// Creates instance of TransitionContext class
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        public TransitionContext(IDataSet dataSet, string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName, string? breadcrumbs = null)
        {
            DataSet = dataSet;
            _listOfRequestMetadata.AddRequestMetadata(domainName, useCaseName, applicationLayerName, stateName, transitionName, breadcrumbs);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public void AddRequestMetadata(IBPUAIdentifier bpuaIdentifier)
        {
            _listOfRequestMetadata.AddRequestMetadata(bpuaIdentifier);
        }

        /// <summary>
        /// Adds request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="requestMetadata">Request metadata</param>
        public void AddRequestMetadata(IRequestMetadata requestMetadata)
        {
            _listOfRequestMetadata.AddRequestMetadata(requestMetadata);
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
            _listOfRequestMetadata.AddRequestMetadata(domainName, useCaseName, applicationLayerName, stateName, transitionName, breadcrumbs);
        }

        /// <summary>
        /// Adds transition metadata
        /// ITransitionContext interface implementation
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs data</param>
        public void AddTransitionMetadata(string domainName, string useCaseName, string stateName, string transitionName, string? breadcrumbs = null)
        {
            _listOfTransitionMetadata.AddTransitionMetadata(domainName, useCaseName, stateName, transitionName, breadcrumbs);
        }

        /// <summary>
        /// Removes current request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        public void RemoveCurrentRequestMetadata()
        {
            _listOfRequestMetadata.RemoveCurrentRequestMetadata();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets BPUA identifier
        /// ITransitionContext interface implementation
        /// </summary>
        public IBPUAIdentifier BPUAIdentifier
        {
            get
            {
                return _listOfRequestMetadata.GetBPUAIdentifier();
            }
        }

        /// <summary>
        /// Gets data set
        /// ITransitionContext interface implementation
        /// </summary>
        public IDataSet DataSet
        {
            get; private set;
        }

        /// <summary>
        /// Gets request metadata
        /// ITransitionContext interface implementation
        /// </summary>
        public IRequestMetadata RequestMetadata
        {
            get
            {
                return _listOfRequestMetadata.GetRequestMetadata();
            }
        }

        /// <summary>
        /// Gets transitions metadata
        /// ITransitionContext interface implementation
        /// </summary>
        public IReadOnlyList<ITransitionMetadata> TransitionsMetadata
        {
            get
            {
                return _listOfTransitionMetadata;
            }
        }
        #endregion
    }
}
