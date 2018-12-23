using System;
using System.Drawing;
using AoC;

namespace Day22
{
    class Mask
    {
        public int Bits;
        public int Dx, Dy;

        public Mask(int bits, int dx, int dy) => (Bits, Dx, Dy) = (bits, dx, dy);

        public static readonly Mask[] All = new Mask[]
        {
            new Mask(0b1000, 0, -1),
            new Mask(0b0100, 1, 0),
            new Mask(0b0010, -1, 0),
            new Mask(0b0001, 0, 1),
        };

        public static readonly Mask N = All[0];
        public static readonly Mask E = All[1];
        public static readonly Mask W = All[2];
        public static readonly Mask S = All[3];

        public const int Visited = 0b10000;
    }

    class Cell
    {
        public int[] Bits = new int[3];

        public int this[Equip equip]
        {
            get => Bits[(int)equip];
            set => Bits[(int)equip] = value;
        }
    }

    class ManualSolver : Solver
    {
        public ManualSolver(Point targetPos)
            : base(targetPos) { }

        public int CalcMinDistToTarget(int[,] erosion)
        {
            var board = BuildBoard(erosion);

            var grid = new Cell[Size.Width, Size.Height].Fill(_ => new Cell());
            for (var y = 0; y < Size.Height; ++y)
            {
                for (var x = 0; x < Size.Width; ++x)
                {
                    var cur = board[x, y];
                    var left = x > 0 ? board[x - 1, y] : 0;
                    var up = y > 0 ? board[x, y - 1] : 0;

                    void Connect(Equip equip, char a, char b)
                    {
                        if (left == a || left == b)
                        {
                            grid[x, y][equip] |= Mask.W.Bits;
                            grid[x - 1, y][equip] |= Mask.E.Bits;
                        }
                        if (up == a || up == b)
                        {
                            grid[x, y][equip] |= Mask.N.Bits;
                            grid[x, y - 1][equip] |= Mask.S.Bits;
                        }
                    }

                    if (cur == '.' || cur == '=')
                        Connect(Equip.Climbing, '.', '=');
                    if (cur == '.' || cur == '|')
                        Connect(Equip.Torch, '.', '|');
                    if (cur == '=' || cur == '|')
                        Connect(Equip.Neither, '=', '|');
                }
            }

            int shortest = 50;//int.MaxValue;

            int Walk(Point inPos, Equip inEquip, int inMinutes)
            {
                if (inMinutes + Math.Abs(TargetPos.X - inPos.X) + Math.Abs(TargetPos.Y - inPos.Y) >= shortest)
                    return int.MaxValue;

                var cell = grid[inPos.X, inPos.Y];
                if ((cell[inEquip] & Mask.Visited) != 0)
                    return int.MaxValue;

                if (inPos == TargetPos && inEquip == Equip.Torch)
                {
                    shortest = Math.Min(shortest, inMinutes);
                    //shortest.Dump();
                    //stack.Reverse().Select(s => $"{s.Item1.X},{s.Item1.Y}({s.Item2.ToString()[0]})={s.Item3}").StringJoin(' ').Dump();
                    return inMinutes;
                }

                //$"{inPos} {inEquip}".Dump();

                var conn = cell[inEquip] |= Mask.Visited;

                var minutes = int.MaxValue;

                for (var i = 0; i < 3; ++i)
                {
                    var equip = i + inEquip;
                    if (equip > Equip.Neither)
                        equip = 0;

                    foreach (var mask in Mask.All)
                    {
                        if ((conn & mask.Bits) != 0)
                        {
                            var subMinutes = inMinutes + 1;
                            if (inEquip != equip)
                                subMinutes += 7;

                            subMinutes = Walk(new Point(inPos.X + mask.Dx, inPos.Y + mask.Dy), equip, subMinutes);
                            minutes = Math.Min(minutes, subMinutes);
                        }
                    }
                }

                cell[inEquip] &= ~Mask.Visited;

                return minutes;
            }

            return Walk(new Point(0, 0), Equip.Torch, 0);
        }

    }

}
