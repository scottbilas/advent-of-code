using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AoC;
using Shouldly;

namespace Day17
{
    static class Solver
    {
        public static (int touched, int retained) CountReachableTiles((int x, int y) spring, string clayText)
        {
            var veins = Regex
                .Matches(clayText, @"([xy])=(\d+), ([xy])=(\d+)..(\d+)")
                .Select(m =>
                {
                    var left = int.Parse(m.Groups[2].Value);
                    var (from, to) = (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

                    return m.Groups[1].Value == "x"
                        ? (x0: left, y0: from, x1: left, y1: to)
                        : (x0: from, y0: left, x1: to, y1: left);
                })
                .ToList();

            var bounds = (
                left: veins.Min(l => l.x0), right: veins.Max(l => l.x1) + 1,
                top: veins.Min(l => l.y0), bottom: veins.Max(l => l.y1) + 1);

            var board = new char[bounds.right - bounds.left + 2, bounds.bottom - bounds.top].Fill('.');
            var offset = (x: bounds.left - 1, y: bounds.top);

            char GetCell((int x, int y) coord) => board[coord.x - offset.x, coord.y - offset.y];
            void SetCell((int x, int y) coord, char c) => board[coord.x - offset.x, coord.y - offset.y] = c;
            
            foreach (var (x0, y0, x1, y1) in veins)
            {
                if (y0 == y1)
                    for (var x = x0; x <= x1; ++x)
                        SetCell((x, y0), '#');
                else
                    for (var y = y0; y <= y1; ++y)
                        SetCell((x0, y), '#');
            }

            // ffwd to top
            var start = spring;
            while (start.y < bounds.top)
                ++start.y;
            
            Fill(start);

            void Fill((int x, int y) coord)
            {
                // seek bottom
                for (;;)
                {
                    if (coord.y >= bounds.bottom)
                        return;

                    var c = GetCell(coord);
                    if (c == '|')
                        return;
                    
                    if (c != '.')
                    {
                        --coord.y;
                        break;
                    }

                    SetCell(coord, '|');
                    ++coord.y;
                }

                for (;;--coord.y)
                {
                    // find bounds
                    var (left, hasLeft, right, hasRight) = (coord.x - 1, true, coord.x + 1, true);
    
                    for (;;--left)
                    {
                        if (left < bounds.left)
                        {
                            hasLeft = false;
                            break;
                        }
    
                        if (GetCell((left, coord.y)) == '#')
                        {
                            ++left;
                            break;
                        }

                        var c = GetCell((left, coord.y + 1)); 
                        if (c == '.' || c == '|')
                        {
                            hasLeft = false;
                            break;
                        }
                    }
                    
                    for (;;++right)
                    {
                        if (right >= bounds.right)
                        {
                            hasRight = false;
                            break;
                        }
    
                        if (GetCell((right, coord.y)) == '#')
                        {
                            --right;
                            break;
                        }

                        var c = GetCell((right, coord.y + 1));
                        if (c == '.' || c == '|')
                        {
                            hasRight = false;
                            break;
                        }
                    }
    
                    if (hasLeft && hasRight)
                    {
                        for (var x = left; x <= right; ++x)
                            SetCell((x, coord.y), '~');
                    }
                    else
                    {
                        for (var x = left; x <= right; ++x)
                            SetCell((x, coord.y), '|');

                        if (!hasLeft)
                            Fill((left, coord.y + 1));
                        if (!hasRight)
                            Fill((right, coord.y + 1));
                        
                        return;
                    }
                }
            }

            return (touched: board.SelectCells().Count(c => c.cell == '|' || c.cell == '~'),
                retained: board.SelectCells().Count(c => c.cell == '~'));
        }
    }
}
