using System;
using System.Reflection;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides transition key resolver functionality
    /// </summary>
    internal static class TransitionKeyResolver
    {
        #region Public Methods
        /// <summary>
        /// Tries to resolve transition key from static TransitionKey property
        /// </summary>
        public static string? TryToResolveTransitionKey(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            PropertyInfo? propertyInfo = type.GetProperty(
                "TransitionKey",
                BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo?.PropertyType == typeof(string))
            {
                object? value = propertyInfo.GetValue(null);
                if (value is string key && !string.IsNullOrWhiteSpace(key))
                {
                    return key;
                }
            }

            return null;
        }

        /// <summary>
        /// Builds transition key from identification fields
        /// </summary>
        public static string? TryToBuildTransitionKeyFromIdentification(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            string? domainName = GetStaticString(type, "DomainName");
            string? useCaseName = GetStaticString(type, "UseCaseName");
            string? applicationLayerName = GetStaticString(type, "ApplicationLayerName");
            string? stateName = GetStaticString(type, "StateName");
            string? transitionName = GetStaticString(type, "TransitionName");

            if (string.IsNullOrWhiteSpace(domainName) ||
                string.IsNullOrWhiteSpace(useCaseName) ||
                string.IsNullOrWhiteSpace(applicationLayerName) ||
                string.IsNullOrWhiteSpace(stateName) ||
                string.IsNullOrWhiteSpace(transitionName))
            {
                return null;
            }

            return domainName + "." +
                   useCaseName + "." +
                   applicationLayerName + "." +
                   stateName + "." +
                   transitionName;
        }
        #endregion

        #region Methods
        static string? GetStaticString(Type type, string propertyName)
        {
            PropertyInfo? propertyInfo = type.GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo?.PropertyType == typeof(string))
            {
                object? value = propertyInfo.GetValue(null);
                if (value is string str && !string.IsNullOrWhiteSpace(str))
                {
                    return str;
                }
            }

            return null;
        }
        #endregion
    }
}
