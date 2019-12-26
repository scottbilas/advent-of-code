using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using QuickGraph;
using QuickGraph.Graphviz;
using RoyT.AStar;
using Unity.Coding.Utils;
using static Aoc2019.MiscStatics;

namespace Aoc2019.Day18
{
    class Solver
    {
        public static int FindMinimumSteps(char[,] grid, NPath render = null)
        {
            var graph = new VaultGraph(grid);
            if (render != null)
                graph.Render(render);
            return graph.FindMinimumSteps();
        }

        public static char[,] SplitEntrance(char[,] grid)
        {
            grid = (char[,])grid.Clone();

            var center = grid.SelectCells().Single(c => c.cell == '@').pos;
            grid[center.X - 1, center.Y - 1] = '@';
            grid[center.X,     center.Y - 1] = '#';
            grid[center.X + 1, center.Y - 1] = '@';
            grid[center.X - 1, center.Y    ] = '#';
            grid[center.X,     center.Y    ] = '#';
            grid[center.X + 1, center.Y    ] = '#';
            grid[center.X - 1, center.Y + 1] = '@';
            grid[center.X,     center.Y + 1] = '#';
            grid[center.X + 1, center.Y + 1] = '@';

            return grid;
        }
    }

    class VaultGraph
    {
        class Node : List<(char pos, int steps)> {}

        int m_StartCount, m_KeyCount;
        Node[] m_Graph = new Node[k_TotalChars];

        static VaultGraph()
        {
            var i = 0;
            foreach (var (l, r) in Arr(('0', '9'), ('A', 'Z'), ('a', 'z'))) // 0 < A < a
            {
                for (var c = l; c <= r; ++c, ++i)
                {
                    k_ToIndex[c] = i;
                    k_ToBit[c] = 1ul << i;
                    k_FromIndex[i] = c;
                }
            }
        }

        const int k_CharLutLen = 'z' + 1;
        const int k_TotalChars = 10 + 26 + 26;

        static readonly int[] k_ToIndex = new int[k_CharLutLen];
        static readonly ulong[] k_ToBit = new ulong[k_CharLutLen];
        static readonly char[] k_FromIndex = new char[k_CharLutLen];

        public static int ToIndex(char c) => k_ToIndex[c];
        public static ulong ToBit(char c) => k_ToBit[c];
        public static char FromIndex(int i) => k_FromIndex[i];

        static void SetBit(ref ulong bits, char pos) => bits |= ToBit(pos);
        static void ClearBit(ref ulong bits, char pos) => bits &= ~ToBit(pos);
        static bool TestBit(ulong bits, char pos) => (bits & ToBit(pos)) != 0;

        public VaultGraph(char[,] grid)
        {
            var board = With(grid.GetDimensions(), sz => new Grid(sz.X, sz.Y));
            var items = new Dictionary<char, Position>();

            // parse into royt's pathfinder grid

            foreach (var (pos, cell) in grid
                .SelectCells()
                .Select(c => (pos: new Position(c.pos.X, c.pos.Y), c.cell)))
            {
                if (cell == '@')
                {
                    var start = (char)(m_StartCount++ + '0');
                    grid[pos.X, pos.Y] = start;
                    items.Add(start, pos);
                }
                else if (cell != '.')
                {
                    board.BlockCell(pos);
                    if (cell != '#')
                    {
                        items.Add(cell, pos);
                        if (cell.IsAsciiLower())
                            ++m_KeyCount;
                    }
                }
            }

            // run paths from everything to everything to build simplified graph

            for (var i = 0; i < m_Graph.Length; ++i)
                m_Graph[i] = new Node();

            foreach (var (a, b) in
                new Combinations<char>(items.Keys.Ordered().ToList(), 2)
                .Select(l => l.First2()))
            {
                var (apos, bpos) = (items[a], items[b]);
                board.UnblockCell(apos);
                board.UnblockCell(bpos);
                var p = board.GetPath(apos, bpos, MovementPatterns.LateralOnly);
                if (p.Length > 0)
                {
                    var steps = p.Length - 1;
                    m_Graph[ToIndex(a)].Add((b, steps));
                    m_Graph[ToIndex(b)].Add((a, steps));
                }
                board.BlockCell(apos);
                board.BlockCell(bpos);
            }
        }

        unsafe struct MoveState
        {
            // need to write custom hash otherwise it's slow as ef (like 20x slower)
            public int CalcHashCode(int moverCount, ulong removed)
            {
                var hash = new HashCode();
                hash.Add(Steps);
                hash.Add(removed);

                for (var i = 0; i < moverCount; ++i)
                    hash.Add(Pos[i]);

                return hash.ToHashCode();
            }

            public fixed char Pos[10];
            public int Steps;
        }

        public unsafe int FindMinimumSteps()
        {
            var minSteps = int.MaxValue;
            var removed = 0ul;
            var removedCount = 0;
            var cache = new Dictionary<int /*state hash*/, int /*steps*/>();

            var walkQueue = new Queue<(char pos, int steps)>();
            var solveStack = new Stack<(int mover, char to, int steps)>();

            void Move(MoveState moveState)
            {
                // find all reachable keys

                var solveStackStart = solveStack.Count;

                for (var mover = 0; mover < m_StartCount; ++mover)
                {
                    var walkFrom = (pos: moveState.Pos[mover], steps: moveState.Steps);
                    var walkVisited = ToBit(walkFrom.pos);
                    do
                    {
                        foreach (var (to, stepsTo) in m_Graph[ToIndex(walkFrom.pos)])
                        {
                            if (TestBit(walkVisited, to))
                                continue;
                            SetBit(ref walkVisited, to);

                            var totalStepsTo = walkFrom.steps + stepsTo;

                            if (TestBit(removed, to)) // pass through and continue search
                                walkQueue.Enqueue((to, totalStepsTo));
                            else if (to.IsAsciiLower())
                                solveStack.Push((mover, to, totalStepsTo));
                        }
                    }
                    while (walkQueue.TryDequeue(out walkFrom));
                }

                // iterate through removing each found key, recursing to branch

                while (solveStack.Count > solveStackStart)
                {
                    var (mover, to, steps) = solveStack.Pop();

                    if (removedCount == m_KeyCount - 1)
                    {
                        // this is the last key
                        minSteps = Math.Min(minSteps, steps);
                    }
                    else
                    {
                        var targetState = moveState;
                        targetState.Pos[mover] = to;
                        targetState.Steps = steps;

                        var hash = targetState.CalcHashCode(m_StartCount, removed);
                        var hadCache = cache.TryGetValue(hash, out var cacheSteps);
                        if (!hadCache || cacheSteps > steps)
                        {
                            SetBit(ref removed, to);
                            SetBit(ref removed, to.ToAsciiUpper());
                            ++removedCount;

                            Move(targetState);

                            --removedCount;
                            ClearBit(ref removed, to.ToAsciiUpper());
                            ClearBit(ref removed, to);

                            cache[hash] = steps;
                        }
                    }
                }
            }

            var pos = new MoveState();
            for (var i = 0; i < m_StartCount; ++i)
            {
                var c = FromIndex(i);
                pos.Pos[i] = c;
                SetBit(ref removed, c);
            }

            Move(pos);

            return minSteps;
        }

        // graphing

        public UndirectedGraph<char, SUndirectedTaggedEdge<char, int>> ToGraphViz()
        {
            var viz = new UndirectedGraph<char, SUndirectedTaggedEdge<char, int>>();

            var skip = new HashSet<(char, char)>();
            for (var i = 0; i < k_TotalChars; ++i)
            {
                foreach (var (from, steps) in m_Graph[i])
                {
                    var to = FromIndex(i);
                    if (skip.Add((from, to)) && skip.Add((to, from)))
                        viz.AddVerticesAndEdge(new SUndirectedTaggedEdge<char, int>(FromIndex(i), from, steps));
                }
            }

            return viz;
        }

        public void Render(NPath path) =>
            Utils.RenderGraphViz(path, ToGraphViz().ToGraphviz(g =>
                {
                    g.FormatVertex += (_, args) => args.VertexFormatter.Comment = args.Vertex.ToString();
                    g.FormatEdge += (_, args) =>
                    {
                        args.EdgeFormatter.Label.Value = $"  {args.Edge.Tag}  ";
                        args.EdgeFormatter.Length = (int)Math.Log(args.Edge.Tag);
                    };
                }), "-Kneato");
    }
}
