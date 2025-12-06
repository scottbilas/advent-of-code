<Query Kind="Statements" />

#load "lib"

var sample = """
	L68
	L30
	R48
	L5
	R60
	L55
	L1
	L99
	R14
	L82
	""";

var input = ReadInput(Util.CurrentScriptPath);

IEnumerable<(bool, int)> Parse(string text) =>
	text.Lines.Select(l => (l[0] == 'R', l[1..].Int));

int Solve1(string input)
{
	var (dial, count) = (50, 0);
	foreach (var (right, dist) in Parse(input))
	{
		dial = (dial + (right ? dist : -dist)) % 100;
		if (dial == 0)
			++count;
	}

	return count;
}

Check("Sample 1", () => Solve1(sample), 3);
Check("Problem 1", () => Solve1(input), 1081);

int Solve2(string input)
{
	var (dial, count) = (50, 0);
	foreach (var (right, dist) in Parse(input))
	{
		void swap() => dial = dial != 0 ? (100 - dial) : 0;

		if (!right) swap();
		dial += dist;
		count += dial / 100;
		dial %= 100;
		if (!right) swap();
	}

	return count;
}

Check("Sample 2", () => Solve2(sample), 6);
Check("Problem 2", () => Solve2(input), 6689);
Console.WriteLine("Everything is shiny!");
