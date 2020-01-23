using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Coding.Utils;

namespace Aoc2017
{
    public interface ITreeRO<out T>
        where T : ITreeRO<T>
    {
        T Parent { get; }
        IReadOnlyList<T> Children { get; }
    }

    public interface ITreeKey<out T>
    {
        T Key { get; }
    }

    public interface ITreeTag<out T>
    {
        T Tag { get; }
    }

    public interface ITree<T> : ITreeRO<T>
        where T : ITree<T>
    {
        new T Parent { get; set; }
        List<T> ChildList { get; set; }
    }

    public class Tree<TKey> : ITree<Tree<TKey>>, ITreeKey<TKey>
    {
        public Tree<TKey> Parent { get; set; }
        public IReadOnlyList<Tree<TKey>> Children => ChildList.OrEmpty();
        public List<Tree<TKey>> ChildList { get; set; }
        public TKey Key { get; set; }
    }

    public class Tree<TKey, TTag> : ITree<Tree<TKey, TTag>>, ITreeKey<TKey>, ITreeTag<TTag>
    {
        public Tree<TKey, TTag> Parent { get; set; }
        public IReadOnlyList<Tree<TKey, TTag>> Children => ChildList.OrEmpty();
        public List<Tree<TKey, TTag>> ChildList { get; set; }
        public TKey Key { get; set; }
        public TTag Tag { get; set; }
    }

    public static class Tree
    {
        public static Dictionary<TKey, Tree<TKey, TTag>> Create<TSrc, TKey, TTag>(
            IEnumerable<TSrc> links,
            Func<TSrc, (TKey p, TKey c)> getKeysFunc,
            Action<TSrc, Tree<TKey, TTag>, Tree<TKey, TTag>> applyLinkFunc)
        {
            var forest = new Dictionary<TKey, Tree<TKey, TTag>>();
            foreach (var link in links)
            {
                var (p, c) = getKeysFunc(link);
                var pn = forest.GetOrAdd(p, _ => new Tree<TKey, TTag> { Key = p });
                var cn = forest.GetOrAdd(c, _ => new Tree<TKey, TTag> { Key = c });
                pn.AddChild(cn);

                applyLinkFunc?.Invoke(link, pn, cn);
            }
            return forest;
        }

        public static Dictionary<TKey, Tree<TKey, TTag>> Create<TUserData, TKey, TTag>(
            IEnumerable<(TUserData userData, TKey parent, TKey child)> links,
            Action<TUserData, Tree<TKey, TTag>, Tree<TKey, TTag>> applyLinkFunc)
        {
            var forest = new Dictionary<TKey, Tree<TKey, TTag>>();
            foreach (var (userData, parentKey, childKey) in links)
            {
                var parent = forest.GetOrAdd(parentKey, _ => new Tree<TKey, TTag> { Key = parentKey });
                var child = forest.GetOrAdd(childKey, _ => new Tree<TKey, TTag> { Key = childKey });
                parent.AddChild(child);

                applyLinkFunc(userData, parent, child);
            }
            return forest;
        }

        public static Dictionary<TKey, Tree<TKey>> Create<TKey>(
            IEnumerable<(TKey parent, TKey child)> links)
        {
            var forest = new Dictionary<TKey, Tree<TKey>>();
            foreach (var (parentKey, childKey) in links)
            {
                var parent = forest.GetOrAdd(parentKey, _ => new Tree<TKey> { Key = parentKey });
                var child = forest.GetOrAdd(childKey, _ => new Tree<TKey> { Key = childKey });
                parent.AddChild(child);
            }
            return forest;
        }

        public static Dictionary<TKey, Tree<TKey>> Create<TKey>(params (TKey parent, TKey child)[] links) =>
            Create(links.AsEnumerable());
    }

    public static class TreeExtensions
    {
        public static IEnumerable<T> SelectRoots<T>([NotNull] this IEnumerable<T> @this) where T : ITreeRO<T> =>
            @this.Where(v => v.Parent == null);

        public static IEnumerable<T> SelectKeys<T>([NotNull] this IEnumerable<ITreeKey<T>> @this) =>
            @this.Select(v => v.Key);

        public static TV GetRoot<TK, TV>([NotNull] this IDictionary<TK, TV> @this) where TV : ITreeRO<TV> =>
            @this.Values.SelectRoots().Single();

        public static T GetRoot<T>([NotNull] this IEnumerable<T> @this) where T : ITreeRO<T> =>
            @this.SelectRoots().Single();

        public static List<T> EnsureChildren<T>([NotNull] this T @this) where T : ITree<T> =>
            @this.ChildList ??= new List<T>();

        public static void AddChild<T>(this T @this, T child)
            where T : class, ITree<T>
        {
            if (ReferenceEquals(child.Parent, @this))
                return;
            if (!(child.Parent is null))
                throw new ArgumentException("Child already has a parent", nameof(child));

            child.Parent = @this;
            @this.EnsureChildren().Add(child);
        }
    }
}
