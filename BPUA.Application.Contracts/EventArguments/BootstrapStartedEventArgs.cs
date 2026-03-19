using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Event arguments for the "bootstrap started" lifecycle event.
    /// </summary>
    /// <remarks>
    /// Raised at the very beginning of the BPUA boot process,
    /// before any assemblies are loaded or services are registered.
    /// </remarks>
    public sealed class BootstrapStartedEventArgs : EventArgs
    {
    }
}
