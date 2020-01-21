using System.Linq;

namespace Aoc2017
{
    public enum Dir
    {
        W, N, E, S
    }

    public static partial class MiscStatics
    {
        public static readonly Dir[] Dirs = Arr(Dir.W, Dir.N, Dir.E, Dir.S);
    }

    public static partial class Extensions
    {
        static readonly (Dir dir, Dir reverse, string name, Int2 move)[] s_Dirs = new[]
        {
            (Dir.W, Dir.E, "west",  new Int2(-1,  0)),
            (Dir.N, Dir.S, "north", new Int2( 0, -1)),
            (Dir.E, Dir.W, "east",  new Int2( 1,  0)),
            (Dir.S, Dir.N, "south", new Int2( 0,  1)),
        };

        public static Int2 GetMove(this Dir @this) => s_Dirs[(int)@this].move;
        public static string GetName(this Dir @this) => s_Dirs[(int)@this].name;
        public static Dir GetReverse(this Dir @this) => s_Dirs[(int)@this].reverse;
        public static Dir ParseDir(this string @this) => s_Dirs.First(v => v.name == @this).dir;
    }
}
