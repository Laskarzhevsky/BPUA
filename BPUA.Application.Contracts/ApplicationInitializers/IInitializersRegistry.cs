using System;
using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines initializers registry functionality
    /// </summary>
    public interface IInitializersRegistry : IList<Type>
    {
    }
}
