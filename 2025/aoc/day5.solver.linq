<Query Kind="Statements">
  <NuGetReference>OkTools.Core</NuGetReference>
  <Namespace>OkTools.Core</Namespace>
  <Namespace>OkTools.Core.Extensions</Namespace>
  <Namespace>static System.Math</Namespace>
</Query>

#load "lib"

var sample = """
	3-5
	10-14
	16-20
	12-18

	1
	5
	8
	11
	17
	32
	""";

var input = ReadInput(Util.CurrentScriptPath);

(Long2[] fresh, long[] ingred) Parse(string text) => new(
	text.Blocks2.a.Lines.Array(l => l.PLong2),
	text.Blocks2.b.Longs);

int Solve1(string input)
{
	var (fresh, ingred) = Parse(input);
	return ingred.Count(i => fresh.Any(v => i >= v.X && i <= v.Y));
}

Check("Sample 1", () => Solve1(sample), 3);
Check("Problem 1", () => Solve1(input), 681);

long Solve2(string input) => Parse(input).fresh
	.OrderBy(v => v.X)
	.Append(Long2.MaxValue)
	.Aggregate(
		(count: 0L, cur: Long2.Zero, init: 1),
		(s, next) => s.init==1
			? (s.count, next, 0)
			: next.X <= s.cur.Y
				? (s.count, (s.cur.X, Max(s.cur.Y, next.Y)), 0)
				: (s.count + s.cur.Y - s.cur.X + 1, next, 0),
		s => s.count);

Check("Sample 2", () => Solve2(sample), 14);
Check("Problem 2", () => Solve2(input), 348820208020395);

Console.WriteLine("Everything is shiny!");
record Data;
