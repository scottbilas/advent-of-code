using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using MoreLinq.Extensions;
using RoyT.AStar;
using AoC;

namespace Day15
{
    static class Extensions
    {
        public static IEnumerable<Unit> OrderByReading(this IEnumerable<Unit> @this)
            => @this.OrderByReading(u => u.Pos);

        public static Point ToPoint(ref this Position @this)
            => new Point(@this.X, @this.Y);
        
        public static Position ToPosition(ref this Point @this)
            => new Position(@this.X, @this.Y);
    }
    
    [DebuggerDisplay("{Type} @{Pos.X},{Pos.Y} = {HP}")]
    class Unit
    {
        public char Type { get; private set; }
        public int AttackPower { get; set; } = 3;

        public Point Pos;
        public int HP = 200;

        public Unit(char type, int x, int y)
        {
            Type = type;
            Pos = new Point(x, y);
        }

        public Unit(char type, int x, int y, int attackPower)
            : this(type, x, y)
        {
            if (Type == 'E')
                AttackPower = attackPower;
        }
    }
    
    class Board
    {
        public object[,] Cells;
        public List<Unit> Units = new List<Unit>();

        public Board(params string[] lines)
        {
            var (cx, cy) = (lines[0].Length, lines.Length);
            Cells = new object[cx, cy];

            foreach (var coord in Cells.SelectCoords())
            {
                var c = lines[coord.Y][coord.X];
                if (c == '#')
                {
                    Cells[coord.X, coord.Y] = c;
                }
                else if (c != '.')
                {
                    var unit = new Unit(c, coord.X, coord.Y);
                    Units.Add(unit);
                    Cells[coord.X, coord.Y] = unit;
                }
            }
        }
        
        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);
        
        public static int Sim(params string[] lines)
            => SimFull(3, lines).outcome;
        
        public static (int outcome3, int elvesOutcome) Si2(params string[] lines)
        {
            var outcome3 = Sim(lines);
            for (var attackPower = 4; ; ++attackPower)
            {
                var result = SimFull(attackPower, lines);
                if (result.edeaths == 0)
                    return (outcome3, result.outcome);
            }
        }

        public static int GetPathDistance(Grid pathfinder, Point start, Point end)
            => pathfinder.GetPath(start.ToPosition(), end.ToPosition(), MovementPatterns.LateralOnly).Length;
        
        public IEnumerable<Unit> SelectEnemyUnits(Unit self)
            => Units.Where(t => t.Type != self.Type);

        public IEnumerable<Point> SelectEnemyMoveTargets(IEnumerable<Unit> enemyUnits)
            => enemyUnits
                .SelectMany(e => e.Pos.SelectAdjacent())
                .Where(p => Cells[p.X, p.Y] == null);

        public IEnumerable<(Point pos, int dist)> SelectReachableMoveTargets(Unit self, IEnumerable<Point> moveTargets, Grid pathfinder)
            => moveTargets
                .Select(pos => (pos, dist: GetPathDistance(pathfinder, self.Pos, pos)))
                .Where(target => target.dist != 0);

        public IEnumerable<Point> SelectNearestMoveTarget(IEnumerable<(Point pos, int dist)> moveTargets)
            => moveTargets
                .MinBy(target => target.dist)
                .Select(target => target.pos);

        public Point? ChooseMoveTarget(IEnumerable<Point> moveTargets)
            => moveTargets
                .OrderByReading()
                .FirstOrNull();

        public Point ChooseMoveStep(Grid pathfinder, Point current, Point target)
            => current
                .SelectAdjacent()
                .Where(pos => Cells[pos.X, pos.Y] == null)
                .Select(pos => (pos, dist: GetPathDistance(pathfinder, pos, target)))
                .MinBy(path => path.dist)
                .Select(path => path.pos)
                .OrderByReading()
                .First();
        
        public Grid GeneratePathfinder()
        {
            var pathfinder = new Grid(Width, Height);
            foreach (var (_, x, y) in Cells.SelectCells().Where(c => c.cell != null))
                pathfinder.BlockCell(new Position(x, y));

            return pathfinder;
        }
        
        public static (int edeaths, int outcome, Board board)
            SimFull(int elfAttackPower, string[] lines, int maxRound = int.MaxValue)
        {
            var board = new Board(lines);
        
            var elfCount = 0; 
            foreach (var elf in board.Units.Where(u => u.Type == 'E'))
            {
                elf.AttackPower = elfAttackPower;
                ++elfCount;
            }

            var round = 0;

            (int edeaths, int outcome, Board board) GetReturn()
            {
                return (
                    edeaths: elfCount - board.Units.Count(u => u.Type == 'E'),
                    outcome: round * board.Units.Sum(u => u.HP),
                    board);
            }

            var pathfinder = board.GeneratePathfinder();

            for (; round < maxRound; ++round)
            {
                //Render(round);
        
                board.Units = board.Units.OrderByReading(u => u.Pos).ToList();
        
                for (var iUnit = 0; iUnit < board.Units.Count; ++iUnit)
                {
                    var unit = board.Units[iUnit];
        
                    bool Attack()
                    {
                        var target = unit.Pos
                            .SelectAdjacent(p => board.Cells[p.X, p.Y])
                            .OfType<Unit>()
                            .Where(t => t.Type != unit.Type)
                            .MinBy(t => t.HP)
                            .FirstOrDefault();
                        
                        if (target == null)
                            return false;
                        
                        target.HP -= unit.AttackPower;
                        
                        if (target.HP <= 0)
                        {
                            board.Cells[target.Pos.X, target.Pos.Y] = null;
                            pathfinder.UnblockCell(target.Pos.ToPosition());
                            
                            var found = board.Units.IndexOf(target);
                            board.Units.RemoveAt(found);
                            if (found < iUnit)
                                --iUnit;
                        }
                        
                        return true;
                    }
        
                    if (!Attack())
                    {
                        var enemyUnits = board.Units
                            .Where(t => t.Type != unit.Type)
                            .ToList();
        
                        if (!enemyUnits.Any())
                            return GetReturn();
                            
                        var validTargets = enemyUnits
                            .SelectMany(e => e.Pos.SelectAdjacent())
                            .Where(p => board.Cells[p.X, p.Y] == null) // open space
                            .Select(p => (pos: p, path: pathfinder.GetPath(unit.Pos.ToPosition(), p.ToPosition(), MovementPatterns.LateralOnly)))
                            .Where(t => t.path.Any()) // reachable
                            .MinBy(t => t.path.Length) // nearest
                            .OrderByReading(t => t.pos) // reading order
                            .ToList();
                            
                        if (validTargets.Any())
                        {
                            var chosen = validTargets.First().pos;
        
                            // now select how to step to get there
                            var step = unit.Pos
                                .SelectAdjacent()
                                .Where(p => board.Cells[p.X, p.Y] == null)
                                .Select(p => (pos: p, path: pathfinder.GetPath(p.ToPosition(), chosen.ToPosition(), MovementPatterns.LateralOnly)))
                                .Where(t => t.path.Any())
                                .MinBy(t => t.path.Length) // nearest
                                .OrderByReading(t => t.pos) // reading order
                                .First();
                            
                            board.Cells[unit.Pos.X, unit.Pos.Y] = null;
                            pathfinder.UnblockCell(unit.Pos.ToPosition());
        
                            unit.Pos = step.pos;
        
                            board.Cells[unit.Pos.X, unit.Pos.Y] = unit;
                            pathfinder.BlockCell(unit.Pos.ToPosition());
        
                            Attack();
                        }
                    }
                }
            }
            
            return GetReturn();
        }

        public char[,] Render(Func<Unit, char> unitRenderer)
        {
            return new char[Width, Height]
                .Fill(coord =>
                {
                    switch (Cells[coord.X, coord.Y])
                    {
                        case Unit unit:
                            return unitRenderer?.Invoke(unit) ?? unit.Type;
                        case char c:
                            return c;
                        case null:
                            return '.';
                        default:
                            throw new InvalidOperationException(); 
                    }
                });
        }

        public char[,] Render()
            => Render(u => u.Type);
    }
}
