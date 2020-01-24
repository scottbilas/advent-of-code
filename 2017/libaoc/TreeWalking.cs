using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2017
{
	public enum RecurseResult
	{
		Continue,		// keep recursing
		StopAndKeep,	// stop recursing but keep this node
		StopAndSkip,	// stop recursing but skip this node
	}

	public static class TreeWalkingExtensions
    {
	    public static IEnumerable<T> SelectTree<T>([NotNull] this T @this)
		    where T : ITreeRO<T> =>
		    @this.Children.SelectMany(SelectTree).Prepend(@this);

	    public static IEnumerable<(T node, int depth)> SelectTreeWithIndices<T>([NotNull] this T @this)
		    where T : ITreeRO<T>
	    {
		    static IEnumerable<(T, int)> Walk(T node, int depth)
		    {
			    yield return (node, depth);
			    foreach (var item in node.Children.SelectMany(c => Walk(c, depth + 1)))
				    yield return item;
		    }

		    return Walk(@this, 0);
	    }

	    public static IEnumerable<T> SelectTree<T>([NotNull] this T @this, [NotNull] Func<T, RecurseResult> shouldRecurse)
		    where T : ITreeRO<T>
	    {
		    var result = shouldRecurse(@this);

		    if (result != RecurseResult.StopAndSkip)
			    yield return @this;

		    if (result != RecurseResult.Continue)
			    yield break;

		    foreach (var child in @this.Children)
			    foreach (var n in SelectTree(child))
				    yield return n;
	    }

		public static IEnumerable<T> SelectAncestors<T>([NotNull] this T @this)
			where T : ITreeRO<T>
		{
			for (var i = @this.Parent; i != null; i = i.Parent)
				yield return i;
		}

	    public static int CalcDepth<T>([NotNull] this T @this)
			where T : ITreeRO<T> =>
			@this.SelectAncestors().Count();

	    public static IEnumerable<string> Render<T>([NotNull] this T @this)
			where T : ITreeRO<T> =>
		    @this.SelectTreeWithIndices().Select(iter => new string(' ', iter.depth * 2) + iter.node);
    }
}
