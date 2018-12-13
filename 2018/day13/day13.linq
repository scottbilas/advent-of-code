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

    Sim("sample1").ShouldBe(new Pos(0, 3));
    Sim("sample2").ShouldBe(new Pos(7, 3));
    Sim("sample3", false).ShouldBe(new Pos(6, 4));

    // problem

    var firstCollide = Sim("input");
    firstCollide.ToString().Dump();
    firstCollide.ShouldBe(new Pos(83, 121));

    var lastSurviving = Sim("input", false);
    lastSurviving.ToString().Dump();
    lastSurviving.ShouldBe(new Pos(102, 144));
}

struct Pos
{
    public int X, Y;
    public Pos(int x, int y) { X = x; Y = y; }
    public override string ToString() => $"{X},{Y}";
}

enum Dir { L, U, R, D };

class Cart : IComparable<Cart>
{
    public Pos Pos;
    public int Step;
    public Dir Dir = Dir.D;
    public Cart(int x, int y) { Pos.X = x; Pos.Y = y; }

    public int CompareTo(Cart obj)
    {
        var compare = Pos.Y.CompareTo(obj.Pos.Y);
        return compare == 0 ? Pos.X.CompareTo(obj.Pos.X) : compare;
    }
}

Pos Sim(string name, bool findFirstCollision = true)
{
    var lines = File.ReadAllLines($"{scriptDir}/{name}.txt");
    var maxLine = lines.Max(l => l.Length);
    
    var state = new char[maxLine, lines.Length];
    var carts = new List<Cart>();
    
    if (File.Exists($"{scriptDir}/{name}.out"))
        File.Delete($"{scriptDir}/{name}.out");
    
    void Render(int frame)
    {
        if (lines.Length > 20)
            return;
        
        var dup = (char[,])state.Clone();
        foreach (var c in carts)
        {
            if ("<^>v".Contains(dup[c.Pos.X, c.Pos.Y]))
                dup[c.Pos.X, c.Pos.Y] = 'X';
            else
            {
                switch (c.Dir)
                {
                    case Dir.L:
                        dup[c.Pos.X, c.Pos.Y] = '<';
                        break;
                    case Dir.U:
                        dup[c.Pos.X, c.Pos.Y] = '^';
                        break;
                    case Dir.R:
                        dup[c.Pos.X, c.Pos.Y] = '>';
                        break;
                    case Dir.D:
                        dup[c.Pos.X, c.Pos.Y] = 'v';
                        break;
                }
            }
        }

        using (var f = File.AppendText($"{scriptDir}/{name}.out"))
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

            f.WriteLine(string.Join("; ", carts.Select(c => c.Pos.ToString())));

            f.WriteLine();
            f.WriteLine();
        }
    }

    for (var y = 0; y < lines.Length; ++y)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; ++x)
        {
            switch (line[x])
            {
                case 'v':
                    state[x, y] = '|';
                    carts.Add(new Cart(x, y) { Dir = Dir.D });
                    break;
                case '^':
                    state[x, y] = '|';
                    carts.Add(new Cart(x, y) { Dir = Dir.U });
                    break;
                case '<':
                    state[x, y] = '-';
                    carts.Add(new Cart(x, y) { Dir = Dir.L });
                    break;
                case '>':
                    state[x, y] = '-';
                    carts.Add(new Cart(x, y) { Dir = Dir.R });
                    break;
                default:
                    state[x, y] = line[x];
                    break;
            }
        }
    }
    
    carts.Sort();

    Render(0);

    for (var frame = 0;;++frame)
    {
        carts.Sort();

        for (var i = 0; i < carts.Count;)
        {
            var cart = carts[i];
            switch (state[cart.Pos.X, cart.Pos.Y])
            {
                case '-':
                    if (cart.Dir == Dir.L)
                    {
                        --cart.Pos.X;
                    }
                    else
                    {
                        cart.Dir.ShouldBe(Dir.R);
                        ++cart.Pos.X;
                    }
                    break;
                case '|':
                    if (cart.Dir == Dir.U)
                    {
                        --cart.Pos.Y;
                    }
                    else
                    {
                        cart.Dir.ShouldBe(Dir.D);
                        ++cart.Pos.Y;
                    }
                    break;
                case '/':
                    switch (cart.Dir)
                    {
                        case Dir.L:
                            ++cart.Pos.Y;
                            cart.Dir = Dir.D;
                            break;
                        case Dir.U:
                            ++cart.Pos.X;
                            cart.Dir = Dir.R;
                            break;
                        case Dir.R:
                            --cart.Pos.Y;
                            cart.Dir = Dir.U;
                            break;
                        case Dir.D:
                            --cart.Pos.X;
                            cart.Dir = Dir.L;
                            break;
                    }
                    break;
                case '\\':
                    switch (cart.Dir)
                    {
                        case Dir.L:
                            --cart.Pos.Y;
                            cart.Dir = Dir.U;
                            break;
                        case Dir.U:
                            --cart.Pos.X;
                            cart.Dir = Dir.L;
                            break;
                        case Dir.R:
                            ++cart.Pos.Y;
                            cart.Dir = Dir.D;
                            break;
                        case Dir.D:
                            ++cart.Pos.X;
                            cart.Dir = Dir.R;
                            break;
                    }
                    break;
                case '+':
                    switch (cart.Dir)
                    {
                        case Dir.L:
                            --cart.Pos.X;
                            break;
                        case Dir.U:
                            --cart.Pos.Y;
                            break;
                        case Dir.R:
                            ++cart.Pos.X;
                            break;
                        case Dir.D:
                            ++cart.Pos.Y;
                            break;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (state[cart.Pos.X, cart.Pos.Y] == '+')
            {
                switch (cart.Step)
                {
                    case 0: // turn left
                        switch (cart.Dir)
                        {
                            case Dir.L:
                                cart.Dir = Dir.D;
                                break;
                            case Dir.U:
                                cart.Dir = Dir.L;
                                break;
                            case Dir.R:
                                cart.Dir = Dir.U;
                                break;
                            case Dir.D:
                                cart.Dir = Dir.R;
                                break;
                        }
                        cart.Step = 1;
                        break;
                    case 1: // go straight
                        cart.Step = 2;
                        break;
                    case 2: // turn right
                        switch (cart.Dir)
                        {
                            case Dir.L:
                                cart.Dir = Dir.U;
                                break;
                            case Dir.U:
                                cart.Dir = Dir.R;
                                break;
                            case Dir.R:
                                cart.Dir = Dir.D;
                                break;
                            case Dir.D:
                                cart.Dir = Dir.L;
                                break;
                        }
                        cart.Step = 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            
            var collide = carts.SingleOrDefault(c => c != cart && c.Pos.X == cart.Pos.X && c.Pos.Y == cart.Pos.Y);
            if (collide != null)
            {
                if (findFirstCollision)
                {
                    Render(frame + 1);
                    return cart.Pos;
                }

                var where = carts.IndexOf(collide);
                
                if (where < i)
                {
                    carts.RemoveAt(i);
                    carts.RemoveAt(where);
                    --i;
                }
                else
                {
                    carts.RemoveAt(where);
                    carts.RemoveAt(i);
                }
                
                continue;
            }
            else
                ++i;
        }

        Render(frame + 1);
        if (carts.Count == 1 && !findFirstCollision)
        {
            return carts[0].Pos;
        }
    }
}
