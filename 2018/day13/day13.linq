<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
    // sample

    Sim("sample1").ShouldBe((0, 3));
    Sim("sample2").ShouldBe((7, 3));
    Sim("sample3", false).ShouldBe((6, 4));

    // problem

    var firstCollide = Sim("input");
    firstCollide.ToString().Dump();
    firstCollide.ShouldBe((83, 121));

    var lastSurviving = Sim("input", false);
    lastSurviving.ToString().Dump();
    lastSurviving.ShouldBe((102, 144));
}

class Cart : IComparable<Cart>
{
    public int X, Y;
    public char C;
    public int AI;

    public Cart(int x, int y, char dir) { X = x; Y = y; C = dir; }

    public int CompareTo(Cart other)
    {
        var compare = Y.CompareTo(other.Y);
        return compare == 0 ? X.CompareTo(other.X) : compare;
    }
}

(int x, int y) Sim(string name, bool findFirstCollision = true)
{
    var lines = File.ReadAllLines($"{scriptDir}/{name}.txt");
    var maxLine = lines.Max(l => l.Length);
    
    var state = new char[maxLine, lines.Length];
    var carts = new List<Cart>();
    
    var outPath = $"{scriptDir}/{name}.out";
    if (File.Exists(outPath))
        File.Delete(outPath);
    
    void Render(int frame)
    {
        if (lines.Length > 20)
            return;
        
        var dup = (char[,])state.Clone();
        foreach (var c in carts)
        {
            if ("<^>v".Contains(dup[c.X, c.Y]))
                dup[c.X, c.Y] = 'X';
            else
                dup[c.X, c.Y] = c.C;
        }

        using (var f = File.AppendText(outPath))
        {
            f.WriteLine($"***[{frame}]***");
            f.WriteLine();
            
            for (var y = 0; y < lines.Length; ++y)
            {
                var sb = new StringBuilder();
                for (var x = 0; x < maxLine; ++x)
                    sb.Append(dup[x, y] == 0 ? ' ' : dup[x, y]);
                f.WriteLine(sb.ToString());
            }
            f.WriteLine();

            f.WriteLine(string.Join("; ", carts.Select(c => $"{c.X},{c.Y}")));

            f.WriteLine();
            f.WriteLine();
        }
    }

    var cartToState = (from:"^v<>", to:"|-");

    for (var y = 0; y < lines.Length; ++y)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; ++x)
        {
            var found = cartToState.from.IndexOf(line[x]);
            if (found >= 0)
            {
                state[x, y] = cartToState.to[found / 2];
                carts.Add(new Cart(x, y, line[x]));
            }
            else
                state[x, y] = line[x];
        }
    }

    var indexers = "^>v<";
    var moveSpec = (dx: new[] { 0, 1, 0, -1 }, dy: new[] { -1, 0, 1, 0 });
    var turnSpec = (spec: new[] { '/', '\\', 0, 2 }, next: new[] { ">^<v", "<v>^", "<^>v", ">v<^" });

    for (var frame = 0;;++frame)
    {
        carts.Sort();
        Render(frame);
        
        for (var i = 0; i < carts.Count; ++i)
            if (carts[i].C == 'X')
                carts.RemoveAt(i--);

        if (carts.Count == 1 && !findFirstCollision)
            return (carts[0].X, carts[0].Y);

        for (var i = 0; i < carts.Count; ++i)
        {
            var cart = carts[i];
            if (cart.C == 'X')
                continue;

            var idir = indexers.IndexOf(cart.C);

            // move
            cart.X += moveSpec.dx[idir];
            cart.Y += moveSpec.dy[idir];

            // turn
            var cell = state[cart.X, cart.Y];
            if (cell == '+')
            {
                cell = (char)cart.AI;
                cart.AI = (cart.AI + 1) % 3;
            }
            
            var spec = Array.IndexOf(turnSpec.spec, cell);
            if (spec >= 0)
                cart.C = turnSpec.next[spec][idir];
            
            foreach (var dup in carts.Where(c => c != cart && c.X == cart.X && c.Y == cart.Y))
            {
                cart.C = dup.C = 'X';
                if (findFirstCollision)
                    return (cart.X, cart.Y);
            }
        }
    }
}
