
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aoc2017
{
    public static class Comparers
    {
        class StructuralEqualityComparer<TContainer, TElement> : IEqualityComparer<TContainer>
            where TContainer : IStructuralEquatable
            where TElement : IEquatable<TElement>
        {
            static IEqualityComparer s_Comparer = EqualityComparer<TElement>.Default;

            public static StructuralEqualityComparer<TContainer, TElement> Instance { get; }
                = new StructuralEqualityComparer<TContainer, TElement>();

            public bool Equals([AllowNull] TContainer a, [AllowNull] TContainer b)
            {
                if (ReferenceEquals(a, b))
                    return true;
                if (a is null || b is null)
                    return false;
                return ((IStructuralEquatable)a).Equals(b, s_Comparer);
            }

            public int GetHashCode([DisallowNull] TContainer obj) =>
                ((IStructuralEquatable)obj).GetHashCode(s_Comparer);
        }

        public static IEqualityComparer<T[]> Array<T>() where T : IEquatable<T> =>
            StructuralEqualityComparer<T[], T>.Instance;
        public static bool ArrayEquals<T>(T[] a, T[] b) where T : IEquatable<T> =>
            StructuralEqualityComparer<T[], T>.Instance.Equals(a, b);
    }
}
