using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Represents an immutable-by-convention copy of an identifier at activation request time.
    /// The snapshot prevents the delayed activation pipeline from observing later modifications
    /// to the original identifier object supplied by the caller.
    /// </summary>
    sealed class IdentifierSnapshot : IBpuIdentifier
    {
        /// <summary>
        /// Copies all relevant identifier values from the supplied identifier into a separate object.
        /// Null string values that are important for path and key calculations are normalized to empty strings.
        /// </summary>
        /// <param name="identifier">The identifier to copy.</param>
        public IdentifierSnapshot(IBpuIdentifier identifier)
        {
            if (string.IsNullOrEmpty(identifier.DomainName))
            {
                DomainName = string.Empty;
            }
            else
            {
                DomainName = identifier.DomainName;
            }

            if (string.IsNullOrEmpty(identifier.UseCaseName))
            {
                UseCaseName = string.Empty;
            }
            else
            {
                UseCaseName = identifier.UseCaseName;
            }

            ApplicationLayerName = identifier.ApplicationLayerName;
            StateName = identifier.StateName;
            TransitionName = identifier.TransitionName;
            Breadcrumbs = identifier.Breadcrumbs;
            RequestName = identifier.RequestName;
        }

        /// <summary>
        /// Gets or sets the application layer name captured from the original identifier.
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? ApplicationLayerName
        {
            get; set; 
        }

        /// <summary>
        /// Gets or sets the breadcrumbs captured from the original identifier.
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? Breadcrumbs 
        {
            get; set; 
        }

        /// <summary>
        /// Gets or sets the domain name captured from the original identifier.
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
        /// Gets or sets the state name captured from the original identifier.
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? StateName 
        {
            get; set; 
        }

        /// <summary>
        /// Gets or sets the transition name captured from the original identifier.
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? TransitionName 
        {
            get; set; 
        }

        /// <summary>
        /// Gets or sets the use-case name captured from the original identifier.
        /// IBpuIdentifier interface implementation
        /// </summary>
        public string? UseCaseName 
        {
            get; set; 
        }
    }
}
