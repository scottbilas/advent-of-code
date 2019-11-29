using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AoC;

namespace Day17
{
    static class Solver
    {
        public static (int touched, int resting) CountReachableTiles(Point spring, string clayText)
        {
            var veins = Regex
                .Matches(clayText, @"([xy])=(\d+), ([xy])=(\d+)..(\d+)")
                .Select(m =>
                {
                    var left = int.Parse(m.Groups[2].Value);
                    var (from, to) = (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

                    return m.Groups[1].Value == "x"
                        ? (r: new Rectangle(left, from, 0, to - from + 1), s: new Size(0, 1))
                        : (r: new Rectangle(from, left, to - from + 1, 0), s: new Size(1, 0));
                })
                .ToList();

            var bounds = veins.Select(v => v.r).Bounds();
            bounds.Inflate(1, 0);
            ++bounds.Width;
            
            var board = new char[bounds.Right - bounds.Left, bounds.Bottom - bounds.Top].Fill('.');

            char GetCell(int x, int y) => board[x - bounds.Left, y - bounds.Top];
            void SetCell(int x, int y, char c) => board[x - bounds.Left, y - bounds.Top] = c;
            
            foreach (var (r, s) in veins)
                for (var p = r.Location; p != r.BottomRight(); p += s)
                    SetCell(p.X, p.Y, '#');

            Fill(new Point(spring.X, Math.Max(spring.Y, bounds.Top)));

            void Fill(Point coord)
            {
                // seek bottom
                for (;;++coord.Y)
                {
                    if (coord.Y >= bounds.Bottom)
                        return;

                    var c = GetCell(coord.X, coord.Y);
                    if (c == '|')
                        return;
                    
                    if (c != '.')
                    {
                        --coord.Y;
                        break;
                    }

                    SetCell(coord.X, coord.Y, '|');
                }

                // fill up
                for (;;--coord.Y)
                {
                    (int value, bool has) FindWall(int seek)
                    {
                        for (var x = coord.X + seek; ; x += seek)
                        {
                            if (x < bounds.Left || x >= bounds.Right)
                                return (x, false);

                            var c = GetCell(x, coord.Y + 1);
                            if (c == '.' || c == '|')
                                return (x, false);
                            
                            if (GetCell(x, coord.Y) == '#')
                                return (x - seek, true);
                        }
                    }
                    
                    // find bounds
                    var (left, hasLeft) = FindWall(-1);
                    var (right, hasRight) = FindWall(1);

                    void FillFlat(char c)
                    {
                        for (var x = left; x <= right; ++x)
                            SetCell(x, coord.Y, c);
                    }

                    if (!hasLeft || !hasRight)
                    {
                        FillFlat('|');

                        if (!hasLeft)
                            Fill(new Point(left, coord.Y + 1));
                        if (!hasRight)
                            Fill(new Point(right, coord.Y + 1));
                        
                        return;
                    }

                    FillFlat('~');
                }
            }

            var cells = board.SelectCells().Select(c => c.cell).ToLookup(c => c);
            var resting = cells['~'].Count();
            return (touched: cells['|'].Count() + resting, resting);
        }
    }
}
