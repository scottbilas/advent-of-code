<Query Kind="Program">
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
    // sample

    var sample = BuildTree(@"2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2");
    sample.metaSum.ShouldBe(138);
    sample.value.ShouldBe(66);
    Render(sample.root, "sample");

    // problem

    var input = BuildTree(File.ReadAllText($"{scriptDir}/input.txt"));
    input.metaSum.Dump().ShouldBe(42146);
    input.value.Dump().ShouldBe(26753);
    Render(input.root, "input");
}

class Node
{
    public List<Node> Children = new List<Node>();
    public List<int> Metadata = new List<int>();
}

(int metaSum, int value, Node root) BuildTree(string license)
{
    var ints = Regex
        .Matches(license, @"\d+").Cast<Match>()
        .Select(m => int.Parse(m.Value))
        .ToList();

    var read = 0;
    var metaSum = 0;

    Node ReadNode()
    {
        var node = new Node();

        var childCount = ints[read++];
        var metaCount = ints[read++];

        for (int i = 0; i < childCount; ++i)
            node.Children.Add(ReadNode());

        for (int i = 0; i < metaCount; ++i)
        {
            var meta = ints[read++];
            node.Metadata.Add(meta);
            metaSum += meta;
        }

        return node;
    }

    var root = ReadNode();
    var values = new Dictionary<Node, int>();

    int GetValue(Node node)
    {
        int value;
        if (values.TryGetValue(node, out value))
            return value;
        
        if (node.Children.Any())
        {
            value = node.Metadata
                .Where(i => i <= node.Children.Count)
                .Sum(i => GetValue(node.Children[i - 1]));
        }
        else
            value = node.Metadata.Sum();

        values.Add(node, value);
        return value;
    }
    
    return (metaSum, value: GetValue(root), root);
}

void Render(Node root, string name)
{
    var graph = Walk(root).ToBidirectionalGraph<Node, Edge<Node>>();

    IEnumerable<Edge<Node>> Walk(Node node)
    {
        foreach (var i in node.Children)
        {
            yield return new Edge<Node>(node, i);
            foreach (var j in Walk(i))
                yield return j;
        }
    }

    var dotName = $"{name}.dot";
    var svgName = $"{name}.svg";

    var graphviz = new GraphvizAlgorithm<Node, Edge<Node>>(graph);
    graphviz.FormatVertex += (sender, args) => args.VertexFormatter.Label = string.Join(",", args.Vertex.Metadata);
    graphviz.Generate(new FileDotEngine(), $"{scriptDir}/{dotName}");

    using (var process = Process.Start(new ProcessStartInfo
    {
        FileName = "dot",
        Arguments = $"-Tsvg {dotName} -o {svgName}", // part 2 is insane as a png, use svg instead
        WorkingDirectory = scriptDir,
        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
    }))
    {
        process.WaitForExit();
    }
}
