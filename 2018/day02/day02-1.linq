<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

var sampleChecksum = Checksum(new[] { "abcdef","bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" });
sampleChecksum.ShouldBe(12);

// input

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

var result = Checksum(File.ReadLines(inputPath));
result.ShouldBe(4693);

int Checksum(IEnumerable<string> ids)
{
	var letters = new int['z' - 'a' + 1];
	var doubles = 0;
	var triples = 0;
	
	foreach (var id in ids)
	{
		foreach (var c in id)
			++letters[c - 'a'];

		var doubled = false;
		var tripled = false;
		for (int i = 0; i < letters.Length; ++i)
		{
			doubled |= letters[i] == 2;
			tripled |= letters[i] == 3;
			letters[i] = 0;
		}
		
		if (doubled)
			++doubles;
		if (tripled)
			++triples;
	}
	
	return doubles * triples;
}

// the obligatory 'one-liner'...
File.ReadLines(inputPath).SelectMany(id => id.GroupBy(_ => _).GroupBy(g => g.Count())).GroupBy(c => c.Key).Where(c => c.Key > 1).Aggregate(1, (x, y) => x * y.Count()).ShouldBe(4693);
