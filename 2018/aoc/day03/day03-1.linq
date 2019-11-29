<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

CalcOverlap(new[] {
    "#1 @ 1,3: 4x4",
    "#2 @ 3,1: 4x4",
    "#3 @ 5,5: 2x2" })
    .ShouldBe(4);

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

CalcOverlap(
    File.ReadLines(inputPath))
    .Dump()
    .ShouldBe(111935);

int CalcOverlap(IEnumerable<string> textRects)
{
    var fabric = new int[1000, 1000];

    var overlaps =
        from rect in
            from textRect in textRects
            let ints = (
                from Match m in Regex.Matches(textRect, @"\d+")
                select int.Parse(m.Value)).ToList()
            select (l: ints[1], t: ints[2], w: ints[3], h: ints[4])
        from x in Enumerable.Range(rect.l, rect.w)
        from y in Enumerable.Range(rect.t, rect.h)
        select fabric[x, y]++ == 1 ? 1 : 0;
        
    return overlaps.Sum();
}
