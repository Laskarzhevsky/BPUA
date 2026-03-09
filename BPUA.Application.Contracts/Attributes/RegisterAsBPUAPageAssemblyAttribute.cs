using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Marks an assembly as containing BPUA pages (UI) to be auto-registered with BPUA application
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class RegisterAsBPUAPageAssemblyAttribute : Attribute
    {
    }
}
