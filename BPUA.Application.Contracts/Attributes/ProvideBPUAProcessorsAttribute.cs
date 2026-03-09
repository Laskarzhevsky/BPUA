using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks an assembly that contains one or more IBPUAAssemblyProcessor implementations.
    /// The platform will compose these processors after assemblies are loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ProvideBPUAProcessorsAttribute : Attribute
    {
    }
}
