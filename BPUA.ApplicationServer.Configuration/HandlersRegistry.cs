using System;
using System.Collections.Generic;

namespace BPUA.ApplicationServer.Configuration
{
    /// <summary>
    /// Provides handlers registry functionality
    /// </summary>
    public class HandlersRegistry : Dictionary<string, Type>, IHandlersRegistry
    {
    }
}
