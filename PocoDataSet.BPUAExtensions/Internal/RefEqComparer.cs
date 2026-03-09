using System.Collections.Generic;

namespace PocoDataSet.BPUAExtensions.Internal
{
    internal sealed class RefEqComparer<T> : IEqualityComparer<T> where T : class
    {
        public static readonly RefEqComparer<T> Instance = new RefEqComparer<T>();

        public bool Equals(T? x, T? y)
        {
            return object.ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }
    }
}
