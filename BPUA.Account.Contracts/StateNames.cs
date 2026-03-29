namespace BPUA.Account.Contracts
{
    /// <summary>
    /// Defines state names
    /// </summary>
    public class StateNames
    {
        /// <summary>
        /// Defines Logged In state
        /// </summary>
        public static string LOGGED_IN = "LoggedIn";

        /// <summary>
        /// Defines registered state
        /// </summary>
        public static string REGISTERED = "Registered";

        /// <summary>
        /// Defines waiting for account registration state
        /// </summary>
        public static string WAITING_OF_ACCOUNT_REGISTRATION = "WaitingOfAccountRegistration";

        /// <summary>
        /// Defines waiting for password change state
        /// </summary>
        public static string WAITING_FOR_PASSWORD_CHANGE = "WaitingForPasswordChange";

        /// <summary>
        /// Defines waiting for password reset state
        /// </summary>
        public static string WAITING_FOR_PASSWORD_RESET = "WaitingForPasswordReset";
    }
}
