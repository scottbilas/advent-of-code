<Query Kind="Statements">
  <NuGetReference>OkTools.Core</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>OkTools.Core</Namespace>
  <Namespace>OkTools.Core.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

string ReadInput(string scriptPath)
{
	var npath = scriptPath.ToNPath();
	return npath.ChangeFilename(npath.FileName.Split('.')[0] + ".input.txt").ReadAllText();
}

void Check<T>(string name, Func<T> pred, T answer)
{
	var start = DateTime.UtcNow;
	var result = pred();
	var delta = DateTime.UtcNow - start;
	result.ShouldBe(answer);
	$"Result: {result}\n{delta.TotalMilliseconds}ms".Dump(name);
}

static class Utils
{
	public static IEnumerable<int> RangeExtent(int start, int end) => Enumerable.Range(start, end-start);
	public static IEnumerable<int> Range(int count) => Enumerable.Range(0, count);

	public static TO[] Array<TI, TO>(this IEnumerable<TI> @this, Func<TI, TO> pred) => @this.Select(pred).ToArray();

	public static string[] Trim(this IEnumerable<string> @this) => @this.Array(l => l.Trim());

	static readonly Int2[] s_off4 = [
			new(0, -1),              new( 1, 0),
			new(0,  1),              new(-1, 0)];
	static readonly Int2[] s_off8 = [
			new(0, -1), new( 1, -1), new( 1, 0), new( 1, 1),
			new(0,  1), new(-1,  1), new(-1, 0), new(-1, -1)];

	extension(Int2)
	{
		public static Int2[] Off4 => s_off4;
		public static Int2[] Off8 => s_off8;
	}

	extension<T>(T[,] @this)
	{
		public int Cx => @this.GetLength(0);
		public int Cy => @this.GetLength(1);
		public Int2 Size => new(@this.Cx, @this.Cy);
		public Int4 Rect => new(0, 0, @this.Cx, @this.Cy);

		public IEnumerable<Int2> Coords
		{
			get
			{
				var size = @this.Size;
				for (var y = 0; y < size.X; ++y)
					for (var x = 0; x < size.Y; ++x)
						yield return new Int2(x, y);

			}
		}
		
		public ref T At(Int2 c) => ref @this[c.X, c.Y];

		public T SafeAt(int x, int y, T def = default) => x >= 0 && x < @this.Cx && y >= 0 && y < @this.Cy ? @this[x, y] : def;
		public T SafeAt(Int2 c, T def = default) => @this.SafeAt(c.X, c.Y, def);

		public T[,] Fill(Func<Int2, T> generator)
		{
			foreach (var coord in @this.Coords)
				@this[coord.X, coord.Y] = generator(coord);

			return @this;
		}

		public T[,] Fill(T fill) => @this.Fill(_ => fill);

		public T[,] AddBorder(T fill = default)
		{
			var size = @this.Size;
			var newGrid = new T[size.X+2, size.Y+2];
			newGrid.Fill(c =>
			{
				if (c.X == 0 || c.Y == 0 || c.X == size.X+1 || c.Y == size.Y+1)
					return fill;
				return @this[c.X-1, c.Y-1];
			});
			return newGrid;
		}
	}

	extension(string @this)
	{
		public IEnumerable<Match> SelectMatches(string pattern) => Regex.Matches(@this, pattern);
		public Match[] Matches(string pattern) => @this.SelectMatches(pattern).ToArray();

		public int Int => int.Parse(@this);
		public int[] PInts => @this.Matches(@"\d+").Select(m => m.Value.Int).ToArray();
		public int[] Ints => @this.Matches(@"[-+]?\d+").Select(m => m.Value.Int).ToArray();
		public Int2 PInt2 => new(@this.PInts.ToTuple2());
		public Int2 Int2 => new(@this.Ints.ToTuple2());

		public long Long => long.Parse(@this);
		public long[] PLongs => @this.Matches(@"\d+").Select(m => m.Value.Long).ToArray();
		public long[] Longs => @this.Matches(@"[-+]?\d+").Select(m => m.Value.Long).ToArray();
		public Long2 PLong2 => new(@this.PLongs.ToTuple2());
		public Long2 Long2 => new(@this.Longs.ToTuple2());

		public string[] Lines => @this.TrimEnd().Replace("\r", "").Split('\n');
		public string[] LinesTrim => @this.Lines.Trim();

		public string[] Blocks => @this.Replace("\r", "").Split("\n\n");
		public (string a, string b) Blocks2 => @this.Blocks.ToTuple2();

		public char[,] Grid
		{
			get
			{
				var lines = @this.LinesTrim;
				if (lines.Any(l => l.Length != lines[0].Length))
					throw new InvalidOperationException("Non-regular grid");

				return new char[lines[0].Length, lines.Length]
					.Fill(coord => lines[coord.Y][coord.X]);
			}
		}
	}

	extension(char @this)
	{
		public int Int => (@this >= '0' && @this <= '9') ? @this - '0' : throw new Exception($"Invalid char: {@this}");
	}
}
