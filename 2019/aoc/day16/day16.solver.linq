<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>RoyT.AStar</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var input = inputPath.ReadAllText().Trim();
    
// --- PART 1 ---

    // *SAMPLES*

    PatternRle(0).Take(8).ToArray().ShouldBe(Arr((0, 0), (1, 1), (0, 1), (-1, 1), (0, 1), (1, 1), (0, 1), (-1, 1)));
    PatternRle(1).Take(8).ToArray().ShouldBe(Arr((0, 1), (1, 2), (0, 2), (-1, 2), (0, 2), (1, 2), (0, 2), (-1, 2)));
    PatternRle(2).Take(8).ToArray().ShouldBe(Arr((0, 2), (1, 3), (0, 3), (-1, 3), (0, 3), (1, 3), (0, 3), (-1, 3)));
    PatternRle(3).Take(8).ToArray().ShouldBe(Arr((0, 3), (1, 4), (0, 4), (-1, 4), (0, 4), (1, 4), (0, 4), (-1, 4)));

    Solve1("12345678", 1).ShouldBe("48226158");
    Solve1("12345678", 2).ShouldBe("34040438");
    Solve1("12345678", 3).ShouldBe("03415518");
    Solve1("12345678", 4).ShouldBe("01029498");
    Solve1("80871224585914546619083218645595", 100).ShouldBe("24176176");
    Solve1("19617804207202209144916044189917", 100).ShouldBe("73745418");
    Solve1("69317163492948606335995924319873", 100).ShouldBe("52432133");

    // *PROBLEM*

    Solve1(input, 100).Dump().ShouldBe("96136976");

// --- PART 2 ---

    // *SAMPLES*

    Solve2("03036732577212944063491565474664").ShouldBe("84462026");
    Solve2("02935109699940807407585447034323").ShouldBe("78725270");
    Solve2("03081770884921959731165446850517").ShouldBe("53553731");

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe("85600369");
}

IEnumerable<(int value, int count)> PatternRle(int dstPos)
{
    var first = true;
    foreach (var value in Arr(0, 1, 0, -1).Repeat())
    {
        yield return (value, dstPos + (first ? 0 : 1));
        first = false;
    }
}

string Solve1(string text, int phases)
{
    var line = text.ToDigits();
    var nextLine = line.ToArray();
    
    for (var phase = 0; phase < phases; ++phase)
    {
        Swap(ref line, ref nextLine);
        
        for (var y = 0; y < line.Length; ++y)
        {
            var (accum, x) = (0, 0);
            foreach (var next in PatternRle(y))
            {
                if (next.value == 0)
                    x += next.count;
                else
                {
                    var total = 0;
                    for (var end = Math.Min(x + next.count, line.Length); x < end; ++x)
                        total += line[x];
                    accum += next.value * total;
                }

                if (x >= line.Length)
                    break;
            }

            nextLine[y] = Math.Abs(accum) % 10;
        }
    }
   
    return nextLine.Take(8).FromDigits();
}

string Solve2(string text)
{
    var offset = text.Substring(0, 7).ParseInt();

    var segment = text
        .ToDigits().Repeat()                // parse and fill with repeating input
        .Skip(offset % text.Length)         // align to start location in input
        .Take(text.Length * 10000 - offset) // will be summing to the very end
        .ToArray();
    
    for (var phase = 0; phase < 100; ++phase)
    {
        int prev, sum = segment.Sum();
        for (var i = 0; i < segment.Length; ++i)
        {
            prev = segment[i];
            segment[i] = sum % 10;
            sum -= prev;
        }
    }
    
    return segment.Take(8).FromDigits();
}
