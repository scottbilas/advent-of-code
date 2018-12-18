using System;
using System.Collections.Generic;
using System.Drawing;
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
            for (var iBoard = 0; iBoard < offsets.Count; ++iBoard)
            {
                boards[iBoard] = new string[lines.Count];
                for (var iLine = 0; iLine < lines.Count; ++iLine)
                    boards[iBoard][iLine] = lines[iLine].Substring(offsets[iBoard].Index, offsets[iBoard].Length);
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

            var board = new Board(boardData[0]);
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
            var adjacent = new Point(5, 10).SelectAdjacent().ToList();
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

            var board = new Board(boardData[0]);
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
        public void MoveStepSelection_MatchesSample()
        {
            var boardData = CutBoards(@"
                In range:     Nearest:      Chosen:       Distance:     Step:
                #######       #######       #######       #######       #######
                #.E...#       #.E...#       #.E...#       #4E212#       #..E..#
                #...?.#  -->  #...!.#  -->  #...+.#  -->  #32101#  -->  #.....#
                #..?G?#       #..!G.#       #...G.#       #432G2#       #...G.#
                #######       #######       #######       #######       #######
                ");
            
            var board = new Board(boardData[0].Select(l => l.Replace("?", ".")).ToArray());
            var pathfinder = board.GeneratePathfinder();

            // In range

            var friendlyUnit = board.Units.Single(u => u.Type == 'E');
            var enemyUnits = board.SelectEnemyUnits(friendlyUnit).ToList();
            enemyUnits.Count.ShouldBe(1);

            var inRangeRender = board.Render();
            var inRangeTargets = board.SelectReachableMoveTargets(friendlyUnit, board.SelectEnemyMoveTargets(enemyUnits), pathfinder).ToList();
            foreach (var (inRangeTarget, _) in inRangeTargets)
                inRangeRender[inRangeTarget.X, inRangeTarget.Y] = '?';
            inRangeRender.ToLines().ShouldBe(boardData[0]);
            
            // Nearest

            var nearestRender = board.Render();
            var nearestTargets = board.SelectNearestMoveTarget(inRangeTargets).ToList();
            foreach (var nearestTarget in nearestTargets)
                nearestRender[nearestTarget.X, nearestTarget.Y] = '!';
            nearestRender.ToLines().ShouldBe(boardData[1]);
            
            // Chosen
            
            var chosenRender = board.Render();
            var chosenTargetOpt = board.ChooseMoveTarget(nearestTargets);
            chosenTargetOpt.ShouldNotBeNull();
            var chosenTarget = chosenTargetOpt.Value;
            chosenRender[chosenTarget.X, chosenTarget.Y] = '+';
            chosenRender.ToLines().ShouldBe(boardData[2]);
            
            // Distance

            var distanceRender = board.Render();
            foreach (var (_, x, y) in distanceRender.SelectCells().Where(c => c.cell == '.'))
            {
                var dist = Board.GetPathDistance(pathfinder, new Point(x, y), chosenTarget);
                distanceRender[x, y] = (char)(dist - 1 + '0');
            }
            distanceRender.ToLines().ShouldBe(boardData[3]);
            
            // Step
            
            var stepRender = board.Render();
            var chosenStep = board.ChooseMoveStep(pathfinder, friendlyUnit.Pos, chosenTarget);
            stepRender[friendlyUnit.Pos.X, friendlyUnit.Pos.Y] = '.';
            stepRender[chosenStep.X, chosenStep.Y] = friendlyUnit.Type;
            stepRender.ToLines().ShouldBe(boardData[4]);
        }

        [Test]
        public void MoveSimulation_MatchesSample()
        {
            var boardData = CutBoards(@"
                Initially:      After 1 round:  After 2 rounds: After 3 rounds:
                #########       #########       #########       #########
                #G..G..G#       #.G...G.#       #..G.G..#       #.......#
                #.......#       #...G...#       #...G...#       #..GGG..#
                #.......#       #...E..G#       #.G.E.G.#       #..GEG..#
                #G..E..G#       #.G.....#       #.......#       #G..G...#
                #.......#       #.......#       #G..G..G#       #......G#
                #.......#       #G..G..G#       #.......#       #.......#
                #G..G..G#       #.......#       #.......#       #.......#
                #########       #########       #########       #########
                ");

            IEnumerable<string> Sim(int maxRound)
                => Board.SimFull(3, boardData[0], maxRound).board.Render().ToLines(); 
            
            Sim(0).ShouldBe(boardData[0]);
            Sim(1).ShouldBe(boardData[1]);
            Sim(2).ShouldBe(boardData[2]);
            Sim(3).ShouldBe(boardData[3]);
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
        
            var result = Board.Si2(ScriptDir.Combine("input.txt").ReadAllLines());
            //result.outcome3.Dump();
            //result.elvesOutcome.Dump();
            result.ShouldBe((188576, 57112));
        }        
    }
}
