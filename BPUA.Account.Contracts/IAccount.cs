namespace BPUA.Account.Contracts
{
    /// <summary>
    /// Defines account-related contracts for the BPUA application.
    /// This interface serves as a marker for account-related components and
    /// can be extended with common account properties or methods in the future.
    /// </summary>
    public interface IAccount
    {
        #region Properties
        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        string Email 
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the password associated with the entity.
        /// </summary>
        string Password
        {
            get; set; 
        }
        #endregion
    }
}
