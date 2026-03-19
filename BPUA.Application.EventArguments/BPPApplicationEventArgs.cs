using System;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Provides BPUA application event arguments functionality
    /// </summary>
    public class BPUAApplicationEventArgs : ServiceRequestEventArgs
    {
        #region Public Methods
        /// <summary>
        /// Gets BPUA service key
        /// </summary>
        /// <returns>BPUA service key</returns>
        public virtual string GetBPUAServiceKey()
        {
            return GetType().Name;
        }
        #endregion
    }
}
