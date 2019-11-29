<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// samples

CalcFreq(new[] { "+1", "-2", "+3", "+1" }).ShouldBe(2);
CalcFreq(new[] { "+1", "-1" }).ShouldBe(0);
CalcFreq(new[] { "+3", "+3", "+4", "-2", "-4" }).ShouldBe(10);
CalcFreq(new[] { "-6", "+3", "+8", "+5", "-6" }).ShouldBe(5);
CalcFreq(new[] { "+7", "+7", "-2", "-7", "-4" }).ShouldBe(14);

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

CalcFreq(File.ReadLines(inputPath)).Dump().ShouldBe(566);

int CalcFreq(IEnumerable<string> changes)
{
    var input = changes
        .Select(int.Parse)
        .ToList()
        .Repeat();

    var found = new HashSet<int>();
    return input
        .Scan((freq, offset) => freq + offset)
        .First(i => !found.Add(i));
}

// one-liner

File
    .ReadLines(inputPath)                       // get data
    .Select(int.Parse)                          // as ints
    .Repeat()                                   // make it a ring
    .Scan(                                      // this lets me alloc a HashSet inline..
        new { h = new HashSet<int>(), f = 0 },  // pack it and the current freq
        (a, o) => new { a.h, f = a.f + o })     // accumulate the freq as we go, pass along HashSet
    .First(a => !a.h.Add(a.f))                  // first freq that fails to add will be the dup
    .f                                          // just need the dup freq
    .ShouldBe(566);

File.ReadLines(inputPath).Select(int.Parse).Repeat().Scan(new { h = new HashSet<int>(), f = 0 }, (a, o) => new { a.h, f = a.f + o }).First(a => !a.h.Add(a.f)).f
    .ShouldBe(566);
