namespace BPUA.Account.Contracts
{
    /// <summary>
    /// Defines transition names
    /// </summary>
    public class TransitionsNames
    {
        /// <summary>
        /// Defines changing password transition
        /// </summary>
        public static string CHANGING_PASSWORD = "ChangingPassword";

        /// <summary>
        /// Defines logging in transition
        /// </summary>
        public static string LOGGING_IN = "LoggingIn";

        /// <summary>
        /// Defines registering transition
        /// </summary>
        public static string REGISTERING = "Registering";

        /// <summary>
        /// Defines resetting password transition
        /// </summary>
        public static string RESETTING_PASSWORD = "ResettingPassword";

        /// <summary>
        /// Defines switching to account registration transition
        /// </summary>
        public static string SWITCHING_TO_ACCOUNT_REGISTRATION = "SwitchingToAccountRegistration";

        /// <summary>
        /// Definnes switching to password change transition
        /// </summary>
        public static string SWITCHING_TO_PASSWORD_CHANGE = "SwitchingToPasswordChange";

        /// <summary>
        /// Definnes switching to password reset transition
        /// </summary>
        public static string SWITCHING_TO_PASSWORD_RESET = "SwitchingToPasswordReset";
    }
}
