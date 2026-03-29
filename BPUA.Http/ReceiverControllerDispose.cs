using System;

using Microsoft.AspNetCore.Mvc;

namespace BPUA.Http
{
    /// <summary>
    /// Provides receiver controller functionality
    /// </summary>
    public partial class ReceiverController
    {
        #region Destructors
        /// <summary>
        /// Flag indicating whether object had been disposed
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Gets or sets flag indicating whether component is disposable, true by default
        /// </summary>
        public bool IsDisposable
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Disposes object
        /// IDisposable interface implementation
        /// </summary>
        [NonAction]
        public void Dispose()
        {
            if (IsDisposable)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        /// <param name="displosing">True for calling from Finalize method or False from distractor</param>
        [NonAction]
        public virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this.ReleaseManagedResources();
                }

                this.ReleaseUnmangedResources();
            }

            this._isDisposed = true;
        }

        /// <summary>
        /// Releases managed resources
        /// </summary>
        void ReleaseManagedResources()
        {
            OperatingSystem.Dispose();
            OperatingSystem = default!;
        }

        /// <summary>
        /// Releases unmanaged resources
        /// </summary>
        void ReleaseUnmangedResources()
        {
        }

        /// <summary>
        /// Object destructor
        /// </summary>
        ~ReceiverController()
        {
            Dispose(false);
        }
        #endregion
    }
}
