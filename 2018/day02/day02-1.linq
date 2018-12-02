<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

Checksum(
    new[] { "abcdef","bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" })
    .ShouldBe(12);

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

Checksum(
    File.ReadLines(inputPath))
    .Dump()
    .ShouldBe(4693);

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

// one-liner

File
    .ReadLines(inputPath)                    // get ids
    .SelectMany(id => id                     // flatten each word's letter dup counts
        .GroupBy(i => i)                     //   - find dup letters
        .GroupBy(g => g.Count()))            //   - ignore when we find multiple letters with same dup count
    .GroupBy(c => c.Key)                     // group so we can count dup counts across words
    .Where(c => c.Key == 2 || c.Key == 3)    // only want doubles and triples
    .Aggregate(1, (x, y) => x * y.Count())   // fun way to multiply doubles by triples
    .ShouldBe(4693);

File.ReadLines(inputPath).SelectMany(id => id.GroupBy(i => i).GroupBy(g => g.Count())).GroupBy(c => c.Key).Where(c => c.Key == 2 || c.Key == 3).Aggregate(1, (x, y) => x * y.Count())
    .ShouldBe(4693);
