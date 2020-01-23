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
	    public static IEnumerable<T> SelectTree<T>([NotNull] this ITreeRO<T> @this)
		    where T : ITreeRO<T> =>
		    @this.Children.SelectMany(c => SelectTree(c)).Prepend((T)@this);

	    public static IEnumerable<T> SelectTree<T>([NotNull] this ITreeRO<T> @this, [NotNull] Func<T, RecurseResult> shouldRecurse)
		    where T : ITreeRO<T>
	    {
		    IEnumerable<T> Walk(T node)
		    {
			    var result = shouldRecurse(node);

			    if (result != RecurseResult.StopAndSkip)
				    yield return node;

			    if (result != RecurseResult.Continue)
				    yield break;

			    foreach (var child in node.Children)
				    foreach (var n in Walk(child))
					    yield return n;
		    }

		    return Walk((T)@this);
	    }
    }
}
