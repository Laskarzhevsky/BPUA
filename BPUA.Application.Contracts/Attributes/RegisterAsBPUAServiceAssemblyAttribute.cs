using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks an assembly as containing BPUA services to be auto-registered with BPUA application
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class RegisterAsBPUAServiceAssemblyAttribute : Attribute
    {
    }
}
