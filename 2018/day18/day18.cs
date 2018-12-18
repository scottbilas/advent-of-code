using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AoC;
using MoreLinq;

namespace Day18
{
    static class Solver
    {
        public static int Sim(int minutes, string yardText)
        {
            var yardLines = yardText.Split('\n').Select(l => l.Trim()).Where(l => l.Any()).ToList(); // << UTIL
            var board = new char[yardLines.Count, yardLines[0].Length].Fill(coord => yardLines[coord.y][coord.x]);
            var dims = board.GetDimensions();

            var next = (char[,])board.Clone();

            for (var minute = 0; minute < minutes; ++minute)
            {
                next.Fill(coord =>
                {
                    int tcount = 0, lcount = 0;
                    void Add(char c)
                    {
                        if (c == '#') ++lcount; else if (c == '|') ++tcount;
                    }
                    
                    if (coord.y > 0)
                    {
                        if (coord.x > 0)
                            Add(board[coord.x-1, coord.y-1]);
                        Add(board[coord.x, coord.y-1]);
                        if (coord.x < (dims.cx - 1))
                            Add(board[coord.x + 1, coord.y-1]);
                    }
                    if (coord.x > 0)
                        Add(board[coord.x - 1, coord.y]);
                    if (coord.x < (dims.cx - 1))
                        Add(board[coord.x + 1, coord.y]);
                    if (coord.y < (dims.cy - 1))
                    {
                        if (coord.x > 0)
                            Add(board[coord.x-1, coord.y+1]);
                        Add(board[coord.x, coord.y+1]);
                        if (coord.x < (dims.cx - 1))
                            Add(board[coord.x + 1, coord.y+1]);
                    }

                    char cc = board[coord.x, coord.y]; 
                    char nc = cc;
                    if (cc == '.' && tcount >= 3)
                        nc = '|';
                    else if (cc == '|' && lcount >= 3)
                        nc = '#';
                    else if (cc == '#' && (lcount < 1 || tcount < 1))
                        nc = '.';

                    return nc;
                });

                var last = board;
                board = next;
                next = last;
            }

            int a = 0, b= 0;
            foreach (var c in board.SelectCells())
            {
                if (c.cell == '#')
                    ++a;
                else if (c.cell == '|')
                    ++b;
            }

            Debug.WriteLine($"m:{minutes}, p:{a*b}");
            return a * b;
        }
    }
}
