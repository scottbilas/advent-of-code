<Query Kind="Statements">
  <NuGetReference>OkTools.Core</NuGetReference>
  <Namespace>OkTools.Core</Namespace>
  <Namespace>OkTools.Core.Extensions</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static Utils</Namespace>
</Query>

#load "lib"

var sample = """
	..@@.@@@@.
	@@@.@.@.@@
	@@@@@.@.@@
	@.@@@@..@.
	@@.@@@@.@@
	.@@@@@@@.@
	.@.@.@.@@@
	@.@@@.@@@@
	.@@@@@@@@.
	@.@.@@@.@.
	""";

var input = ReadInput(Util.CurrentScriptPath);

IEnumerable<Int2> Available(char[,] grid) =>
	grid.Coords.Where(c => grid.At(c) == '@' &&
		Int2.Off8.Count(o => grid.SafeAt(c+o) == '@') < 4);

int Solve1(string input) => Available(input.Grid).Count();

Check("Sample 1", () => Solve1(sample), 13);
Check("Problem 1", () => Solve1(input), 1626);

int Solve2(string input)
{
	var grid = input.Grid;

	for (var total = 0;;)
	{
		var old = total;
		foreach (var c in Available(grid))
		{
			grid.At(c) = '.';
			++total;
		}
		if (total == old)
			return total;
	}
}

Check("Sample 2", () => Solve2(sample), 43);
Check("Problem 2", () => Solve2(input), 9173);

Console.WriteLine("Everything is shiny!");
