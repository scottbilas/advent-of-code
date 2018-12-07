<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

void Main()
{
    // sample

    var sample = Parse(@"
        Step C must be finished before step A can begin.
        Step C must be finished before step F can begin.
        Step A must be finished before step B can begin.
        Step A must be finished before step D can begin.
        Step B must be finished before step E can begin.
        Step D must be finished before step E can begin.
        Step F must be finished before step E can begin.
        ").ToList();

    CalcOrder(sample).ShouldBe("CABDFE");
    CalcParallelCost(sample, 2, 1).ShouldBe(15);

    BuildQuickGraph(sample);

    // problem

    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);
    var input = Parse(File.ReadAllText($"{scriptDir}/input.txt")).ToList();

    CalcOrder(input).Dump().ShouldBe("FMOXCDGJRAUIHKNYZTESWLPBQV");
    CalcParallelCost(input, 5, 61).Dump().ShouldBe(1053);
}

IEnumerable<(char l, char r)> Parse(string instrs) => Regex
    .Matches(instrs, @" (\w) ").Cast<Match>()
    .Select(m => m.Groups[1].Value)
    .Batch(2, b => (b.First()[0], b.Last()[0]));

class Node
{
    public char C;
    public bool Used;
    public List<Node> Parents = new List<Node>();
    public List<Node> Children = new List<Node>();
}

Node BuildGraph(IEnumerable<(char l, char r)> instrs)
{
    var graph = new Dictionary<char, Node>();

    foreach (var instr in instrs)
    {
        Node nl, nr;
        if (!graph.TryGetValue(instr.l, out nl))
            graph.Add(instr.l, nl = new Node { C = instr.l });
        if (!graph.TryGetValue(instr.r, out nr))
            graph.Add(instr.r, nr = new Node { C = instr.r });

        nl.Children.Add(nr);
        nr.Parents.Add(nl);
    }

    var root = new Node();
    root.Children.AddRange(graph.Values.Where(v => !v.Parents.Any()));
    foreach (var c in root.Children)
        c.Parents.Add(root);

    return root;
}

IBidirectionalGraph<char, SEdge<char>> BuildQuickGraph(IEnumerable<(char l, char r)> instrs) => instrs
    .Select(i => new SEdge<char>(i.l, i.r))
    .ToBidirectionalGraph<char, SEdge<char>>();

string CalcOrder(IEnumerable<(char l, char r)> instrs)
{
    var output = new List<char>();
    var graph = BuildQuickGraph(instrs);
    var working = new SortedSet<char>(graph.Vertices.Where(graph.IsInEdgesEmpty));
    
    while (working.Any())
    {
        var current = working.First();
        output.Add(current);
        working.Remove(current);

        graph
            .OutEdges(current)
            .Select(edge => edge.Target)
            .Where(target => graph
                .InEdges(target)
                .All(edge => output.Contains(edge.Source)))
            .ForEach(newWork => working.Add(newWork));
    }

    return new string(output.ToArray());
}

class Worker
{
    public Node Node;
    public int TimeRemain;
}

class Work
{
    public Node Node;
    public Worker Worker;
}

int CalcParallelCost(IEnumerable<(char l, char r)> instrs, int workerCount, int baseStepCost)
{
    var time = -1;

    var root = BuildGraph(instrs);
    var workers = Enumerable.Range(0, workerCount).Select(_ => new Worker()).ToArray();
    workers[0].Node = root;
    workers[0].TimeRemain = 1;
    var working = new[] { new Work { Node = root, Worker = workers[0] } }.ToList();

    while (working.Any())
    {
        // complete jobs
        for (int i = 0; i < working.Count(); )
        {
            var work = working[i];
            if (work.Worker != null && --work.Worker.TimeRemain == 0)
            {
                work.Worker.Node = null;
                work.Worker = null;
                work.Node.Used = true;
                working.AddRange(work.Node.Children
                    .Where(c => c.Parents.All(p => p.Used))
                    .Select(c => new Work { Node = c }));
                working.RemoveAt(i);
            }
            else
                ++i;
        }

        working.Sort((x, y) => x.Node.C.CompareTo(y.Node.C));

        // assign jobs
        foreach (var worker in workers.Where(w => w.Node == null))
        {
            var work = working.FirstOrDefault(w => w.Worker == null);
            if (work != null)
            {
                work.Worker = worker;
                worker.Node = work.Node;
                worker.TimeRemain = baseStepCost + work.Node.C - 'A';
            }
            else
                break;
        }

        ++time;
    }

    return time;
}
