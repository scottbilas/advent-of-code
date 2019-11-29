using System.Drawing;
using System.Linq;
using Aoc2018;

namespace Day22
{
    enum Equip { Climbing, Torch, Neither };

    class Solver
    {
        public Point TargetPos { get; }
        public Size Size { get; }

        public Solver(Point targetPos, Size padding)
        {
            TargetPos = targetPos;
            Size = new Size(TargetPos.X + 1 + padding.Width, TargetPos.Y + 1 + padding.Height);
        }

        public int[,] BuildErosion(int caveDepth)
        {
            var erosion = new int[Size.Width, Size.Height];
            return erosion.Fill(pos =>
            {
                int geo;
                if ((pos.X == 0 && pos.Y == 0) || (pos.X == TargetPos.X && pos.Y == TargetPos.Y))
                    geo = 0;
                else if (pos.Y == 0)
                    geo = pos.X * 16807;
                else if (pos.X == 0)
                    geo = pos.Y * 48271;
                else
                    geo = erosion[pos.X - 1, pos.Y] * erosion[pos.X, pos.Y - 1];
                return (geo + caveDepth) % 20183;
            });
        }

        public int CalcRisk(int[,] erosion)
            => erosion
                .SelectCells(new Size(TargetPos.X + 1, TargetPos.Y + 1))
                .Select(cell => cell.cell % 3)
                .Sum();

        public char[,] BuildBoard(int[,] erosion)
            => new char[Size.Width, Size.Height]
                .Fill(p => ".=|"[erosion[p.X, p.Y] % 3]);
    }
}
