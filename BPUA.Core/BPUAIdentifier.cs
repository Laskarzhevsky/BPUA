namespace BPUA.Core
{
    /// <summary>
    /// Defines BPUA identifier
    /// </summary>
    public class BPUAIdentifier : IBPUAIdentifier
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BPUAIdentifier()
        {
        }

        /// <summary>
        /// Creates instance of BPUA identifier string representation
        /// </summary>
        /// <param name="bpuaIdentifierStringRepresentation">BPUA identifier string representation</param>
        public BPUAIdentifier(string bpuaIdentifierStringRepresentation)
        {
            string[] bpuaIdentifierPats = bpuaIdentifierStringRepresentation.Split('_');
            DomainName = bpuaIdentifierPats[0];
            UseCaseName = bpuaIdentifierPats[1];
            ApplicationLayerName = bpuaIdentifierPats[2];
            StateName = bpuaIdentifierPats[3];

            if (bpuaIdentifierPats.Length > 4)
            {
                TransitionName = bpuaIdentifierPats[4];
            }
        }

        /// <summary>
        /// Creates instance of BPUA identifier from metadata
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State nName</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="requestName">Request name</param>
        public BPUAIdentifier(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs = null, string? requestName = null)
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
        /// Clones BPUA identifier
        /// IBPUAIdentifier interface implementation
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier to clone</param>
        /// <returns>Cloned BPUA identifier</returns>
        public IBPUAIdentifier Clone(IBPUAIdentifier bpuaIdentifier)
        {
            IBPUAIdentifier clonedBpuaIdentifier = new BPUAIdentifier();

            clonedBpuaIdentifier.DomainName = DomainName;
            clonedBpuaIdentifier.UseCaseName = UseCaseName;
            clonedBpuaIdentifier.ApplicationLayerName = ApplicationLayerName;
            clonedBpuaIdentifier.StateName = StateName;
            clonedBpuaIdentifier.TransitionName = TransitionName;
            clonedBpuaIdentifier.Breadcrumbs = Breadcrumbs;
            clonedBpuaIdentifier.RequestName = RequestName;

            return clonedBpuaIdentifier;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets application layer name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets breadcrumbs
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? Breadcrumbs
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets request name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? RequestName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets state name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? StateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets transition name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? TransitionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets application layer name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}
