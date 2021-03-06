using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2017
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<(T value, int index)> WithIndices<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Select((value, index) => (value, index));

        public static int IndexOf<T>([NotNull] this IEnumerable<T> @this, T value)
        {
            var i = 0;
            foreach (var item in @this)
            {
                if (item.Equals(value))
                    return i;
                ++i;
            }

            return -1;
        }

        public static bool IsValidIndex<T>([NotNull] this ICollection<T> @this, int index) =>
            index >= 0 && index < @this.Count;

        public static IEnumerable<T> Except<T>([NotNull] this IEnumerable<T> @this, T value) =>
            @this.Where(v => !v.Equals(value));

        public static int Resize<T>([NotNull] this List<T> @this, int newCount)
        {
            var oldCount = @this.Count;
            if (oldCount < newCount)
                @this.AddRange(newCount - oldCount);
            else if (oldCount > newCount)
                @this.RemoveRange(newCount, oldCount - newCount);

            return oldCount;
        }

        public static int EnsureSize<T>([NotNull] this List<T> @this, int minimumCount)
        {
            var oldCount = @this.Count;
            if (oldCount < minimumCount)
                @this.AddRange(minimumCount - oldCount);

            return oldCount;
        }

        public static void ExpandAndSetAt<T>([NotNull] this List<T> @this, int index, T value)
        {
            @this.EnsureSize(index + 1);
            @this[index] = value;
        }

        public static int AddRange<T>([NotNull] this ICollection<T> @this, IEnumerable<T> itemsToAdd)
        {
            var old = @this.Count;
            foreach (var item in itemsToAdd)
                @this.Add(item);
            return old;
        }

        public static int AddRange<T>([NotNull] this ICollection<T> @this, int numItemsToAdd, T value = default)
        {
            var old = @this.Count;
            for (var i = 0; i < numItemsToAdd; ++i)
                @this.Add(value);
            return old;
        }

        public static int AddRange<T>([NotNull] this ICollection<T> @this, int numItemsToAdd, Func<T> generator)
        {
            var old = @this.Count;
            for (var i = 0; i < numItemsToAdd; ++i)
                @this.Add(generator());
            return old;
        }

        public static int EnqueueRange<T>([NotNull] this Queue<T> @this, IEnumerable<T> items)
        {
            var i = 0;
            foreach (var item in items)
            {
                @this.Enqueue(item);
                ++i;
            }
            return i;
        }

        public static int EnqueueRange<T>([NotNull] this Queue<T> @this, params T[] items) =>
            @this.EnqueueRange(items.AsEnumerable());

        public static int RemoveRange<TK, TV>([NotNull] this IDictionary<TK, TV> @this, IEnumerable<TK> keys) =>
            keys.Count(@this.Remove);

        public static int WrapIndex<T>([NotNull] this ICollection<T> @this, int index) =>
            Utils.WrapIndex(index, @this.Count);

        public static int BounceIndex<T>([NotNull] this ICollection<T> @this, int index) =>
            Utils.BounceIndex(index, @this.Count);
    }
}
