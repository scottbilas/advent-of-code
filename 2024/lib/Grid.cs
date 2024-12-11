static partial class Extensions
{
    public static (Int2 size, IEnumerable<(Int2 coord, char value)> cells) ParseGrid(this string @this)
    {
        var lines = @this.SelectLinesAsSegments(true).ToList();
        if (lines.Any(l => l.Length != lines[0].Length))
            throw new InvalidOperationException("Non-regular grid");

        return (
            new Int2(lines[0].Length, lines.Count),
            lines.SelectMany((line, y) => line.Select((c, x) => (new Int2(x, y), c))));
    }

    public static IEnumerable<(Int2 coord, char value)> ParseGrid(this string @this, out Int2 size)
    {
        var result = @this.ParseGrid();
        size = result.size;
        return result.cells;
    }

    public static T[,] ToGrid<T>(this string @this, Func<char, T> selector) =>
        @this.ToGrid(selector, out var _);

    public static T[,] ToGrid<T>(this string @this, Func<char, T> selector, out Int2 size)
    {
        var result = @this.ParseGrid();
        size = result.size;

        var grid = new T[size.X, size.Y];
        foreach (var (coord, value) in result.cells)
            grid[coord.X, coord.Y] = selector(value);
        return grid;
    }

    public static char[,] ToGrid(this string @this) => @this.ToGrid(c => c);
    public static char[,] ToGrid(this string @this, out Int2 size) => @this.ToGrid(c => c, out size);

    public static int[,] ToIntGrid(this string @this) =>
        @this.ToIntGrid(out var _);
    public static int[,] ToIntGrid(this string @this, out Int2 size) =>
        @this.ToGrid(c => c.Int(), out size);


    public static T[,] Copy<T>(this T[,] @this) => (T[,])@this.Clone();

    public static T Get<T>(this T[,] @this, in Int2 pos) => @this[pos.X, pos.Y];
    public static T[,] Set<T>(this T[,] @this, in Int2 pos, T value) { @this[pos.X, pos.Y] = value; return @this; }

    public static T[,] Fill<T>(this T[,] @this, T value) =>
        @this.Fill(_ => value);

    public static T[,] Fill<T>(this T[,] @this, Func<Int2, T> generator)
    {
        foreach (var coord in @this.Coords())
            @this[coord.X, coord.Y] = generator(coord);

        return @this;
    }

    public static T[,] AddBorder<T>(this T[,] @this, T borderValue, int borderWidth = 1)
    {
        var (cx, cy) = @this.Size();
        var newGrid = new T[cx + borderWidth * 2, cy + borderWidth * 2];
        foreach (var (x, y) in newGrid.Bounds().BorderCoords(borderWidth))
            newGrid[x, y] = borderValue;
        foreach (var (pos, c) in @this.Cells())
            newGrid[pos.X + borderWidth, pos.Y + borderWidth] = c;
        return newGrid;
    }

    public static bool HasCoord<T>(this T[,] @this, Int2 coord) =>
        @this.Size().SizeToRect().HasCoord(coord);

    public static IEnumerable<Int2> Coords<T>(this T[,] @this) =>
        @this.Bounds().Coords();
    public static RectInt2 Bounds<T>(this T[,] @this) =>
        @this.Size().SizeToRect();
    public static Int2 Size<T>(this T[,] @this) =>
        new(@this.GetLength(0), @this.GetLength(1));

    public static IEnumerable<(Int2 pos, T cell)> Cells<T>(this T[,] @this, IEnumerable<Int2> coords) =>
        coords.Select(pos => (pos, @this[pos.X, pos.Y]));
    public static IEnumerable<(Int2 pos, T cell)> Cells<T>(this T[,] @this, in RectInt2 rect) =>
        @this.Cells(rect.Coords());
    public static IEnumerable<(Int2 pos, T cell)> Cells<T>(this T[,] @this) =>
        @this.Cells(@this.Bounds());

}
