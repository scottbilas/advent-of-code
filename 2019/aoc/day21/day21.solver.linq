<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var mem = inputPath.ReadAllInts().ToArray();

// --- PART 1 ---

    Solve1(mem).Dump().ShouldBe(19350258);

// --- PART 2 ---

    Solve2(mem).Dump().ShouldBe(1142627861);
}

int Run(int[] mem, string program)
{
    var ptr = 0;
    var output = new IntCodeVM(mem, () => program[ptr++]).Run().ToArray();

    var sb = new StringBuilder();
    foreach (var c in output)
    {
        if (c >= 128)
            return c;

        sb.Append((char)c);
    }

    sb.ToString().Dump();
    return 0;
}

int Solve1(int[] mem) => Run(mem, Parse(@"
    NOT A J
    NOT C T
    AND D T
    OR T J
    WALK
    "));

int Solve2(int[] mem)
{
    var instrs = Parse(@"
        ## patterns
    
        # jump through island
        NOT C J
        NOT E T
        AND H T
        AND T J

        # land on closest edge of gap unless followed by hole
        NOT C T
        AND E T
        OR T J

        # detect ##.##. (jump on pos=0)
        OR B T
        OR E T
        NOT T T
        OR T J

        ## safeties
        
        # certain death if don't jump
        NOT A T
        OR T J
        
        # certain death if jump
        AND D J
        
        RUN
        ");

    // check program by simulating all possibilities
    foreach (var line in GetAllCombos())
        Sim(instrs, line);
    if (fails.Any())
        $"FAILS: {fails.Count} ({fails.StringJoin(", ")})".Dump();

    // run the actual program
    return Run(mem, instrs);
}

string Parse(string instrText) =>
    instrText.ReplaceMatches(@"#.*", "").SelectLines().StringJoin('\n') + '\n';

// simulation and solver

string GetSensors(string line, int jumpPos)
{
    var sensorPos = jumpPos + 1;
    var sensors = "".PadLeft(sensorPos);
    
    for (var sensor = 'A'; sensor <= 'I' && sensorPos < line.Length; ++sensor, ++sensorPos)
        sensors += line[sensorPos] != '.' ? sensor : '-';

    return sensors;
}

IEnumerable<string> GetAllCombos()
{
    for (var i = 1; i < (1 << 9); ++i)
        yield return "#####" + Range(0, 9).Select(b => (i & (1 << b)) != 0 ? '#' : '.').ToStringFromChars() + "###";
}

List<string> FindMoves(string line)
{
    var moves = new Stack<string>();
    var found = new List<string>();
    var start = Math.Max(0, line.IndexOf('.') - 3);
    var end = line.LastIndexOf('.') + 1;

    void Move(string move, int pos)
    {
        moves.Push(move);
        pos += move.Length;

        if (pos >= end)
        {
            var f = "";
            var insertAt = found.Count;
            foreach (var m in moves.Reverse())
            {
                if (m[0] == 'J')
                    found.Add(GetSensors(line, f.Length));
                f += m;
            }
            found.Insert(insertAt, f);
        }
        else if (line[pos] != '.')
        {
            Move(">", pos);
            Move("J---", pos);
        }
        
        moves.Pop();
    }

    if (start > 0)
        moves.Push(new string(' ', start));

    Move(">", start);
    Move("J---", start);
    
    return found;
}

List<int> fails = new List<int>();
void Render(int sim, string line, IEnumerable<string> moves)
{
    var foundMoves = FindMoves(line);
    if (!foundMoves.Any())
        return;

    if (moves != null && moves.Last() == "X")
        fails.Add(sim);
    
    $"line {sim.ToString() + ':',-4} {line}".Dump();

    if (moves != null)
    {
        var output = new List<string>();
        var moved = "";
        foreach (var move in moves)
        {
            if (move[0] == 'J')
                output.Add(GetSensors(line, moved.Length));
            moved += move;
        }

        $"moves:    {moved}".Dump();
        foreach (var o in output)
            $"          {o}".Dump();
    }

    foreach (var move in foundMoves.AsSmartEnumerable())
        $"{(move.IsFirst ? "found:" : "      ")}    {move.Value}".Dump();
        
    "".Dump();
}

int sim = 0;
bool Sim(string instrText, string line, bool alwaysRender = false)
{
    ++sim;

    var instrs = instrText.SelectWords().SkipLast(1).ToArray(); // remove run/walk
    var regs = new AutoDictionary<char, bool>();

    var moves = new List<string>();
    for (var pos = 0; pos < line.Length;)
    {
        foreach (var (instr, src, dst) in instrs.Batch(3, i => (i[0], i[1][0], i[2][0])))
        {
            for (var sensor = 'A'; sensor <= 'I'; ++sensor)
                regs[sensor] = line.TryGetAt(pos + 1 + sensor - 'A') != '.';

            if (instr == "NOT")
                regs[dst] = !regs[src];
            else if (instr == "AND")
                regs[dst] = regs[src] && regs[dst];
            else if (instr == "OR")
                regs[dst] = regs[src] || regs[dst];
        }

        var move = regs['J'] ? "J---" : ">";
        moves.Add(move);
        pos += move.Length;

        if (line.TryGetAt(pos) == '.')
        {
            moves.Add("X");
            break;
        }
    }

    if (alwaysRender || moves.Last() == "X")
        Render(sim, line, moves);
    
    return true;
}