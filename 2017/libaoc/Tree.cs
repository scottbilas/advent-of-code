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

        public override string ToString() => Key + (Children.Any() ? $" ({Children.Count})" : "");
    }

    public class Tree<TKey, TTag> : ITree<Tree<TKey, TTag>>, ITreeKey<TKey>, ITreeTag<TTag>
    {
        public Tree<TKey, TTag> Parent { get; set; }
        public IReadOnlyList<Tree<TKey, TTag>> Children => ChildList.OrEmpty();
        public List<Tree<TKey, TTag>> ChildList { get; set; }
        public TKey Key { get; set; }
        public TTag Tag { get; set; }

        public override string ToString() => $"{Key} [{Tag}]" + (Children.Any() ? $" ({Children.Count})" : "");
    }

    public static class Tree
    {
        public static Dictionary<TKey, TNode> Create<TKey, TNode, TLink>(
            [NotNull] IEnumerable<TLink> links,
            [NotNull] Func<TLink, (TKey parent, TKey child)> getKeysFunc,
            [NotNull] Func<TKey, TNode> createFunc,
            [CanBeNull] Action<TLink, TNode, TNode> applyLinkFunc)
            where TNode : ITree<TNode>
        {
            var forest = new Dictionary<TKey, TNode>();
            foreach (var link in links)
            {
                var (parentKey, childKey) = getKeysFunc(link);
                var parent = forest.GetOrAdd(parentKey, createFunc);
                var child = forest.GetOrAdd(childKey, createFunc);
                parent.AddChild(child);

                applyLinkFunc?.Invoke(link, parent, child);
            }
            return forest;
        }

        public class WithTagOperator<TTag>
        {
            internal static WithTagOperator<TTag> Instance = new WithTagOperator<TTag>();

            public Dictionary<TKey, Tree<TKey, TTag>> Create<TKey, TUserData>(
                [NotNull] Action<TUserData, Tree<TKey, TTag>, Tree<TKey, TTag>> applyLinkFunc,
                [NotNull] IEnumerable<(TUserData userData, TKey parent, TKey child)> links) =>
                Tree.Create(
                    links, link => (link.parent, link.child), key => new Tree<TKey, TTag> { Key = key },
                    (link, parent, child) => applyLinkFunc(link.userData, parent, child));

            // ReSharper disable MemberHidesStaticFromOuterClass
            public Dictionary<TKey, Tree<TKey, TTag>> Create<TKey>([NotNull] IEnumerable<(TKey parent, TKey child)> links) =>
                Tree.Create(links, link => (link.parent, link.child), key => new Tree<TKey, TTag> { Key = key }, null);
            public Dictionary<TKey, Tree<TKey, TTag>> Create<TKey>([NotNull] params (TKey parent, TKey child)[] links) =>
                Create(links.AsEnumerable());
            public Dictionary<TKey, Tree<TKey, TTag>> Create<TKey>([NotNull] IEnumerable<(TKey parent, TKey[] children)> links) =>
                Create(links.SelectMany(l => l.children.Select(c => (l.parent, c))));
            public Dictionary<TKey, Tree<TKey, TTag>> Create<TKey>([NotNull] IEnumerable<(TKey parent, IEnumerable<TKey> children)> links) =>
                Create(links.SelectMany(l => l.children.Select(c => (l.parent, c))));
            // ReSharper restore MemberHidesStaticFromOuterClass
        }

        public static WithTagOperator<TTag> WithTag<TTag>() => WithTagOperator<TTag>.Instance;

        public static Dictionary<TKey, Tree<TKey>> Create<TKey>([NotNull] IEnumerable<(TKey parent, TKey child)> links) =>
            Tree.Create(links, _ => _, key => new Tree<TKey> { Key = key }, null);
        public static Dictionary<TKey, Tree<TKey>> Create<TKey>([NotNull] params (TKey parent, TKey child)[] links) =>
            Create(links.AsEnumerable());

        public static Dictionary<TKey, Tree<TKey>> Create<TKey>([NotNull] IEnumerable<(TKey parent, TKey[] children)> links) =>
            Create(links.SelectMany(l => l.children.Select(c => (l.parent, c))));
        public static Dictionary<TKey, Tree<TKey>> Create<TKey>([NotNull] IEnumerable<(TKey parent, IEnumerable<TKey> children)> links) =>
            Create(links.SelectMany(l => l.children.Select(c => (l.parent, c))));
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
            where T : ITree<T>
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
