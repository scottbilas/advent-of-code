<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>RoyT.AStar</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
    // sample
    
    Si2("#######", // #######   
        "#.G...#", // #G....#   G(200)
        "#...EG#", // #.G...#   G(131)
        "#.#.#G#", // #.#.#G#   G(59)
        "#..G#E#", // #...#.#   
        "#.....#", // #....G#   G(200)
        "#######") // #######
        // Combat ends after 47 full rounds
        // Goblins win with 590 total hit points left
        // Outcome: 47 * 590 = 27730
        .ShouldBe((27730, 4988));
    
    Sim("#######", //      #######
        "#G..#E#", //      #...#E#   E(200)
        "#E#E.E#", //      #E#...#   E(197)
        "#G.##.#", // -->  #.E##.#   E(185)
        "#...#E#", //      #E..#E#   E(200), E(200)
        "#...E.#", //      #.....#
        "#######") //      #######
                   // Combat ends after 37 full rounds
                   // Elves win with 982 total hit points left
                   // Outcome: 37 * 982 = 36334
        .ShouldBe(36334);
    
    Si2("#######", //      #######   
        "#E..EG#", //      #.E.E.#   E(164), E(197)
        "#.#G.E#", //      #.#E..#   E(200)
        "#E.##E#", // -->  #E.##.#   E(98)
        "#G..#.#", //      #.E.#.#   E(200)
        "#..E#.#", //      #...#.#   
        "#######") //      #######   
        // Combat ends after 46 full rounds
        // Elves win with 859 total hit points left
        // Outcome: 46 * 859 = 39514
        .ShouldBe((39514, 31284));
    
    Si2("#######", //      #######   
        "#E.G#.#", //      #G.G#.#   G(200), G(98)
        "#.#G..#", //      #.#G..#   G(200)
        "#G.#.G#", // -->  #..#..#   
        "#G..#.#", //      #...#G#   G(95)
        "#...E.#", //      #...G.#   G(200)
        "#######") //      #######
        // Combat ends after 35 full rounds
        // Goblins win with 793 total hit points left
        // Outcome: 35 * 793 = 27755
        .ShouldBe((27755, 3478));
    
    Si2("#######", //      #######   
        "#.E...#", //      #.....#   
        "#.#..G#", //      #.#G..#   G(200)
        "#.###.#", // -->  #.###.#   
        "#E#G#G#", //      #.#.#.#   
        "#...#G#", //      #G.G#G#   G(98), G(38), G(200)
        "#######") //      #######   
        // Combat ends after 54 full rounds
        // Goblins win with 536 total hit points left
        // Outcome: 54 * 536 = 28944
        .ShouldBe((28944, 6474));
    
    Si2("#########", //      #########   
        "#G......#", //      #.G.....#   G(137)
        "#.E.#...#", //      #G.G#...#   G(200), G(200)
        "#..##..G#", //      #.G##...#   G(200)
        "#...##..#", // -->  #...##..#   
        "#...#...#", //      #.G.#...#   G(200)
        "#.G...G.#", //      #.......#   
        "#.....G.#", //      #.......#   
        "#########") //      #########   
        // Combat ends after 20 full rounds
        // Goblins win with 937 total hit points left
        // Outcome: 20 * 937 = 18740
        .ShouldBe((18740, 1140));

    // problem

    var result = Si2(File.ReadAllLines($"{scriptDir}/input.txt"));
    result.outcome3.Dump();
    result.elvesOutcome.Dump();
    result.ShouldBe((188576, 57112));
}

class Unit
{
    public char Type;
    public int X, Y;
    public int HP = 200;
    public int AttackPower = 3;
}

int Sim(params string[] lines)
{
    return SimFull(3, lines).outcome;
}

(int outcome3, int elvesOutcome) Si2(params string[] lines)
{
    var outcome3 = Sim(lines);
    for (var attackPower = 4; ; ++attackPower)
    {
        var result = SimFull(attackPower, lines);
        if (result.edeaths == 0)
            return (outcome3, result.outcome);
    }
}

(int edeaths, int outcome) SimFull(int attackPower, string[] lines)
{
    var (cx, cy) = (lines[0].Length, lines.Length);
    var board = new object[cx, cy];
    var astar = new Grid(cx, cy);

    var ecount = 0;
    var units = new List<Unit>();
    
    for (var y = 0; y < cy; ++y)
    {
        for (var x = 0; x < cx; ++x)
        {
            var c = lines[y][x];
            if (c == 'E' || c == 'G')
            {
                var unit = new Unit { Type = c, X = x, Y = y };
                if (c == 'E')
                {
                    unit.AttackPower = attackPower;
                    ++ecount;
                }
                units.Add(unit);
                board[x, y] = unit;
                astar.BlockCell(new Position(x, y));
            }
            else if (c == '#')
            {
                board[x, y] = c;
                astar.BlockCell(new Position(x, y));
            }
        }
    }

    for (var round = 0; ; ++round)
    {
        //Render(round);

        /*if (round == 1)
        {
            "HERE".Dump();
        }*/
        
        units = units.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();

        for (var iunit = 0; iunit < units.Count; ++iunit)
        {
            var unit = units[iunit];

            bool Attack()
            {
                var target =
                    new[]
                    {
                        board[unit.X, unit.Y - 1],
                        board[unit.X - 1, unit.Y],
                        board[unit.X + 1, unit.Y],
                        board[unit.X, unit.Y + 1],
                    }
                    .OfType<Unit>()
                    .Where(t => t.Type != unit.Type)
                    .MinBy(t => t.HP)
                    .FirstOrDefault();
                
                if (target == null)
                    return false;
                
                target.HP -= unit.AttackPower;
                
                if (target.HP <= 0)
                {
                    board[target.X, target.Y] = null;
                    astar.UnblockCell(new Position(target.X, target.Y));
                    
                    var found = units.IndexOf(target);
                    units.RemoveAt(found);
                    if (found < iunit)
                        --iunit;
                }
                
                return true;
            }

            if (!Attack())
            {
                var enemyUnits = units
                    .Where(t => t.Type != unit.Type)
                    .ToList();

                if (!enemyUnits.Any())
                {
                    //Render(round);
                    return (ecount - units.Count(u => u.Type == 'E'), round * units.Sum(u => u.HP));
                }
                    
                var validTargets = enemyUnits
                    .SelectMany(t => new[]
                    {
                        (x:t.X, y:t.Y - 1),
                        (x:t.X - 1, y:t.Y),
                        (x:t.X + 1, y:t.Y),
                        (x:t.X, y:t.Y + 1),
                    })
                    .Select(l => (l.x, l.y, c: board[l.x, l.y]))
                    .Where(t => t.c == null) // open space
                    .Select(t =>
                    {
                        var path = astar.GetPath(
                            new Position(unit.X, unit.Y), new Position(t.x, t.y),
                            MovementPatterns.LateralOnly);
                        return (t.x, t.y, path);
                    })
                    .Where(t => t.path.Any()) // reachable
                    .MinBy(t => t.path.Length) // nearest
                    .OrderBy(t => t.y).ThenBy(t => t.x) // reading order
                    .ToList();
                    
                if (validTargets.Any())
                {
                    var chosen = (validTargets[0].x, validTargets[0].y);

                    // now select how to step to get there
                    var step = new[]
                    {
                        (x:unit.X, y:unit.Y - 1),
                        (x:unit.X - 1, y:unit.Y),
                        (x:unit.X + 1, y:unit.Y),
                        (x:unit.X, y:unit.Y + 1),
                    }
                    .Select(l => (l.x, l.y, c: board[l.x, l.y]))
                    .Where(t => t.c == null) // open space
                    .Select(t =>
                    {
                        var path = astar.GetPath(
                            new Position(t.x, t.y), new Position(chosen.x, chosen.y),
                            MovementPatterns.LateralOnly);
                        return (t.x, t.y, path);
                    })
                    .Where(t => t.path.Any())
                    .MinBy(t => t.path.Length) // nearest
                    .OrderBy(t => t.y).ThenBy(t => t.x) // reading order
                    .First();
                    
                    board[unit.X, unit.Y] = null;
                    astar.UnblockCell(new Position(unit.X, unit.Y));

                    unit.X = step.x;
                    unit.Y = step.y;

                    board[unit.X, unit.Y] = unit;
                    astar.BlockCell(new Position(unit.X, unit.Y));

                    Attack();
                }
            }
        }
    }

    void Render(int round)
    {
        var dup = (object[,])board.Clone();
        foreach (var u in units)
            dup[u.X, u.Y] = u.Type;

        for (var y = 0; y < lines.Length; ++y)
        {
            Console.WriteLine(new string(Enumerable.Range(0, cx).Select(x => dup[x, y])
                .Select(c =>
                {
                    if (c == null)
                        return '.';
                    if (c is Unit u)
                        return u.Type;
                    return (char)c;
                })
                .ToArray()));
        }
        Console.WriteLine();

        Console.WriteLine($"ROUND: {round} (attack = {attackPower})");
        Console.WriteLine($"G: " + string.Join(", ", units.Where(u => u.Type == 'G').Select(u => u.HP.ToString())));
        Console.WriteLine($"E: " + string.Join(", ", units.Where(u => u.Type == 'E').Select(u => u.HP.ToString())));
        
        Console.WriteLine();
        Console.WriteLine();
    }
}
