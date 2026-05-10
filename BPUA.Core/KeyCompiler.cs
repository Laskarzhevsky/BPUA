namespace BPUA.Core
{
    /// <summary>
    /// Provides key compiler functionality
    /// </summary>
    public static class KeyCompiler
    {
        #region Public Methods
        /// <summary>
        /// Compiles request handler key
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <returns>Compiled transition handler handler key</returns>
        public static string CompileHierarchicalRequestHandlerKey(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs)
        {
            return $"{domainName}_{useCaseName}_{applicationLayerName}_{stateName}_{transitionName}_{breadcrumbs}";
        }

        /// <summary>
        /// Compiles hosted application layer key
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <returns>Compiled hosted application layer key</returns>
        public static string CompileHostedApplicationLayerKey(string? domainName, string? useCaseName, string? applicationLayerName)
        {
            return $"{domainName}_{useCaseName}_{applicationLayerName}";
        }

        /// <summary>
        /// Compiles state handler key
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <returns>Compiled state handler key</returns>
        public static string CompileStateHandlerKey(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName)
        {
            return $"{domainName}_{useCaseName}_{applicationLayerName}_{stateName}";
        }

        /// <summary>
        /// Compiles state handler keys
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateNames">State names</param>
        /// <returns>Compiled state handler keys</returns>
        public static string[] CompileStateHandlerKeys(string? domainName, string? useCaseName, string? applicationLayerName, string[]? stateNames)
        {
            if (stateNames == null)
            {
                return new string[1] { $"{domainName}_{useCaseName}_{applicationLayerName}_" };
            }

            string[] compileStateHandlerKeys = new string[stateNames.Length];
            for (int i = 0; i < stateNames.Length; i++)
            {
                compileStateHandlerKeys[i] = $"{domainName}_{useCaseName}_{applicationLayerName}_{stateNames[i]}";
            }

            return compileStateHandlerKeys;
        }

        /// <summary>
        /// Compiles state handler visualizer key
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <returns>Compiled state handler visualizer key</returns>
        public static string CompileStateHandlerVisualizerKey(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName)
        {
            return $"/{domainName}_{useCaseName}_{applicationLayerName}_{stateName}";
        }

        /// <summary>
        /// Compiles state visualizer handler keyss
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateNames">State names</param>
        /// <returns>Compiled state handler visualizer keys</returns>
        public static string[] CompileStateHandlerVisualizerKeys(string? domainName, string? useCaseName, string? applicationLayerName, string[]? stateNames)
        {
            if (stateNames == null)
            {
                return new string[1] { $"/{domainName}_{useCaseName}_{applicationLayerName}_" };
            }

            string[] compileStateHandlerKeys = new string[stateNames.Length];
            for (int i = 0; i < stateNames.Length; i++)
            {
                compileStateHandlerKeys[i] = $"/{domainName}_{useCaseName}_{applicationLayerName}_{stateNames[i]}";
            }

            return compileStateHandlerKeys;
        }

        /// <summary>
        /// Compiles state prefix key
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <returns>Compiled transition handler handler key</returns>
        public static string CompileStatePrefixKey(string? requestName, string? domainName, string? useCaseName, string? applicationLayerName, string? stateName)
        {
            return $"{requestName}_{domainName}_{useCaseName}_{applicationLayerName}_{stateName}";
        }

        /// <summary>
        /// Compiles request handler key
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Compiled request handler key</returns>
        public static string CompileRequestHandlerKey(IBpuIdentifier bpuIdentifier)
        {
            return $"{bpuIdentifier.DomainName}_{bpuIdentifier.UseCaseName}_{bpuIdentifier.ApplicationLayerName}_{bpuIdentifier.StateName}_{bpuIdentifier.TransitionName}";
        }

        /// <summary>
        /// Compiles request handler key
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">transition name</param>
        /// <returns>Compiled request handler key</returns>
        public static string CompileRequestHandlerKey(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName)
        {
            return $"{domainName}_{useCaseName}_{applicationLayerName}_{stateName}_{transitionName}";
        }

        /// <summary>
        /// Compiles request handler keys
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateNames">State names</param>
        /// <param name="transitionName">transition name</param>
        /// <returns>Compiled request handler keys</returns>
        public static string[] CompileRequestHandlerKeys(string? domainName, string? useCaseName, string? applicationLayerName, string[]? stateNames, string? transitionName)
        {
            if (stateNames == null)
            {
                return new string[1] { $"{domainName}_{useCaseName}_{applicationLayerName}__{transitionName}" };
            }

            string[] compileStateHandlerKeys = new string[stateNames.Length];
            for (int i = 0; i < stateNames.Length; i++)
            {
                compileStateHandlerKeys[i] = $"{domainName}_{useCaseName}_{applicationLayerName}_{stateNames[i]}_{transitionName}";
            }

            return compileStateHandlerKeys;
        }

        /// <summary>
        /// Compiles request route key
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Compiled request route key</returns>
        public static string CompileRequestRouteKey(string requestName, IBpuIdentifier bpuIdentifier)
        {
            return $"{requestName}_{bpuIdentifier.DomainName}_{bpuIdentifier.UseCaseName}_{bpuIdentifier.ApplicationLayerName}_{bpuIdentifier.StateName}_{bpuIdentifier.TransitionName}";
        }

        /// <summary>
        /// Compiles request route key
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Compiled request route key</returns>
        public static string CompileRequestRouteKey(IBpuIdentifier bpuIdentifier)
        {
            return $"{bpuIdentifier.RequestName}_{bpuIdentifier.DomainName}_{bpuIdentifier.UseCaseName}_{bpuIdentifier.ApplicationLayerName}_{bpuIdentifier.StateName}_{bpuIdentifier.TransitionName}";
        }

        /// <summary>
        /// Compiles request route key
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">transition name</param>
        /// <returns>Compiled request route key</returns>
        public static string CompileRequestRouteKey(string? requestName, string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName)
        {
            return $"{requestName}_{domainName}_{useCaseName}_{applicationLayerName}_{stateName}_{transitionName}";
        }
        #endregion
    }
}
