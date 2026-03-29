using System;
using System.Collections.Generic;

namespace BPUA.ApplicationServer.Configuration
{
    /// <summary>
    /// Provides initializers registry functionality
    /// </summary>
    public class InitializersRegistry : List<Type>, IInitializersRegistry
    {
    }
}
