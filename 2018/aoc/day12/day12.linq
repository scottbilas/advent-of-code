<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

    // sample

    SimSum(sampleState, sampleInstrs, 20).ShouldBe(325);

    // problem

    SimSum(inputState, inputInstrs, 20).ShouldBe(3230);
    SimSum(inputState, inputInstrs, 50000000000).Dump();
    // ^ answer = ((50000000000-200)*88)+17904=4400000000304
}

long SimSum(string textState, string textInstrs, long genCount)
{
    var size = 500;
    
    var pots0 = new bool[size];
    var pots1 = new bool[size];
    var pots = pots0;
    var potBase = 100;
    
    for (int i = 0; i < textState.Length; ++i)
        pots[potBase + i] = textState[i] == '#';
    
    var instrs = textInstrs.Split('\n').Select(l => l.Trim()).Where(l => l.Any()).Select(l =>
    {
        var instr = new bool[6];
        instr[0] = l[0] == '#';
        instr[1] = l[1] == '#';
        instr[2] = l[2] == '#';
        instr[3] = l[3] == '#';
        instr[4] = l[4] == '#';
        instr[5] = l[9] == '#';
        return instr;
    })
    .Where(instr => instr[5])
    .ToList();
    
    var lastSum = 0;
    var lastDeltas = new List<int>();
    for (var gen = 0; ; ++gen)
    {
        var sum = 0;
        for (int i = 0; i < pots.Length; ++i)
        {
            if (pots[i])
                sum += i - potBase;
        }
        //$"{gen}: {lastSum},{sum2} d={sum2-lastSum}".Dump();
        var delta = sum - lastSum;
        lastDeltas.Add(delta);
        if (lastDeltas.TakeLast(100).Count(s => s == sum) == delta)
        {
            return (genCount - gen) * delta;
        }

        if (gen == genCount)
            return sum;

        var nextPots = pots == pots0 ? pots1 : pots0;
        for (var i = 0; i < nextPots.Length; ++i)
            nextPots[i] = false;
    
        var first = Array.IndexOf(pots, true) - 4;
        var last = Array.LastIndexOf(pots, true);
        for (var i = first; i < last; ++i)
        {
            foreach (var instr in instrs)
            {
                if (pots[i] == instr[0] &&
                    pots[i + 1] == instr[1] &&
                    pots[i + 2] == instr[2] &&
                    pots[i + 3] == instr[3] &&
                    pots[i + 4] == instr[4])
                {
                    nextPots[i + 2] = true;
                    break;
                }
            }
        }
    
        pots = nextPots;
    }
}

const string sampleState = "#..#.#..##......###...###";
const string sampleInstrs = @"
    ...## => #
    ..#.. => #
    .#... => #
    .#.#. => #
    .#.## => #
    .##.. => #
    .#### => #
    #.#.# => #
    #.### => #
    ##.#. => #
    ##.## => #
    ###.. => #
    ###.# => #
    ####. => #";

const string inputState = "####..##.##..##..#..###..#....#.######..###########.#...#.##..####.###.#.###.###..#.####..#.#..##..#";
const string inputInstrs = @"
    .#.## => .
    ...## => #
    ..#.. => .
    #.#.. => .
    ...#. => .
    .#... => #
    ..... => .
    #.... => .
    #...# => #
    ###.# => .
    ..### => #
    ###.. => .
    ##.## => .
    ##.#. => #
    ..#.# => #
    .###. => .
    .#.#. => .
    .##.. => #
    .#### => .
    ##... => .
    ##### => .
    ..##. => .
    #.##. => .
    .#..# => #
    ##..# => .
    #.#.# => #
    #.### => .
    ....# => .
    #..#. => #
    #..## => .
    ####. => #
    .##.# => #";

