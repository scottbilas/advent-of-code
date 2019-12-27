using RoyT.AStar;

namespace Aoc2019
{
    public static class RoyTAStarExtensions
    {
        public static Position ToPosition(this Int2 @this) => new Position(@this.X, @this.Y);
    }
}
