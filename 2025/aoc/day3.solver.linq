<Query Kind="Statements">
  <NuGetReference>OkTools.Core</NuGetReference>
  <Namespace>OkTools.Core</Namespace>
  <Namespace>OkTools.Core.Extensions</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static Utils</Namespace>
</Query>

#load "lib"

var sample = """
	987654321111111
	811111111111119
	234234234234278
	818181911112111
	""";

var input = ReadInput(Util.CurrentScriptPath);

long Solve(string input, int len) => input
	.Lines.Select(l => l.Array(c => c.Int))
	.Select(bank => Range(len)
		.Aggregate((result: 0L, start: 0, end: bank.Length-len+1), (v, _) =>
		{
			var (at, max) = bank[v.start..v.end].Index().MaxBy(v => v.Item);
			return (v.result*10 + max, v.start+at+1, v.end+1);
		}).result)
	.Sum();

long Solve1(string input) => Solve(input, 2);
Check("Sample 1", () => Solve1(sample), 357);
Check("Problem 1", () => Solve1(input), 17278);

long Solve2(string input) => Solve(input, 12);
Check("Sample 2", () => Solve2(sample), 3121910778619);
Check("Problem 2", () => Solve2(input), 171528556468625L);

Console.WriteLine("Everything is shiny!");
