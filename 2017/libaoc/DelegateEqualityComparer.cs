using System;
using System.Collections.Generic;

namespace Aoc2017
{
    public static class DelegateEqualityComparer
    {
        struct Comparer<T> : IEqualityComparer<T>
        {
            readonly Func<T, T, bool> m_Equals;
            readonly Func<T, int> m_GetHashCode;

            public Comparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                m_Equals = equals;
                m_GetHashCode = getHashCode;
            }

            bool IEqualityComparer<T>.Equals(T x, T y) => m_Equals(x, y);
            int IEqualityComparer<T>.GetHashCode(T obj) => m_GetHashCode(obj);
        }

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
            => new Comparer<T>(equals, getHashCode);

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals)
            => Create(equals, _ => throw new NotImplementedException());
    }
}
