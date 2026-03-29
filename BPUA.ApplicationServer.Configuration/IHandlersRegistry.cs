using System;
using System.Collections.Generic;

namespace BPUA.ApplicationServer.Configuration
{
    /// <summary>
    /// Defines handlers registry functionality
    /// </summary>
    public interface IHandlersRegistry : IDictionary<string, Type>
    {
    }
}
