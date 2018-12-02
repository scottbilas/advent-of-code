<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

var sampleCommon = FindCommon(new[] { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" });
sampleCommon.ShouldBe(new[] { "fgij" });

// input

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

var result = FindCommon(File.ReadAllLines(inputPath));
result.Dump();

IEnumerable<string> FindCommon(string[] ids)
{
	for (int iremove = 0; iremove < ids[0].Length; ++iremove)
	{
		var found = new HashSet<string>();
		foreach (var id in ids)
		{
			var newId = id.Substring(0, iremove) + id.Substring(iremove + 1, id.Length - iremove - 1);
			if (!found.Add(newId))
				yield return newId;
		}
	}
}
