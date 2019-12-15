<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
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
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(Parse(@"
        10 ORE => 10 A
        1 ORE => 1 B
        7 A, 1 B => 1 C
        7 A, 1 C => 1 D
        7 A, 1 D => 1 E
        7 A, 1 E => 1 FUEL
        ")).ShouldBe(31);

    Solve1(Parse(@"
        9 ORE => 2 A
        8 ORE => 3 B
        7 ORE => 5 C
        3 A, 4 B => 1 AB
        5 B, 7 C => 1 BC
        4 C, 1 A => 1 CA
        2 AB, 3 BC, 4 CA => 1 FUEL
        ")).ShouldBe(165);

    var sample0 = 
    Parse(@"
        157 ORE => 5 NZVS
        165 ORE => 6 DCFZ
        44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
        12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
        179 ORE => 7 PSHF
        177 ORE => 5 HKGWZ
        7 DCFZ, 7 PSHF => 2 XJWVT
        165 ORE => 2 GPVTF
        3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT
        ");
    Solve1(sample0).ShouldBe(13312);

    var sample1 = Parse(@"
        2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG
        17 NVRVD, 3 JNWZP => 8 VPVL
        53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL
        22 VJHF, 37 MNCFX => 5 FWMGM
        139 ORE => 4 NVRVD
        144 ORE => 7 JNWZP
        5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC
        5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV
        145 ORE => 6 MNCFX
        1 NVRVD => 8 CXFTF
        1 VJHF, 6 MNCFX => 4 RFSQX
        176 ORE => 6 VJHF
        ");
    Solve1(sample1).ShouldBe(180697);

    var sample2 = Parse(@"
        171 ORE => 8 CNZTR
        7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
        114 ORE => 4 BHXH
        14 VRPVC => 6 BMBT
        6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL
        6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
        15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
        13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
        5 BMBT => 4 WPTQ
        189 ORE => 9 KTJDG
        1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
        12 VRPVC, 27 CNZTR => 2 XDBXC
        15 KTJDG, 12 BHXH => 5 XCVML
        3 BHXH, 2 VRPVC => 7 MZWV
        121 ORE => 7 VRPVC
        7 XCVML => 6 RJRHP
        5 BHXH, 4 VRPVC => 5 LTCX    
        ");
    Solve1(sample2).ShouldBe(2210736);

    // *PROBLEM*

    Solve1(Parse(input)).Dump();

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample0).ShouldBe(82892753);
    Solve2(sample1).ShouldBe(5586022);
    Solve2(sample2).ShouldBe(460664);

    // *PROBLEM*

    Solve2(Parse(input)).Dump();
}

class Pair
{
    public int Count;
    public string Chemical;
    
    public Pair(int count, string chemical) => (Count, Chemical) = (count, chemical);
}

class Rule
{
    public Rule(List<Pair> pairs)
    {
        Output = pairs.Last().Chemical;
        Quantity = pairs.Last().Count;

        pairs.RemoveAt(pairs.Count - 1);
        Inputs = pairs;
    }

    public string Output;
    public int Quantity;
    public IEnumerable<Pair> Inputs;
}

long Solve1(IReadOnlyDictionary<string, Rule> graph, long fuelToProduce = 1)
{
    var stock = new Dictionary<string, long>().ToAutoDictionary();
    var ore = 0L;
    
    void Produce(Rule rule, long need)
    {
        var have = stock[rule.Output];
        var used = Min(need, have);
        stock[rule.Output] -= used;
        need -= used;

        if (need == 0)
            return;

        var runs = (long)Ceiling((double)need / rule.Quantity);
        foreach (var input in rule.Inputs)
        {
            if (input.Chemical == "ORE")
                ore += input.Count * runs;
            else
                Produce(graph[input.Chemical], input.Count * runs);
        }

        stock[rule.Output] += rule.Quantity * runs - need;
    }

    Produce(graph["FUEL"], fuelToProduce);
    
    return ore;
}

long Solve2(IReadOnlyDictionary<string, Rule> graph) =>
    BinarySearch(0, 100000000, fuel => 1000000000000L - Solve1(graph, fuel), _ => _).index;

IReadOnlyDictionary<string, Rule> Parse(string text)
{
    var graph = new Dictionary<string, Rule>();

    var pairs = new List<Pair>();
    foreach (var item in text
        .SelectMatches(@"(?<term>=> )?(?<val>[A-Z0-9]+)")
        .Batch(2, l => (
            term: l[0].Success("term"),
            pair: new Pair(l[0].Int("val"), l[1].Text("val")))))
    {
        pairs.Add(item.pair);
        if (item.term)
        {
            With(new Rule(pairs), r => graph.Add(r.Output, r));
            pairs = new List<Pair>();
        }
    }

    return graph;
}
