using QuickGraph;

namespace Aoc2019
{
    public static class QuickGraphExtensions
    {
        public static UndirectedBidirectionalGraph<TV, TE> ToUndirected<TV, TE>(this IBidirectionalGraph<TV, TE> @this) where TE : IEdge<TV> =>
            new UndirectedBidirectionalGraph<TV, TE>(@this);
    }
}
