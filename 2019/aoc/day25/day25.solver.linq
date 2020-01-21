<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>BetterConsoleTables</NuGetReference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>GraphViz.NET</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>BetterConsoleTables</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");

    var mem = inputPath.ReadAllBigInts().ToArray();

// --- PART 1 ---

    Solve1(mem).Dump().ShouldBe("16778274");

// --- PART 2 ---

    // ?
}

class Room
{
    public string Name;
    public string Desc;
    public string[] Items = new string[0];
    public Dir? SecureExit;
}

class Edge : TaggedUndirectedEdge<string, Dir>
    { public Edge(string va, string vb, Dir dir) : base(va, vb, dir) { } }
class ShipGraph : UndirectedGraph<string, Edge> { }

class ShipVM
{
    BigIntCodeVM _vm;
    Queue<int> _input = new Queue<int>();

    public ShipVM(BigInteger[] mem)
    {
        _vm = new BigIntCodeVM(mem, () =>
        {
            if (_input.IsEmpty())
                _input.EnqueueRange((Console.ReadLine() + '\n').Select(c => (int)c));
            return _input.Dequeue();
        });
    }

    public string GetResult(bool trim = true)
    {
        var text = new StringBuilder();
        foreach (var c in _vm.Run())
        {
            if (c == '\n' && text.ToString().EndsWith("Command?"))
            {
                text.Remove(text.Length - 8, 8);
                break;
            }
            
            if (text.Length > 1000)
                throw new Exception();
            
            text.Append((char)c);
        }

        LastResult = text.ToString();
        if (trim)
            LastResult = LastResult.Trim();

        return LastResult;
    }

    public string LastResult { get; private set; }

    public string Do(string cmd, bool trim = true)
    {
        if (cmd != null)
            _input.EnqueueRange((cmd + '\n').Select(c => (int)c));
        return GetResult(trim);
    }
}

string Solve1(BigInteger[] mem)
{
    var badItems = new List<string>();
    for (;;)
    {
        try { return FindPassword(mem, badItems); }
        catch (LostGameException x) { badItems.Add(x.Item); }
    }
}

class LostGameException : Exception { public string Item; }

string FindPassword(BigInteger[] mem, IEnumerable<string> badItems)
{
    var vm = new ShipVM(mem);
    var graph = new ShipGraph();
    var rooms = new Dictionary<string, Room>();
    var items = new List<string>();

    Room currentRoom = null;
    string attemptedItem = null;
    string checkpoint = null;

    void Render() => RenderRooms(graph, name => rooms[name]);

    Dir[] Move(Dir? dir)
    {
        var text = vm.Do(dir?.GetName(), false);
        if (text.Contains("ejected back to the checkpoint"))
        {
            checkpoint = currentRoom.Name;
            return null;
        }
            
        var match = Regex.Match(text, @"(?x-)
            ==\ (?<name>.*)\ ==\s+
            (?<desc>.*?)\s+
            Doors\ here\ lead:\s+
                (-\ (?<exit>\w+)\s+)+\s+
            (Items\ here:\s+
                (-\ (?<item>[^\r\n]+)\s*)+)?");

        if (!match.Success)
            throw new Exception();

        var name = match.Text("name");
        if (!rooms.TryGetValue(name, out var room))
        {
            room = rooms[name] = new Room
            {
                Name = name,
                Desc = match.Text("desc"),
                Items = match.SelectText("item").ToArray(),
            };
            graph.AddVertex(name);
            if (dir != null)
                graph.AddEdge(new Edge(currentRoom.Name, name, dir.Value));
        }

        currentRoom = room;

        return (match.Select("exit", v => v.ParseDir()).ToArray());
    }

    void Explore(Dir? fromDir, Dir[] exits)
    {
        foreach (var item in currentRoom.Items.Except(badItems))
        {
            attemptedItem = item;
            if (vm.Do("take " + item).SelectLines().Count() > 1)
                throw new Exception();

            items.Add(item);
        }
        
        if (fromDir != null)
            exits = exits.Except(fromDir.Value.GetReverse()).ToArray();

        foreach (var exit in exits)
        {
            var next = Move(exit);
            if (currentRoom.Name == checkpoint)
                currentRoom.SecureExit = exit;
            else
            {
                Explore(exit, next);
                Move(exit.GetReverse());
            }
        }
    }

    try { With(Move(null), m => Explore(null, m)); }
    catch (Exception) { throw new LostGameException { Item = attemptedItem }; }

    graph.ShortestPathsDijkstra(_ => 1, currentRoom.Name)(checkpoint, out var path);
    foreach (var step in path)
        Move(step.Tag);

    var inv = items.ToList();
    var move = currentRoom.SecureExit.Value;
    
    foreach (var set in items.Combinations(1, items.Count))
    {
        foreach (var item in inv.Except(set)) vm.Do("drop " + item);
        foreach (var item in set.Except(inv)) vm.Do("take " + item);
        inv = set.ToList();

        if (Move(move) != null)
            break;
    }

    //Render();

    return Regex.Match(vm.LastResult, @"get in by typing (\w+)").Groups[1].Value;
}

void RenderRooms(ShipGraph graph, Func<string, Room> resolver)
{
    var engine = "dot"; // neato, fdp, sfdp, twopi, circo
    
    var png = Utils.RenderGraphViz(graph.ToGraphviz(g =>
        {
            g.FormatVertex += (_, args) =>
                {
                    args.VertexFormatter.Label = args.Vertex;
                    var room = resolver(args.Vertex);
                    if (room.Items.Any())
                        args.VertexFormatter.Label += "\n(" + room.Items.StringJoin(", ") + ")";
                };
            g.FormatEdge += (_, args) => args.EdgeFormatter.Label.Value = args.Edge.Tag.GetName();
        }), $"-K{engine}");

    ScriptDir.Combine("ship.png").WriteAllBytes(png);

    var table = new Table("Room", "Description", "Item") { Config = TableConfiguration.Markdown() };
    foreach (var room in graph.Vertices.Ordered().Select(resolver))
        table.AddRow(room.Name, room.Desc, room.Items.StringJoin(", "));
        
    ScriptDir.Combine("ship.md").WriteAllText("# Santa's Ship\n\n" + table.ToString().Replace("\r", ""));
}