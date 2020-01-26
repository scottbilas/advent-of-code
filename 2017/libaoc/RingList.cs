using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2017
{
    public static class RingExtensions
    {
        public static RingArray<T> ToRingArray<T>(this IEnumerable<T> @this) => new RingArray<T>(@this);
        public static RingList<T> ToRingList<T>(this IEnumerable<T> @this) => new RingList<T>(@this);
    }

    public class RingArray<T> : IList<T>, IReadOnlyList<T>
    {
        [NotNull] public T[] Data;
        public int Offset;

        public RingArray([NotNull] IEnumerable<T> items) => Data = items.ToArray();
        public RingArray(int count) => Data = new T[count];

        public int Length => Data.Length;
        public ref T this[int index] => ref Data[ResolveIndex(index)];

        int ResolveIndex(int index) => this.WrapIndex(index + Offset);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
            Data.Repeat().Skip(ResolveIndex(0)).Take(Length).GetEnumerator();

        // it's an array, so these are all read-only (we still want the interface for better algo reuse via the other members)

        void ICollection<T>.Add(T item) => throw new ReadOnlyException();
        void ICollection<T>.Clear() => throw new ReadOnlyException();
        bool ICollection<T>.Remove(T item) => throw new ReadOnlyException();
        void IList<T>.Insert(int index, T item) => throw new ReadOnlyException();
        void IList<T>.RemoveAt(int index) => throw new ReadOnlyException();

        // simple forwarders

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
        int IReadOnlyCollection<T>.Count => Length;
        bool ICollection<T>.Contains(T item) => Data.Contains(item);
        int ICollection<T>.Count => Length;
        bool ICollection<T>.IsReadOnly => false;

        // index-processing forwarders

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            var start = ResolveIndex(0);
            var count = Length - start;

            Array.Copy(Data, start, array, arrayIndex, count);
            Array.Copy(Data, 0, array, arrayIndex + count, Length - count);
        }

        T IReadOnlyList<T>.this[int index] => Data[ResolveIndex(index)];

        int IList<T>.IndexOf(T item) => Data.IndexOf(item) - Offset;

        T IList<T>.this[int index]
        {
            get => Data[ResolveIndex(index)];
            set => Data[ResolveIndex(index)] = value;
        }
    }

    public class RingList<T> : IList<T>, IReadOnlyList<T>
    {
        [NotNull] List<T> m_Data;

        public RingList([NotNull] IEnumerable<T> items) => m_Data = items.ToList();

        public RingList(int count)
        {
            m_Data = new List<T>();
            m_Data.Resize(count);
        }

        // simple forwarders

        IEnumerator IEnumerable.GetEnumerator() => m_Data.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => m_Data.GetEnumerator();
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)m_Data).IsReadOnly;
        public void Add(T item) => m_Data.Add(item);
        public void Clear() => m_Data.Clear();
        public bool Contains(T item) => m_Data.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => m_Data.CopyTo(array, arrayIndex);
        public int Count => m_Data.Count;
        public int IndexOf(T item) => m_Data.IndexOf(item);
        public bool Remove(T item) => m_Data.Remove(item);

        // wrapped forwarders

        public void Insert(int index, T item) =>
            m_Data.Insert(this.WrapIndex(index), item);

        public void RemoveAt(int index) =>
            m_Data.RemoveAt(this.WrapIndex(index));

        public T this[int index]
        {
            get => m_Data[this.WrapIndex(index)];
            set => m_Data[this.WrapIndex(index)] = value;
        }
    }
}
