<Query Kind="Statements" />

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

var result = File
	.ReadLines(inputPath)
	.Select(int.Parse)
	.Sum();
	
result.Dump();
