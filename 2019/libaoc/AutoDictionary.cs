using System;
using System.Collections;
using System.Collections.Generic;

namespace Aoc2019
{
    /// <summary>
    /// This is exactly like a Dictionary except instead of throwing on a failed lookup of `dict[key]` it will instead
    /// call the `GetDefault` (given in the ctor) to populate the entry and then return it.
    /// </summary>
    public class AutoDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> m_Dictionary;
        readonly Func<TKey, TValue> m_GetDefault;

        public AutoDictionary(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> getDefault)
            => (m_Dictionary, m_GetDefault) = (dictionary, getDefault);
        public AutoDictionary(Func<TKey, TValue> getDefault)
            : this(new Dictionary<TKey, TValue>(), getDefault) { }
        public AutoDictionary(IDictionary<TKey, TValue> dictionary, TValue defaultValue = default)
            : this(dictionary, _ => defaultValue) { }
        public AutoDictionary(TValue defaultValue = default)
            : this(_ => defaultValue) { }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => m_Dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => m_Dictionary.GetEnumerator();

        public void Add(TKey key, TValue value)
            => m_Dictionary.Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Add(item);
        public void Clear()
            => m_Dictionary.Clear();
        public bool ContainsKey(TKey key)
            => m_Dictionary.ContainsKey(key);
        public bool Contains(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Contains(item);
        public int Count
            => m_Dictionary.Count;
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => m_Dictionary.CopyTo(array, arrayIndex);
        public bool IsReadOnly
            => m_Dictionary.IsReadOnly;
        public bool Remove(TKey key)
            => m_Dictionary.Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Remove(item);
        public bool TryGetValue(TKey key, out TValue value)
            => m_Dictionary.TryGetValue(key, out value);


        public ICollection<TKey> Keys
            => m_Dictionary.Keys;
        public ICollection<TValue> Values
            => m_Dictionary.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
            => m_Dictionary.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
            => m_Dictionary.Values;

        public TValue this[TKey key]
        {
            get
            {
                if (!m_Dictionary.TryGetValue(key, out var value))
                    m_Dictionary.Add(key, value = m_GetDefault(key));
                return value;
            }
            set => m_Dictionary[key] = value;
        }
    }
}
