<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Text.RegularExpressions</Namespace>
</Query>

void Main()
{
    Solver
        .Sim(1000000000, File.ReadAllText(@"C:\proj\advent-of-code\2018\day18\input.txt"));
}

public static class Solver
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
                        Add(board[coord.x - 1, coord.y - 1]);
                    Add(board[coord.x, coord.y - 1]);
                    if (coord.x < (dims.cx - 1))
                        Add(board[coord.x + 1, coord.y - 1]);
                }
                if (coord.x > 0)
                    Add(board[coord.x - 1, coord.y]);
                if (coord.x < (dims.cx - 1))
                    Add(board[coord.x + 1, coord.y]);
                if (coord.y < (dims.cy - 1))
                {
                    if (coord.x > 0)
                        Add(board[coord.x - 1, coord.y + 1]);
                    Add(board[coord.x, coord.y + 1]);
                    if (coord.x < (dims.cx - 1))
                        Add(board[coord.x + 1, coord.y + 1]);
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

            int a = 0, b = 0;
            foreach (var c in board.SelectCells())
            {
                if (c.cell == '#')
                    ++a;
                else if (c.cell == '|')
                    ++b;
            }
            Debug.WriteLine($"m:{minute}, p:{a * b}");

            var last = board;
            board = next;
            next = last;

        }


        return 0;//a * b;
    }
}





public static class Extensions
{
    public static void Copy<T>(this IReadOnlyList<T> @this, int srcOffset, T[] dst, int dstOffset, int count)
    {
        for (var i = 0; i < count; ++i)
            dst[i + dstOffset] = @this[i + srcOffset];
    }

    public static T[] SliceArray<T>(this IReadOnlyList<T> @this, int offset, int count)
    {
        var sliced = new T[count];
        @this.Copy(offset, sliced, 0, count);
        return sliced;
    }

    public static TR FirstOrDefault<T, TR>(this IEnumerable<T> @this, Func<T, bool, TR> selector)
    {
        using (var e = @this.GetEnumerator())
            return e.MoveNext()
                ? selector(e.Current, true)
                : selector(default(T), false);
    }

    public static (int cx, int cy) GetDimensions<T>(this T[,] @this)
        => (cx: @this.GetLength(0), cy: @this.GetLength(1));

    public static IEnumerable<string> ToLines(this char[,] @this)
    {
        var (cx, cy) = @this.GetDimensions();

        var sb = new StringBuilder();
        for (var y = 0; y < cy; ++y)
        {
            sb.Clear();
            for (var x = 0; x < cx; ++x)
                sb.Append(@this[x, y]);
            yield return sb.ToString();
        }
    }

    public static string ToText(this char[,] @this)
        => string.Join("\n", @this.ToLines());

    public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this)
    {
        var (cx, cy) = @this.GetDimensions();
        for (var y = 0; y < cy; ++y)
            for (var x = 0; x < cx; ++x)
                yield return (cell: @this[x, y], x, y);
    }

    public static IEnumerable<(int x, int y)> SelectCoords<T>(this T[,] @this)
    {
        var (cx, cy) = @this.GetDimensions();
        for (var y = 0; y < cy; ++y)
            for (var x = 0; x < cx; ++x)
                yield return (x, y);
    }

    public static T[,] Fill<T>(this T[,] @this, T value)
        => @this.Fill(_ => value);

    public static T[,] Fill<T>(this T[,] @this, Func<(int x, int y), T> generator)
    {
        foreach (var coord in @this.SelectCoords())
            @this[coord.x, coord.y] = generator(coord);

        return @this;
    }

    public static Rectangle Bounds(this IEnumerable<Rectangle> @this)
    {
        var (l, t, r, b) = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
        foreach (var rect in @this)
        {
            l = Math.Min(l, rect.Left);
            t = Math.Min(t, rect.Top);
            r = Math.Max(r, rect.Right);
            b = Math.Max(b, rect.Bottom);
        }

        return Rectangle.FromLTRB(l, t, r, b);
    }

    public static Point BottomRight(in this Rectangle @this)
        => new Point(@this.Right, @this.Bottom);
}
