namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition names
    /// </summary>
    public class TransitionsNames
    {
        /// <summary>
        /// InitializingApplication transition
        /// </summary>
        public static string INITIALIZING_APPLICATION = "InitializingApplication";

        /// <summary>
        /// Initializing use case
        /// </summary>
        public static string INITIALIZING_USE_CASE = "InitializingUseCase";

        /// <summary>
        /// LoadingUseCaseAssemblies transition
        /// </summary>
        public static string LOADING_USE_CASE_ASSEMBLIES = "LoadingUseCaseAssemblies";

        /// <summary>
        /// SwitchingToUseCase transition
        /// </summary>
        public static string SWITCHING_TO_USE_CASE = "SwitchingToUseCase";
    }
}
