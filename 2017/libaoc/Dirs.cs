using System.Linq;
using static Aoc2017.MiscStatics;

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
        static readonly (Dir dir, Dir reverse, Dir[] continues, Dir[] others, string name, Int2 move)[] s_Dirs = new[]
        {
            (Dir.W, Dir.E, Arr(Dir.W, Dir.N, Dir.S), Arr(Dir.N, Dir.E, Dir.S), "west",  new Int2(-1,  0)),
            (Dir.N, Dir.S, Arr(Dir.N, Dir.E, Dir.W), Arr(Dir.E, Dir.S, Dir.W), "north", new Int2( 0, -1)),
            (Dir.E, Dir.W, Arr(Dir.E, Dir.S, Dir.N), Arr(Dir.S, Dir.W, Dir.N), "east",  new Int2( 1,  0)),
            (Dir.S, Dir.N, Arr(Dir.S, Dir.W, Dir.E), Arr(Dir.W, Dir.N, Dir.E), "south", new Int2( 0,  1)),
        };

        public static Int2 GetMove(this Dir @this) => s_Dirs[(int)@this].move;
        public static Int2 GetMove(this Dir @this, Int2 pos) => pos + @this.GetMove();
        public static string GetName(this Dir @this) => s_Dirs[(int)@this].name;
        public static Dir GetReverse(this Dir @this) => s_Dirs[(int)@this].reverse;
        public static Dir[] GetContinues(this Dir @this) => s_Dirs[(int)@this].continues;
        public static Dir[] GetOthers(this Dir @this) => s_Dirs[(int)@this].others;
        public static Dir ParseDir(this string @this) => s_Dirs.First(v => v.name == @this).dir;
    }
}
