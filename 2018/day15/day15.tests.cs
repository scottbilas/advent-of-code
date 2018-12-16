using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoC;
using MoreLinq.Extensions;
using NUnit.Framework;
using RoyT.AStar;
using Shouldly;

namespace Day15
{
    class Day15 : AocFixture
    {
        static string[][] CutBoards(string text)
        {
            var lines = text.Split('\n').Where(l => l.Trim().StartsWith("#")).ToList();
            var offsets = Regex.Matches(lines[0], @"#+");

            var boards = new string[offsets.Count][];
            for (var iboard = 0; iboard < offsets.Count; ++iboard)
            {
                boards[iboard] = new string[lines.Count];
                for (var iline = 0; iline < lines.Count; ++iline)
                    boards[iboard][iline] = lines[iline].Substring(offsets[iboard].Index, offsets[iboard].Length);
            }

            return boards;
        }
        
        [Test]
        public void UnitOrder_MatchesSample()
        {
            var boardData = CutBoards(@"
                                 would take their
                These units:   turns in this order:
                  #######           #######
                  #.G.E.#           #.1.2.#
                  #E.G.E#           #3.4.5#
                  #.G.E.#           #.6.7.#
                  #######           #######
                ");

            var board = Board.Parse(boardData[0]);
            board.Units = board.Units.Shuffle(new Random(0)).ToList();

            //
            board.Units = board.Units.OrderByReading().ToList();
            //

            board
                .Render(u => (char)(board.Units.IndexOf(u) + '1')).ToLines()
                .ShouldBe(boardData[1]);
        }

        [Test]
        public void SelectAdjacent_IsInReadingOrder()
        {
            var adjacent = new Position(5, 10).SelectAdjacent().ToList();
            var ordered = adjacent.OrderByReading();
            
            ordered.ShouldBe(adjacent);
        }

        [Test]
        public void MoveTargetSelection_MatchesSample()
        {
            var boardData = CutBoards(@"
                Targets:      In range:     Reachable:    Nearest:      Chosen:
                #######       #######       #######       #######       #######
                #E..G.#       #E.?G?#       #E.@G.#       #E.!G.#       #E.+G.#
                #...#.#  -->  #.?.#?#  -->  #.@.#.#  -->  #.!.#.#  -->  #...#.#
                #.G.#G#       #?G?#G#       #@G@#G#       #!G.#G#       #.G.#G#
                #######       #######       #######       #######       #######
                ");

            var board = Board.Parse(boardData[0]);
            var pathfinder = board.GeneratePathfinder();

            // Targets
            
            var targetsRender = board.Render();
            targetsRender.ToLines().ShouldBe(boardData[0]);

            var friendlyUnit = board.Units.Single(u => u.Type == 'E');
            var enemyUnits = board.SelectEnemyUnits(friendlyUnit).ToList();
            enemyUnits.Count.ShouldBe(3);

            // In range
            
            var inRangeRender = board.Render();
            var inRangeTargets = board.SelectEnemyMoveTargets(enemyUnits).ToList();
            foreach (var inRangeTarget in inRangeTargets)
                inRangeRender[inRangeTarget.X, inRangeTarget.Y] = '?';
            inRangeRender.ToLines().ShouldBe(boardData[1]);

            // Reachable
            
            var reachableRender = board.Render();
            var reachableTargets = board.SelectReachableMoveTargets(friendlyUnit, inRangeTargets, pathfinder);
            foreach (var reachableTarget in reachableTargets)
                reachableRender[reachableTarget.pos.X, reachableTarget.pos.Y] = '@';
            reachableRender.ToLines().ShouldBe(boardData[2]);
            
            // Nearest

            var nearestRender = board.Render();
            var nearestTargets = board.SelectNearestMoveTarget(reachableTargets);
            foreach (var nearestTarget in nearestTargets)
                nearestRender[nearestTarget.X, nearestTarget.Y] = '!';
            nearestRender.ToLines().ShouldBe(boardData[3]);
            
            // Chosen
            
            var chosenRender = board.Render();
            var chosenTarget = board.ChooseMoveTarget(nearestTargets);
            chosenTarget.ShouldNotBeNull();
            chosenRender[chosenTarget.Value.X, chosenTarget.Value.Y] = '+';
            chosenRender.ToLines().ShouldBe(boardData[4]);
        }
        
        [Test]
        public void Main()
        {
            // sample
            
            Board.Si2("#######", // #######   
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
            
            Board.Sim("#######", //      #######
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
            
            Board.Si2("#######", //      #######   
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
            
            Board.Si2("#######", //      #######   
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
            
            Board.Si2("#######", //      #######   
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
            
            Board.Si2("#########", //      #########   
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
        
            var result = Board.Si2(File.ReadAllLines($"{ScriptDir}/input.txt"));
            //result.outcome3.Dump();
            //result.elvesOutcome.Dump();
            result.ShouldBe((188576, 57112));
        }        
    }
}
