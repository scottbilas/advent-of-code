<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Open.Numeric.Primes</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>Open.Numeric.Primes</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var input = ParseInput(inputPath.ReadAllText());

// --- PART 1 ---

    // *SAMPLES*

    Sample("new",  0).ShouldBe(ParseResults("9 8 7 6 5 4 3 2 1 0"));
    Sample("cut",  3).ShouldBe(ParseResults("3 4 5 6 7 8 9 0 1 2"));
    Sample("cut", -4).ShouldBe(ParseResults("6 7 8 9 0 1 2 3 4 5"));
    Sample("inc",  3).ShouldBe(ParseResults("0 7 4 1 8 5 2 9 6 3"));

    Sample(@"
        deal with increment 7
        deal into new stack
        deal into new stack")
        .ShouldBe(ParseResults("0 3 6 9 2 5 8 1 4 7"));

    Sample(@"
        cut 6
        deal with increment 7
        deal into new stack")
        .ShouldBe(ParseResults("3 0 7 4 1 8 5 2 9 6"));

    Sample(@"
        deal with increment 7
        deal with increment 9
        cut -2")
        .ShouldBe(ParseResults("6 3 0 7 4 1 8 5 2 9"));

    Sample(@"
        deal into new stack
        cut -2
        deal with increment 7
        cut 8
        cut -4
        deal with increment 7
        cut 3
        deal with increment 9
        deal with increment 3
        cut -1")
        .ShouldBe(ParseResults("9 2 5 8 1 4 7 0 3 6"));

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(3939);

// --- PART 2 ---

    Solve2(input).Dump();//.ShouldBe(3939);
}

// forward sim
IEnumerable<long> FindCardPos(IEnumerable<(string name, int value)> instrs, long deckSize, int cardId)
{
    instrs = instrs.UnDefer();
    long cardPos = cardId;

    for (;;)
    {
        long Calc(int x, int k) => (x*cardPos + k + deckSize) % deckSize;
        
        foreach (var (n, i) in instrs)
        {
            if      (n == "new") cardPos = Calc(-1, -1);
            else if (n == "cut") cardPos = Calc( 1, -i);
            else if (n == "inc") cardPos = Calc( i,  0);
        }
        
        yield return cardPos;
    }
}

// reverse sim
IEnumerable<long> FindCardId(IEnumerable<(string name, int value)> instrs, long deckSize, long cardPos)
{
    instrs = instrs.Reverse().UnDefer();

    for (;;)
    {
        foreach (var instr in instrs)
        {
            checked
            {
                if (instr.name == "new")
                    cardPos = -cardPos - 1 + deckSize;
                else if (instr.name == "cut")
                    cardPos = (cardPos + instr.value + deckSize) % deckSize;
                else if (instr.name == "inc")
                {
                    // got to be a non/less-iterative way to do this..
                    while (cardPos % instr.value != 0)
                        cardPos += deckSize;
                    cardPos /= instr.value;
                }
            }
        }
        
        yield return cardPos;
    }
}

int[] Sample(IEnumerable<(string name, int value)> instrs)
{
    const int k_DeckSize = 10;
    
    var forward = new int[k_DeckSize];
    for (var i = 0; i < k_DeckSize; ++i)
        forward[FindCardPos(instrs, k_DeckSize, i).First()] = i;
    
    var reverse = new int[k_DeckSize];
    for (var i = 0; i < k_DeckSize; ++i)
        reverse[i] = (int)FindCardId(instrs, k_DeckSize, i).First();

    reverse.ShouldBe(forward);

    return forward;
}

int[] Sample(string name, int value) => Sample(Arr((name, value)));
int[] Sample(string text) => Sample(ParseInput(text));

int Solve1(IEnumerable<(string name, int value)> instrs) =>
    (int)FindCardPos(instrs, 10007, 2019).First();

int Solve2(IEnumerable<(string name, int value)> instrs)
{
    // too high: 97900789585841
    // too low:   3522820526285
    
    const long k_DeckSize = 10000; //119315717514047; 
    const int k_TargetPos = 2020;
    
    foreach (var deckSize in Prime.Numbers.StartingAt(10000))
    {
        //$"Period for {deckSize}:".Dump();

        var sims = 0;
        foreach (var pos in FindCardPos(instrs, deckSize, k_TargetPos))
        {
            ++sims;
            if (pos == k_TargetPos)
            {
                ((deckSize - 1) % sims).ShouldBe(0);

                $"{deckSize}: {sims} ({(deckSize - 1) / sims}x) {Prime.Factors(deckSize - 1).StringJoin(' ')}".Dump();
                
                //{(Prime.Numbers.IsPrime(sims) ? " (prime)" : "")}
                break;
            }

            //if (sims % 1000000 == 0)
            //    sims.Dump();
        }
    }
    
    return 0;
}

/*
Solve2 notes

deckSize = 119315717514047, which is prime
    ^ 119315717514046 has factors 2, 59657858757023 
    ^ 119315717514048 has factors 1, 2, 2, 2, 2, 2, 2, 3, 11, 70489, 801461

shuffle = 101741582076661 times (also prime)
    ^ 101741582076660 has factors 1, 2, 2, 3, 3, 3, 5, 73, 2381, 1083983
    ^ 101741582076662 has factors 1, 2, 11, 17, 19, 31, 61, 89, 241, 353

it's a 1.17273x size difference

*/

// support

int[] ParseResults(string text) => text.SelectInts().ToArray();

IEnumerable<(string name, int value)> ParseInput(string text) => text
    .SelectMatches(@"(?<new>deal into new stack)|cut (?<cut>-?\d+)|deal with increment (?<inc>\d+)")
    .Select(m => With(
        m.GetGroups().Skip(1).First(g => g.Success),
        g => (g.Name, g.Value.TryParseInt())));