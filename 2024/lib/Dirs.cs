class Dir
{
    public readonly string Name, NiceName;
    public readonly Int2 Move;
    public Dir Reverse = null!;
    public Dir Next4 = null!, Prev4 = null!;
    public Dir Next8 = null!, Prev8 = null!;

    public static readonly Dir W, NW, N, NE, E, SE, S, SW;
    public static readonly Dir[] All8, All4;

    Dir(string name, string niceName, Int2 move) => (Name, NiceName, Move) = (name, niceName, move);

    public override string ToString() => Name;

    static Dir()
    {
        All8 =
        [
            W  = new("W",  "west",       new Int2(-1,  0)),
            NW = new("NW", "northwest",  new Int2(-1, -1)),
            N  = new("N",  "north",      new Int2( 0, -1)),
            NE = new("NE", "northeast",  new Int2( 1, -1)),
            E  = new("E",  "east",       new Int2( 1,  0)),
            SE = new("SE", "southeast",  new Int2( 1,  1)),
            S  = new("S",  "south",      new Int2( 0,  1)),
            SW = new("SW", "southwest",  new Int2(-1,  1)),
        ];

        All4 = [ W, N, E, S ];

        static void Hookup(Dir dir, Dir reverse, Dir next4, Dir next8)
        {
            (dir.Reverse, dir.Next4, dir.Next8) = (reverse, next4, next8);
            dir.Reverse.Reverse = dir.Next4.Prev4 = dir.Next8.Prev8 = dir;
        }

        Hookup(W, E, N, NW); Hookup(NW, SE, N,  N);
        Hookup(N, S, E, NE); Hookup(NE, SW, E,  E);
        Hookup(E, W, S, SE); Hookup(SE, NW, S,  S);
        Hookup(S, N, W, SW); Hookup(SW, NE, W,  W);
    }
}
