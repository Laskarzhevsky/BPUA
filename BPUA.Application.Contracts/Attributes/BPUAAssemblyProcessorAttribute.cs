using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks class as BPUA assembly processor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BPUAAssemblyProcessorAttribute : Attribute
    {
    }
}
