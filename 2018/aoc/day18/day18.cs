using System.Collections.Generic;
using System.Linq;
using AoC;
using static AoC.Utils;

namespace Day18
{
    static class Solver
    {
        public static int Sim(int minutes, string yardText)
        {
            var board = yardText.ToGrid();
            var dims = board.GetDimensions();

            (int trees, int yards) CountThings(IEnumerable<char> cells) =>
                cells.Aggregate((trees: 0, yards: 0), (sum, c) => (
                    trees: sum.trees + (c == '|' ? 1 : 0),
                    yards: sum.yards + (c == '#' ? 1 : 0)));
            
            var next = (char[,])board.Clone();

            IEnumerable<int> Loop()
            {
                for (var minute = 0; minute < minutes; ++minute)
                {
                    next.Fill(coord =>
                    {
                        var counts = CountThings(coord
                            .SelectAdjacentWithDiagonals()
                            .Where(p => p.X >= 0 && p.X < dims.Width && p.Y >= 0 && p.Y < dims.Height)
                            .Select(p => board[p.X, p.Y]));
                        
                        var cc = board[coord.X, coord.Y]; 
                        switch (cc)
                        {
                            case '.' when counts.trees >= 3:
                                return '|';
                            case '|' when counts.yards >= 3:
                                return '#';
                            case '#' when (counts.yards < 1 || counts.trees < 1):
                                return '.';
                            default:
                                return cc;
                        }
                    });

                    Swap(ref board, ref next);

                    var result = CountThings(board.SelectCells().Select(c => c.cell));
                    yield return result.trees * result.yards;

                }
            }

            return Loop().PatternSeekingGetItemAt(minutes);
        }
    }
}
