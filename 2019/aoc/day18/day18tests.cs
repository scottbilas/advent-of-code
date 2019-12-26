using System.Linq;
using NUnit.Framework;
using Shouldly;
using Unity.Coding.Utils;

namespace Aoc2019.Day18
{
    class Day18Tests : AocFixture
    {
        char[,] ReadInputGrid() => ScriptDir.Combine($"{ScriptDir.FileName}.input.txt").ReadGrid();
        NPath RenderBase => ScriptDir.Combine("graphs").CreateDirectory();
        NPath GetRenderPath() => null; // RenderBase.Combine(TestContext.CurrentContext.Test.MethodName.ToLower() + ".png");

        int[] m_Expected;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_Expected = ExtractResults().ParseInts().ToArray();
        }

        [Test]
        public void CharToIndex()
        {
            var i0 = VaultGraph.ToIndex('0');
            var i9 = VaultGraph.ToIndex('9');
            var iA = VaultGraph.ToIndex('A');
            var iZ = VaultGraph.ToIndex('Z');
            var ia = VaultGraph.ToIndex('a');
            var iz = VaultGraph.ToIndex('z');

            i0.ShouldBe(0);
            i9.ShouldBe(iA - 1);
            iZ.ShouldBe(ia - 1);
            iz.ShouldBe(10 + 26 + 26 - 1);
        }

        [Test]
        public void IndexToChar()
        {
            VaultGraph.FromIndex(VaultGraph.ToIndex('0')).ShouldBe('0');
            VaultGraph.FromIndex(VaultGraph.ToIndex('9')).ShouldBe('9');
            VaultGraph.FromIndex(VaultGraph.ToIndex('A')).ShouldBe('A');
            VaultGraph.FromIndex(VaultGraph.ToIndex('Z')).ShouldBe('Z');
            VaultGraph.FromIndex(VaultGraph.ToIndex('a')).ShouldBe('a');
            VaultGraph.FromIndex(VaultGraph.ToIndex('z')).ShouldBe('z');
        }

        [Test]
        public void Sample11()
        {
            Solver.FindMinimumSteps(@"
                #########
                #b.A.@.a#
                #########
                ".ToGrid(), GetRenderPath()).ShouldBe(8);
        }

        [Test]
        public void Sample12()
        {
            Solver.FindMinimumSteps(@"
                ########################
                #f.D.E.e.C.b.A.@.a.B.c.#
                ######################.#
                #d.....................#
                ########################
                ".ToGrid(), GetRenderPath()).ShouldBe(86);
        }

        [Test]
        public void Sample13()
        {
            Solver.FindMinimumSteps(@"
                ########################
                #...............b.C.D.f#
                #.######################
                #.....@.a.B.c.d.A.e.F.g#
                ########################
                ".ToGrid(), GetRenderPath()).ShouldBe(132);
        }

        [Test]
        public void Sample14()
        {
            Solver.FindMinimumSteps(@"
                #################
                #i.G..c...e..H.p#
                ########.########
                #j.A..b...f..D.o#
                ########@########
                #k.E..a...g..B.n#
                ########.########
                #l.F..d...h..C.m#
                #################
                ".ToGrid(), GetRenderPath()).ShouldBe(136);
        }

        [Test]
        public void Sample15()
        {
            Solver.FindMinimumSteps(@"
                ########################
                #@..............ac.GI.b#
                ###d#e#f################
                ###A#B#C################
                ###g#h#i################
                ########################
                ".ToGrid(), GetRenderPath()).ShouldBe(81);
        }

        [Test]
        public void Part1()
        {
            Solver.FindMinimumSteps(
                ReadInputGrid(), GetRenderPath())
                .ShouldBe(m_Expected[0]);
        }

        [Test]
        public void Remap()
        {
            Solver.SplitEntrance(@"
                #######
                #a.#Cd#
                ##...##
                ##.@.##
                ##...##
                #cB#Ab#
                #######
                ".ToGrid()).ShouldBe(k_Sample21Text.ToGrid());
        }

        const string k_Sample21Text = @"
            #######
            #a.#Cd#
            ##@#@##
            #######
            ##@#@##
            #cB#Ab#
            #######";

        [Test]
        public void Sample21()
        {
            Solver.FindMinimumSteps(k_Sample21Text.ToGrid(), GetRenderPath()).ShouldBe(8);
        }

        [Test]
        public void Sample22()
        {
            Solver.FindMinimumSteps(@"
                ###############
                #d.ABC.#.....a#
                ######@#@######
                ###############
                ######@#@######
                #b.....#.....c#
                ###############
                ".ToGrid(), GetRenderPath()).ShouldBe(24);
        }

        [Test]
        public void Sample23()
        {
            Solver.FindMinimumSteps(@"
                #############
                #DcBa.#.GhKl#
                #.###@#@#I###
                #e#d#####j#k#
                ###C#@#@###J#
                #fEbA.#.FgHi#
                #############
                ".ToGrid(), GetRenderPath()).ShouldBe(32);
        }

        [Test]
        public void Sample24()
        {
            Solver.FindMinimumSteps(@"
                #############
                #g#f.D#..h#l#
                #F###e#E###.#
                #dCba@#@BcIJ#
                #############
                #nK.L@#@G...#
                #M###N#H###.#
                #o#m..#i#jk.#
                #############
                ".ToGrid(), GetRenderPath()).ShouldBe(72);
        }

        [Test]
        public void Part2()
        {
            Solver.FindMinimumSteps(
                Solver.SplitEntrance(ReadInputGrid()),
                GetRenderPath())
                .ShouldBe(m_Expected[1]);
        }
    }
}
