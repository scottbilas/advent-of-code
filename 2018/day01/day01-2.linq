<Query Kind="Statements" />

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

var input = Repeat(File
	.ReadLines(inputPath)
	.Select(int.Parse)
	.ToList());

var dups = FindDupFreqs(input);

var result = dups.First();
result.Dump();

IEnumerable<int> FindDupFreqs(IEnumerable<int> list)
{
	var freqs = new HashSet<int>();
	var freq = 0;
	foreach (var i in Repeat(input))
	{
		freq += i;
		if (!freqs.Add(freq))
			yield return freq;
	}
}

IEnumerable<int> Repeat(IEnumerable<int> list)
{
	for (;;)
	{
		foreach (var i in list)
			yield return i;
	}
}
