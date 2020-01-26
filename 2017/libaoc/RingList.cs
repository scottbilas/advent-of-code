using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2017
{
    public static class Ring
    {
        public static RingArray<T> Array<T>(IEnumerable<T> items) => new RingArray<T>(items);
        public static RingList<T> List<T>(IEnumerable<T> items) => new RingList<T>(items);
    }

    public class RingArray<T> : IList<T>, IReadOnlyList<T>
    {
        [NotNull] T[] m_Data;

        public RingArray([NotNull] IEnumerable<T> items) => m_Data = items.ToArray();
        public RingArray(int count) => m_Data = new T[count];

        public int Length => m_Data.Length;
        public ref T this[int index] => ref m_Data[this.WrapIndex(index)];

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)m_Data).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_Data.GetEnumerator();

        int IReadOnlyCollection<T>.Count => Length;

        void ICollection<T>.Add(T item) => throw new InvalidOperationException();
        void ICollection<T>.Clear() => throw new InvalidOperationException();
        bool ICollection<T>.Contains(T item) => m_Data.Contains(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => m_Data.CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();
        int ICollection<T>.Count => Length;
        bool ICollection<T>.IsReadOnly => false;

        T IReadOnlyList<T>.this[int index] => m_Data[this.WrapIndex(index)];

        int IList<T>.IndexOf(T item) => m_Data.IndexOf(item);
        void IList<T>.Insert(int index, T item) => throw new InvalidOperationException();
        void IList<T>.RemoveAt(int index) => throw new InvalidOperationException();
        T IList<T>.this[int index] { get => m_Data[this.WrapIndex(index)]; set => m_Data[this.WrapIndex(index)] = value; }
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

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_Data).GetEnumerator();
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
