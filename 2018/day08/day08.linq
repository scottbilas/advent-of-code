<Query Kind="Program">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
    // sample

    var sample = BuildTree(@"2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2");
    sample.metaSum.ShouldBe(138);
    sample.value.ShouldBe(66);

    // problem

    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);
    var input = BuildTree(File.ReadAllText($"{scriptDir}/input.txt"));

    input.metaSum.Dump().ShouldBe(42146);
    input.value.Dump().ShouldBe(26753);
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
