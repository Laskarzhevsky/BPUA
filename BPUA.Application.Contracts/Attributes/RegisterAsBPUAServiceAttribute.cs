using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks a class as BPUA service to be auto-registered with BPUA application
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegisterAsBPUAServiceAttribute : Attribute
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="serviceTypeKey">Service type key</param>
        public RegisterAsBPUAServiceAttribute(string? serviceTypeKey = null)
        {
            ServiceTypeKey = serviceTypeKey;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets service type key
        /// </summary>
        public string? ServiceTypeKey
        {
            get; set;
        }
        #endregion
    }
}
