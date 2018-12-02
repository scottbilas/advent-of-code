<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

FindCommon(
    new[] { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" })
    .ShouldBe(new[] { "fgij" });

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

FindCommon(
    File.ReadAllLines(inputPath))
    .Single()
    .Dump()
    .ShouldBe("pebjqsalrdnckzfihvtxysomg");

IEnumerable<string> FindCommon(string[] ids)
{
    for (int iremove = 0; iremove < ids[0].Length; ++iremove)
    {
        var found = new HashSet<string>();
        foreach (var id in ids)
        {
            var newId = id.Remove(iremove, 1);
            if (!found.Add(newId))
                yield return newId;
        }
    }
}

// one-liner

Enumerable
    .Range(0, 50)                                               // iterate through char indices we're going to try removing
    .Select(r => new { r, i = File.ReadAllLines(inputPath) })   // get data and pack it up
    .SelectMany(v => v.i                                        // for each char index
        .GroupBy(id => id.Remove(v.r, 1))                       // try removing that from everything and look for dups
        .Where(g => g.Count() > 1))                             // dups only
    .First().Key
    .ShouldBe("pebjqsalrdnckzfihvtxysomg");

Enumerable.Range(0, 50).Select(r => new { r, i = File.ReadAllLines(inputPath) }).SelectMany(v => v.i.GroupBy(id => id.Remove(v.r, 1)).Where(g => g.Count() > 1)).First().Key
    .ShouldBe("pebjqsalrdnckzfihvtxysomg");