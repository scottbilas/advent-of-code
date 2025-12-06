<Query Kind="Statements">
  <NuGetReference>OkTools.Core</NuGetReference>
  <Namespace>OkTools.Core</Namespace>
  <Namespace>OkTools.Core.Extensions</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
</Query>

#load "lib"

var sample = """
	123 328  51 64
	 45 64  387 23
	  6 98  215 314
	*   +   *   +
	""";

var input = ReadInput(Util.CurrentScriptPath);

long Solve1(string input)
{
	var grid = input.Trim().Split('\n').Select(l => l.SplitTrimRemoveEmpty(' ')).ToArray();
	return Range(0, grid[0].Length)
		.Select(x => (
			o: grid[^1][x][0],
			n: grid[..^1].Select(l => long.Parse(l[x]))))
		.Sum(v => v.o == '*' ? v.n.Product() : v.n.Sum());
}

Check("Sample 1", () => Solve1(sample), 4277556);
Check("Problem 1", () => Solve1(input), 5322004718681);

long Solve2(string input)
{
	// force ws to be consistent (in case editor tools trim trailing whitespace)
	var lines = input.TrimEnd().Split('\n').Select(l => l.TrimEnd()).ToArray();
	var max = lines.Max(l => l.Length);
	for (var i = 0; i < lines.Length; ++i)
		lines[i] = lines[i].PadRight(max + 1);

	(int i, char op)[] probs = lines[^1]
		.Index().Where(v => v.Item != ' ')
		.Append((lines[0].Length, 'x'))
		.ToArray();

	return probs[..^1].Select((prob, i) => (
			o: prob.op,
			n: Range(prob.i, probs[i+1].i-1 - prob.i).Select(
				x => long.Parse(lines[..^1].Select(l => l[x]).StringJoin()))))
		.Sum(v => v.o == '*' ? v.n.Product() : v.n.Sum());
}

Check("Sample 2", () => Solve2(sample), 3263827);
Check("Problem 2", () => Solve2(input), 9876636978528);

Console.WriteLine("Everything is shiny!");
