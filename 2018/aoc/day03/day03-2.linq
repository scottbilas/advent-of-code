<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

FindNonOverlapping(new[] {
    "#1 @ 1,3: 4x4",
    "#2 @ 3,1: 4x4",
    "#3 @ 5,5: 2x2" })
    .ShouldBe(new[] { 3 });

// problem

var inputPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "input.txt");

FindNonOverlapping(
    File.ReadLines(inputPath))
    .Single()
    .Dump()
    .ShouldBe(650);

IEnumerable<int> FindNonOverlapping(IEnumerable<string> textRects)
{
    var rects = textRects
        .Select(textRect => Regex
            .Matches(textRect, @"\d+").Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .ToList())
        .Select(ints => (id: ints[0], l: ints[1], t: ints[2], w: ints[3], h: ints[4]));

    var fabric = new int[1000, 1000];
    
    // fill
    foreach (var rect in rects)
        for (int x = rect.l; x < rect.l + rect.w; ++x)
            for (int y = rect.t; y < rect.t + rect.h; ++y)
                fabric[x, y] = fabric[x, y] == 0 ? rect.id : -1;

    // check
    foreach (var rect in rects)
    {
        var ok = true;
        for (int x = rect.l; x < rect.l + rect.w; ++x)
            for (int y = rect.t; y < rect.t + rect.h; ++y)
                if (fabric[x, y] != rect.id)
                    ok = false;
            
        if (ok)
            yield return rect.id;
    }
}
