using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq.Extensions;
using RoyT.AStar;
using AoC;

namespace Day15
{
    static class Extensions
    {
        public static IEnumerable<T> OrderByReading<T>(this IEnumerable<T> @this, Func<T, Position> selector)
        {
            return
                from item in @this
                let pos = selector(item)
                orderby pos.Y, pos.X
                select item;
        }

        public static IEnumerable<Position> OrderByReading(this IEnumerable<Position> @this)
            => @this.OrderByReading(_ => _);

        public static IEnumerable<Unit> OrderByReading(this IEnumerable<Unit> @this)
            => @this.OrderByReading(u => u.Pos);

        public static IEnumerable<Position> SelectAdjacent(this Position @this)
        {
            yield return new Position(@this.X, @this.Y - 1);
            yield return new Position(@this.X - 1, @this.Y);
            yield return new Position(@this.X + 1, @this.Y);
            yield return new Position(@this.X, @this.Y + 1);
        }

        public static IEnumerable<T> SelectAdjacent<T>(this Position @this, Func<Position, T> selector)
            => @this.SelectAdjacent().Select(selector);
    }

    [DebuggerDisplay("{Type} @{Pos.X},{Pos.Y} = {HP}")]
    class Unit
    {
        public char Type { get; private set; }
        public int AttackPower { get; set; } = 3;

        public Position Pos;
        public int HP = 200;

        public Unit(char type, int x, int y)
        {
            Type = type;
            Pos = new Position(x, y);
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

            foreach (var (x, y) in Cells.SelectCoords())
            {
                var c = lines[y][x];
                if (c == '#')
                {
                    Cells[x, y] = c;
                }
                else if (c != '.')
                {
                    var unit = new Unit(c, x, y);
                    Units.Add(unit);
                    Cells[x, y] = unit;
                }
            }
        }
        
        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);
        
        public static int Sim(params string[] lines)
        {
            return SimFull(3, lines).outcome;
        }
        
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

        public IEnumerable<Unit> SelectEnemyUnits(Unit self)
            => Units.Where(t => t.Type != self.Type);

        public IEnumerable<Position> SelectEnemyMoveTargets(IEnumerable<Unit> enemyUnits)
            => enemyUnits
                .SelectMany(e => e.Pos.SelectAdjacent())
                .Where(p => Cells[p.X, p.Y] == null);

        public IEnumerable<(Position pos, int dist)> SelectReachableMoveTargets(Unit self, IEnumerable<Position> moveTargets, Grid pathfinder)
            => moveTargets
                .Select(pos => (pos, dist: pathfinder.GetPath(self.Pos, pos, MovementPatterns.LateralOnly).Length))
                .Where(target => target.dist != 0);

        public IEnumerable<Position> SelectNearestMoveTarget(IEnumerable<(Position pos, int dist)> moveTargets)
            => moveTargets
                .MinBy(target => target.dist)
                .Select(target => target.pos);

        public Position? ChooseMoveTarget(IEnumerable<Position> moveTargets)
            => moveTargets
                .OrderByReading()
                .FirstOrDefault((pos, hasValue) => hasValue ? pos : (Position?)null);

        public Grid GeneratePathfinder()
        {
            var pathfinder = new Grid(Width, Height);
            foreach (var (_, x, y) in Cells.SelectCells().Where(c => c.cell != null))
                pathfinder.BlockCell(new Position(x, y));

            return pathfinder;
        }
        
        public static (int edeaths, int outcome) SimFull(int elfAttackPower, string[] lines)
        {
            var board = new Board(lines);
        
            var elfCount = 0; 
            foreach (var elf in board.Units.Where(u => u.Type == 'E'))
            {
                elf.AttackPower = elfAttackPower;
                ++elfCount;
            }

            var pathfinder = board.GeneratePathfinder();
    
            for (var round = 0; ; ++round)
            {
                //Render(round);
        
                board.Units = board.Units.OrderByReading(u => u.Pos).ToList();
        
                for (var iunit = 0; iunit < board.Units.Count; ++iunit)
                {
                    var unit = board.Units[iunit];
        
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
                            pathfinder.UnblockCell(target.Pos);
                            
                            var found = board.Units.IndexOf(target);
                            board.Units.RemoveAt(found);
                            if (found < iunit)
                                --iunit;
                        }
                        
                        return true;
                    }
        
                    if (!Attack())
                    {
                        var enemyUnits = board.Units
                            .Where(t => t.Type != unit.Type)
                            .ToList();
        
                        if (!enemyUnits.Any())
                        {
                            //Render(round);
                            return (
                                elfCount - board.Units.Count(u => u.Type == 'E'),
                                round * board.Units.Sum(u => u.HP));
                        }
                            
                        var validTargets = enemyUnits
                            .SelectMany(e => e.Pos.SelectAdjacent())
                            .Where(p => board.Cells[p.X, p.Y] == null) // open space
                            .Select(p => (pos: p, path: pathfinder.GetPath(unit.Pos, p, MovementPatterns.LateralOnly)))
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
                                .Select(p => (pos: p, path: pathfinder.GetPath(p, chosen, MovementPatterns.LateralOnly)))
                                .Where(t => t.path.Any())
                                .MinBy(t => t.path.Length) // nearest
                                .OrderByReading(t => t.pos) // reading order
                                .First();
                            
                            board.Cells[unit.Pos.X, unit.Pos.Y] = null;
                            pathfinder.UnblockCell(new Position(unit.Pos.X, unit.Pos.Y));
        
                            unit.Pos = step.pos;
        
                            board.Cells[unit.Pos.X, unit.Pos.Y] = unit;
                            pathfinder.BlockCell(unit.Pos);
        
                            Attack();
                        }
                    }
                }
            }
        }

        public char[,] Render(Func<Unit, char> unitRenderer)
        {
            return new char[Width, Height]
                .Fill(coord =>
                {
                    switch (Cells[coord.x, coord.y])
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
