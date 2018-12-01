<Query Kind="Statements">
  <NuGetReference>System.Interactive</NuGetReference>
</Query>

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

var input = File
	.ReadLines(inputPath)
	.Select(int.Parse)
	.ToList()
	.Repeat();

var found = new HashSet<int>();
var result = input
	.Scan((freq, offset) => freq + offset)
	.Where(i => !found.Add(i))
	.First();

result.Dump();
