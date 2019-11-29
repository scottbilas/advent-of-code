using System.Drawing;
using Aoc2018;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Day22
{
    class Handle
    {
        public Point Pos;
        public Equip Equip;
    }

    struct Room
    {
        public Handle Climbing;
        public Handle Torch;
        public Handle Neither;
    }

    class QuickGraphSolver : Solver
    {
        public QuickGraphSolver(Point targetPos, Size padding)
            : base(targetPos, padding) { }

        public int CalcMinDistToTarget(int[,] erosion)
        {
            var board = BuildBoard(erosion);
            var rooms = new Room[Size.Width, Size.Height];

            var graph = new BidirectionalGraph<Handle, Edge<Handle>>();
            for (var y = 0; y < Size.Height; ++y)
            {
                for (var x = 0; x < Size.Width; ++x)
                {
                    var room = new Room();
                    var pos = new Point(x, y);
                    if (board[x, y] == '.')
                    {
                        graph.AddVertex(room.Climbing = new Handle { Pos = pos, Equip = Equip.Climbing });
                        graph.AddVertex(room.Torch = new Handle { Pos = pos, Equip = Equip.Torch });
                        graph.AddEdge(new Edge<Handle>(room.Climbing, room.Torch));
                    }
                    else if (board[x, y] == '=')
                    {
                        graph.AddVertex(room.Climbing = new Handle { Pos = pos, Equip = Equip.Climbing });
                        graph.AddVertex(room.Neither = new Handle { Pos = pos, Equip = Equip.Neither });
                        graph.AddEdge(new Edge<Handle>(room.Climbing, room.Neither));
                    }
                    else
                    {
                        graph.AddVertex(room.Torch = new Handle { Pos = pos, Equip = Equip.Torch });
                        graph.AddVertex(room.Neither = new Handle { Pos = pos, Equip = Equip.Neither });
                        graph.AddEdge(new Edge<Handle>(room.Torch, room.Neither));
                    }

                    rooms[x, y] = room;

                    if (x > 0)
                    {
                        var left = rooms[x - 1, y];
                        if (room.Climbing != null && left.Climbing != null)
                            graph.AddEdge(new Edge<Handle>(left.Climbing, room.Climbing));
                        if (room.Torch != null && left.Torch != null)
                            graph.AddEdge(new Edge<Handle>(left.Torch, room.Torch));
                        if (room.Neither != null && left.Neither != null)
                            graph.AddEdge(new Edge<Handle>(left.Neither, room.Neither));
                    }

                    if (y > 0)
                    {
                        var up = rooms[x, y - 1];
                        if (room.Climbing != null && up.Climbing != null)
                            graph.AddEdge(new Edge<Handle>(up.Climbing, room.Climbing));
                        if (room.Torch != null && up.Torch != null)
                            graph.AddEdge(new Edge<Handle>(up.Torch, room.Torch));
                        if (room.Neither != null && up.Neither != null)
                            graph.AddEdge(new Edge<Handle>(up.Neither, room.Neither));
                    }
                }
            }

            var sourceVertex = rooms[0, 0].Torch;
            var targetVertex = rooms[TargetPos.X, TargetPos.Y].Torch;

            double EdgeWeight(Edge<Handle> edge) => edge.Source.Equip == edge.Target.Equip ? 1 : 7;

            var undirected = new UndirectedBidirectionalGraph<Handle, Edge<Handle>>(graph);
            undirected.ShortestPathsDijkstra(EdgeWeight, sourceVertex)(targetVertex, out var path);

            var iter = sourceVertex;
            var cost = 0;
            foreach (var p in path)
            {
                cost += (int)EdgeWeight(p);
                iter = p.GetOtherVertex(iter);
                //$"{iter.Pos.X},{iter.Pos.Y}: {iter.Equip}".Dump();
            }

            return cost;
        }
    }
}
