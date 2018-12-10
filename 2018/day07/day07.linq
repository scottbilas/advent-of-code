<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
    // sample

    var sample = BuildGraph(@"
        Step C must be finished before step A can begin.
        Step C must be finished before step F can begin.
        Step A must be finished before step B can begin.
        Step A must be finished before step D can begin.
        Step B must be finished before step E can begin.
        Step D must be finished before step E can begin.
        Step F must be finished before step E can begin.
        ");

    CalcOrder(sample).ShouldBe("CABDFE");
    CalcParallelCost(sample, 2, 1).ShouldBe(15);
    Render(sample, "sample");

    // problem

    var input = BuildGraph(File.ReadAllText($"{scriptDir}/input.txt"));

    CalcOrder(input).Dump().ShouldBe("FMOXCDGJRAUIHKNYZTESWLPBQV");
    CalcParallelCost(input, 5, 61).Dump().ShouldBe(1053);
    Render(input, "input");
}

IBidirectionalGraph<char, SEdge<char>> BuildGraph(string instrs) => Regex
    .Matches(instrs, @" (\w) ").Cast<Match>()
    .Select(m => m.Groups[1].Value[0])
    .Batch(2, b => new SEdge<char>(b.First(), b.Last()))
    .ToBidirectionalGraph<char, SEdge<char>>();

IEnumerable<char> GetRoots(IBidirectionalGraph<char, SEdge<char>> graph) =>
    graph.Vertices.Where(graph.IsInEdgesEmpty);

string CalcOrder(IBidirectionalGraph<char, SEdge<char>> graph)
{
    var output = new List<char>();
    var working = new SortedSet<char>(GetRoots(graph));
    
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
    public char C;
    public int TimeRemain;
}

// TODO: feed part1 into this, should simplify the code
int CalcParallelCost(IBidirectionalGraph<char, SEdge<char>> graph, int workerCount, int baseStepCost)
{
    var time = -1;

    var workers = Enumerable.Range(0, workerCount).Select(_ => new Worker()).ToList();
    var working = new SortedList<char, Worker>(GetRoots(graph).ToDictionary(v => v, v => (Worker)null));
    var used = new List<char>();

    while (working.Any())
    {
        // complete jobs
        for (var i = 0; i < working.Count; )
        {
            var worker = working.Values[i];
            if (worker != null && --worker.TimeRemain == 0)
            {
                var vertex = working.Keys[i];
                working.RemoveAt(i);

                used.Add(vertex);
                worker.C = (char)0;
                graph
                    .OutEdges(vertex)
                    .Select(edge => edge.Target)
                    .Where(target => graph
                        .InEdges(target)
                        .All(edge => used.Contains(edge.Source)))
                    .ForEach(newWork => working.Add(newWork, (Worker)null));
            }
            else
                ++i;
        }

        // assign jobs
        foreach (var worker in workers.Where(w => w.C == 0))
        {
            var work = working.FirstOrDefault(w => w.Value == null);
            if (work.Key == 0)
                break;

            working[work.Key] = worker;
            worker.C = work.Key;
            worker.TimeRemain = baseStepCost + work.Key - 'A';
        }

        ++time;
    }

    return time;
}

void Render(IBidirectionalGraph<char, SEdge<char>> graph, string name)
{
    var dotName = $"{name}.dot";
    var pngName = $"{name}.png";

    var graphviz = new GraphvizAlgorithm<char, SEdge<char>>(graph);
    graphviz.FormatVertex += (sender, args) => args.VertexFormatter.Comment = args.Vertex.ToString();
    graphviz.Generate(new FileDotEngine(), $"{scriptDir}/{dotName}");

    using (var process = Process.Start(new ProcessStartInfo
    {
        FileName = "dot",
        Arguments = $"-Tpng {dotName} -o {pngName}",
        WorkingDirectory = scriptDir,
        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
    }))
    {
        process.WaitForExit();
    }
}
