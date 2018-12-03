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
    .ShouldBe(111935);

int CalcOverlap(IEnumerable<string> textRects)
{
    var fabric = new int[1000, 1000];
    
    // fill
    foreach (var textRect in textRects)
    {
        var ints = Regex.Matches(textRect, @"\d+").Cast<Match>().Select(m => int.Parse(m.Value)).ToList();
        var (l, t, w, h) = (ints[1], ints[2], ints[3], ints[4]);
        
        for (int x = l; x < l + w; ++x)
            for (int y = t; y < t + h; ++y)
                ++fabric[x, y];
    }
    
    // check
    var overlap = 0;
    for (int x = 0; x < 1000; ++x)
        for (int y = 0; y < 1000; ++y)
            if (fabric[x, y] > 1)
                ++overlap;
    
    return overlap;
}

// one-liner
