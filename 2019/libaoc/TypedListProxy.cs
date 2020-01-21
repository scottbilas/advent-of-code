using System;
using System.Collections;
using System.Collections.Generic;

namespace Aoc2019
{
    public class TypedListProxy<T> : IList<T>
    {
        IList m_List;

        public TypedListProxy(IList list) => m_List = list;

        class Enumerator : IEnumerator<T>
        {
            IEnumerator m_Enumerator;

            public Enumerator(IEnumerator enumerator) => m_Enumerator = enumerator;

            public bool MoveNext() => m_Enumerator.MoveNext();
            public void Reset() => m_Enumerator.Reset();

            public T Current => (T)m_Enumerator.Current;
            object IEnumerator.Current => m_Enumerator.Current;

            public void Dispose() { }
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(m_List.GetEnumerator());
        IEnumerator IEnumerable.GetEnumerator() => m_List.GetEnumerator();

        public void Clear() => m_List.Clear();
        public void CopyTo(T[] array, int arrayIndex) => m_List.CopyTo(array, arrayIndex);

        public int Count => m_List.Count;
        public bool IsReadOnly => m_List.IsReadOnly;

        public int IndexOf(T item) => m_List.IndexOf(item);
        public bool Contains(T item) => m_List.Contains(item);

        public void Add(T item) => m_List.Add(item);

        public void RemoveAt(int index) => m_List.RemoveAt(index);

        public bool Remove(T item)
        {
            var found = IndexOf(item);
            if (found < 0)
                return false;

            m_List.Remove(item);
            return true;
        }

        public void Insert(int index, T item) => m_List.Insert(index, item);

        public T this[int index]
        {
            get => (T)m_List[index];
            set => m_List[index] = value;
        }
    }
}
