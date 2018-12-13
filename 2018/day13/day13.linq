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

    Sim("sample1").firstCollision.ShouldBe((0, 3));
    Sim("sample2").firstCollision.ShouldBe((7, 3));
    Sim("sample3").lastCart.ShouldBe((6, 4));

    // problem

    var result = Sim("input");
    Console.WriteLine($"{result.firstCollision.x},{result.firstCollision.y}");
    result.firstCollision.ShouldBe((83, 121));
    Console.WriteLine($"{result.lastCart.x},{result.lastCart.y}");
    result.lastCart.ShouldBe((102, 144));
}

class Cart
{
    public int X, Y;
    public char C;
    public int AI;

    public Cart(int x, int y, char dir) { X = x; Y = y; C = dir; }
}

((int x, int y) firstCollision, (int x, int y) lastCart)
Sim(string name)
{
    var lines = File.ReadAllLines($"{scriptDir}/{name}.txt");
    var maxLine = lines.Max(l => l.Length);
    
    var state = new char[maxLine, lines.Length];
    var carts = new List<Cart>();
    
    const string indexers = "^>v<";

    void Render(int frame)
    {
        var outPath = $"{scriptDir}/{name}.out";
        if (frame == 0 && File.Exists(outPath))
            File.Delete(outPath);

        // TODO: GIF!
        if (lines.Length > 20)
            return;
        
        using (var f = File.AppendText(outPath))
        {
            f.WriteLine($"***[{frame}]***");
            f.WriteLine();

            var dup = (char[,])state.Clone();
            foreach (var c in carts)
                dup[c.X, c.Y] = c.C;

            for (var y = 0; y < lines.Length; ++y)
            {
                for (var x = 0; x < maxLine; ++x)
                    f.Write(dup[x, y] == 0 ? ' ' : dup[x, y]);
                f.WriteLine();
            }
            f.WriteLine();

            f.WriteLine(string.Join("; ", carts.Select(c => $"{c.X},{c.Y}")));

            f.WriteLine();
            f.WriteLine();
        }
    }

    for (var y = 0; y < lines.Length; ++y)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; ++x)
        {
            var found = indexers.IndexOf(line[x]);
            if (found >= 0)
            {
                state[x, y] = "|-|-"[found];
                carts.Add(new Cart(x, y, line[x]));
            }
            else
                state[x, y] = line[x];
        }
    }

    var moveSpec = (dx: new[] { 0, 1, 0, -1 }, dy: new[] { -1, 0, 1, 0 });
    var turnSpec = (spec: new[] { '/', '\\', 0, 2 }, next: new[] { ">^<v", "<v>^", "<^>v", ">v<^" });

    var collisions = new List<(int x, int y)>();

    for (var frame = 0; ; ++frame)
    {
        carts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();

        Render(frame);
        
        for (var i = 0; i < carts.Count; ++i)
            if (carts[i].C == 'X')
                carts.RemoveAt(i--);
                
        if (carts.Count < 2)
            break;

        foreach (var cart in carts.Where(c => c.C != 'X'))
        {
            var idir = indexers.IndexOf(cart.C);

            cart.X += moveSpec.dx[idir];
            cart.Y += moveSpec.dy[idir];

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
                collisions.Add((cart.X, cart.Y));
            }
        }
    }
    
    return (
        collisions.Any() ? collisions[0] : (-1, -1),
        carts.Any() ? (carts[0].X, carts[0].Y) : (-1, -1));
}
