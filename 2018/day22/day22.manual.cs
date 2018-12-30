using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
    }

    struct SubCell
    {
        public int Connections;
        public int TimeFromOrigin;
    }

    class Cell
    {
        public SubCell[] SubCells = new SubCell[3];

        public ref SubCell this[Equip equip] => ref SubCells[(int)equip];

        public Cell()
        {
            for (var i = 0; i < SubCells.Length; ++i)
                SubCells[i].TimeFromOrigin = int.MaxValue;
        }
    }

    class ManualSolver : Solver
    {
        public ManualSolver(Point targetPos, Size padding)
            : base(targetPos, padding) { }

        public int CalcMinDistToTarget(int[,] erosion)
        {
            var board = BuildBoard(erosion);

            var grid = new Cell[Size.Width, Size.Height].FillNew();
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
                            grid[x, y][equip].Connections |= Mask.W.Bits;
                            grid[x - 1, y][equip].Connections |= Mask.E.Bits;
                        }
                        if (up == a || up == b)
                        {
                            grid[x, y][equip].Connections |= Mask.N.Bits;
                            grid[x, y - 1][equip].Connections |= Mask.S.Bits;
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

            var shortest = int.MaxValue;
            var walking = new Queue<(Point pos, Equip equip, int minutes)>();
            walking.Enqueue((new Point(0, 0), Equip.Torch, 0));

            while (walking.Any())
            {
                var (inPos, inEquip, inMinutes) = walking.Dequeue();

                // early-out if we can't possibly get to target faster
                if (inMinutes + Math.Abs(TargetPos.X - inPos.X) + Math.Abs(TargetPos.Y - inPos.Y) >= shortest)
                    continue;

                // early-out if an earlier and faster route to the same subcell was found
                var cell = grid[inPos.X, inPos.Y];
                if (cell[inEquip].TimeFromOrigin <= inMinutes)
                    continue;

                // detect if we hit target
                if (inPos == TargetPos && inEquip == Equip.Torch)
                {
                    shortest = Math.Min(shortest, inMinutes);
                    Debug.WriteLine(shortest);
                }

                // new winner
                cell[inEquip].TimeFromOrigin = inMinutes;

                for (var i = 0; i < 3; ++i)
                {
                    var equip = i + inEquip;
                    if (equip > Equip.Neither)
                        equip = 0;

                    foreach (var mask in Mask.All)
                    {
                        if ((cell[equip].Connections & mask.Bits) != 0)
                        {
                            var subMinutes = inMinutes + 1;
                            if (equip != inEquip)
                                subMinutes += 7;

                            walking.Enqueue((new Point(inPos.X + mask.Dx, inPos.Y + mask.Dy), equip, subMinutes));
                        }
                    }
                }
            }

            return shortest;
        }
    }
}
