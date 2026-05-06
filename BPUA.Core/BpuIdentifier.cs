namespace BPUA.Core
{
    /// <summary>
    /// Defines BPU identifier
    /// </summary>
    public class BpuIdentifier : IBpuIdentifier
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BpuIdentifier()
        {
        }

        /// <summary>
        /// Creates instance of BPU identifier string representation
        /// </summary>
        /// <param name="bpuIdentifierStringRepresentation">BPU identifier string representation</param>
        public BpuIdentifier(string bpuIdentifierStringRepresentation)
        {
            string[] bpuIdentifierPats = bpuIdentifierStringRepresentation.Split('_');
            DomainName = bpuIdentifierPats[0];
            UseCaseName = bpuIdentifierPats[1];
            ApplicationLayerName = bpuIdentifierPats[2];
            StateName = bpuIdentifierPats[3];

            if (bpuIdentifierPats.Length > 4)
            {
                TransitionName = bpuIdentifierPats[4];
            }
        }

        /// <summary>
        /// Creates instance of BPU identifier from metadata
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State nName</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="requestName">Request name</param>
        public BpuIdentifier(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs = null, string? requestName = null)
        {
            DomainName = domainName;
            UseCaseName = useCaseName;
            ApplicationLayerName = applicationLayerName;
            StateName = stateName;
            TransitionName = transitionName;
            Breadcrumbs = breadcrumbs;
            RequestName = requestName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clones BPU identifier
        /// IBpuIdentifier interface implementation
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier to clone</param>
        /// <returns>Cloned BPU identifier</returns>
        public IBpuIdentifier Clone(IBpuIdentifier bpuIdentifier)
        {
            IBpuIdentifier clonedBpuIdentifier = new BpuIdentifier();

            clonedBpuIdentifier.DomainName = DomainName;
            clonedBpuIdentifier.UseCaseName = UseCaseName;
            clonedBpuIdentifier.ApplicationLayerName = ApplicationLayerName;
            clonedBpuIdentifier.StateName = StateName;
            clonedBpuIdentifier.TransitionName = TransitionName;
            clonedBpuIdentifier.Breadcrumbs = Breadcrumbs;
            clonedBpuIdentifier.RequestName = RequestName;

            return clonedBpuIdentifier;
        }

        /// <summary>
        /// Returns string representation of BPU identifier
        /// </summary>
        /// <returns>String representation of BPU identifier</returns>
        public override string? ToString()
        {
            return $"DomainName: {DomainName}, UseCaseName: {UseCaseName}, ApplicationLayerName: {ApplicationLayerName}, StateName: {StateName}, TransitionName: {TransitionName}";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets application layer name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets breadcrumbs
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? Breadcrumbs
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets request name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? RequestName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets state name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? StateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets transition name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? TransitionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets application layer name
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}
