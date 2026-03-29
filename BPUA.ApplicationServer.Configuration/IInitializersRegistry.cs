using System;
using System.Collections.Generic;

namespace BPUA.ApplicationServer.Configuration
{
    /// <summary>
    /// Defines initializers registry functionality
    /// </summary>
    public interface IInitializersRegistry : IList<Type>
    {
    }
}
