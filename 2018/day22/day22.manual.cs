using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }

    struct SubCell
    {
        public int Connections;
        public int ShortestPath;
    }

    class Cell
    {
        public SubCell[] SubCells = new SubCell[3];

        public ref SubCell this[Equip equip] => ref SubCells[(int)equip];

        public Cell()
        {
            for (int i = 0; i < 3; ++i)
                SubCells[i].ShortestPath = int.MaxValue;
        }
    }

    class ManualSolver : Solver
    {
        public ManualSolver(Point targetPos, Size padding)
            : base(targetPos, padding) { }

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

            int shortest = int.MaxValue; //1020; //$$$$ int.MaxValue;

            int Walk(Point inPos, Equip inEquip, int inMinutes)
            {
                if (inMinutes + Math.Abs(TargetPos.X - inPos.X) + Math.Abs(TargetPos.Y - inPos.Y) >= shortest)
                    return int.MaxValue;

                var cell = grid[inPos.X, inPos.Y];
                if (cell[inEquip].ShortestPath < inMinutes)
                    return int.MaxValue;

                if (inPos == TargetPos && inEquip == Equip.Torch)
                {
                    shortest = Math.Min(shortest, inMinutes);
                    Debug.WriteLine(shortest);
                    return inMinutes;
                }

                cell[inEquip].ShortestPath = inMinutes;
                var conn = cell[inEquip].Connections;
                var minutes = int.MaxValue;

                IEnumerable<Mask> SelectMaskOrder()
                {
                    if (TargetPos.X > inPos.X)
                    {
                        yield return Mask.E;
                        yield return Mask.S;
                        yield return Mask.W;
                        yield return Mask.N;
                    }
                    else if (TargetPos.Y > inPos.Y)
                    {
                        yield return Mask.S;
                        yield return Mask.E;
                        yield return Mask.W;
                        yield return Mask.N;
                    }
                    else
                    {
                        yield return Mask.W;
                        yield return Mask.S;
                        yield return Mask.E;
                        yield return Mask.N;
                    }
                }

                for (var i = 0; i < 3; ++i)
                {
                    var equip = i + inEquip;
                    if (equip > Equip.Neither)
                        equip = 0;

                    foreach (var mask in SelectMaskOrder())
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

                return minutes;
            }

            return Walk(new Point(0, 0), Equip.Torch, 0);
        }

    }

}
