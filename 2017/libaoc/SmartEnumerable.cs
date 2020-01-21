using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Aoc2017
{
    public class SmartEnumerable<T> : IEnumerable<SmartEnumerable<T>.Element>
    {
        readonly IEnumerable<T> m_Source;

        public SmartEnumerable([NotNull] IEnumerable<T> source) =>
            m_Source = source;

        public IEnumerator<Element> GetEnumerator()
        {
            using (var e = m_Source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    for (var (index, isFirst, isLast) = (0, true, false); !isLast; ++index)
                    {
                        var current = e.Current;

                        isLast = !e.MoveNext();
                        yield return new Element(current, index, isFirst, isLast);
                        isFirst = false;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
             GetEnumerator();

        [DebuggerDisplay("(#{Index} f:{IsFirst} l:{IsLast})={Value}", Type = "{ValueType}")] // << TODO make this a DebuggerTypeProxy and get rid of ValueType
        public struct Element
        {
            internal Element(T value, int index, bool isFirst, bool isLast) =>
                (Value, Index, IsFirst, IsLast) = (value, index, isFirst, isLast);

            public T Value { get; }
            public int Index { get; }
            public bool IsFirst { get; }
            public bool IsLast { get; }

            [UsedImplicitly]
            string ValueType => typeof(T).FullName;
        }
    }

    public static partial class EnumerableExtensions
    {
        public static SmartEnumerable<T> AsSmartEnumerable<T>([NotNull] this IEnumerable<T> @this) =>
            new SmartEnumerable<T>(@this);
    }
}
