<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// samples

CalcFreq(new[] { "+1", "-2", "+3", "+1" }).ShouldBe(3);
CalcFreq(new[] { "+1", "+1", "-2" }).ShouldBe(0);
CalcFreq(new[] { "-1", "-2", "-3" }).ShouldBe(-6);

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

CalcFreq(File.ReadLines(inputPath)).Dump().ShouldBe(454);

int CalcFreq(IEnumerable<string> changes)
    => changes.Select(int.Parse).Sum();

// one-liner

File.ReadLines(inputPath).Select(int.Parse).Sum()
    .ShouldBe(454);
