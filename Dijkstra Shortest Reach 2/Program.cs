using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snakes_and_Ladders
{
    /**
     * TODO: Use a binary tree for PriorityQueue
     */
    public class PriorityQueue<T> where T : IComparable
    {
        private readonly List<T> _queue = new List<T>();
        public int Count => _queue.Count;
        public bool Empty => Count == 0;

        private readonly Func<T, T, int> _comparer;

        public PriorityQueue(Func<T, T, int> comparer)
        {
            _comparer = comparer;
        }

        public void Enqueue(T element)
        {
            int index;
            for (index = 0; index < _queue.Count; index++)
            {
                if (_comparer(element, _queue[index]) < 0)
                    break;
            }

            if (index == _queue.Count)
                _queue.Add(element);
            else
                _queue.Insert(index, element);
        }

        public T Peek()
        {
            EnsureQueueNotEmpty();
            return _queue[0];
        }

        public T Dequeue()
        {
            EnsureQueueNotEmpty();

            T element = _queue[0];
            _queue.RemoveAt(0);

            return element;
        }

        public void Update(T element)
        {
            int index = _queue.IndexOf(element);
            if (index == -1)
                throw new KeyNotFoundException();

            _queue.RemoveAt(index);
            Enqueue(element);
        }

        public bool Contains(T element)
        {
            return _queue.Contains(element);
        }

        private void EnsureQueueNotEmpty()
        {
            if (!Empty)
                return;

            throw new InvalidOperationException("Queue is empty");
        }
    }

    public struct Edge
    {
        public int From;
        public int To;
        public int Weight;

        public Edge(int @from, int to, int weight = 1)
        {
            From = @from;
            To = to;
            Weight = weight;
        }
    }

    /**
     * TODO: Add another graph implementation using neighbor lists instead of edges matrix
     */
    public class Graph
    {
        private readonly int?[,] _edges;

        public Graph(int n, bool directed = false)
        {
            _edges = new int?[n, n];
            VerticesCount = n;
            Directed = directed;
        }

        public void AddEdge(int v1, int v2, int weight = 1)
        {
            _edges[v1, v2] = weight;

            if (!Directed)
                _edges[v2, v1] = weight;
        }

        public IEnumerable<Edge> OutEdges(int v)
        {
            return OutNeighbors(v).Select(v2 => new Edge(v, v2, _edges[v, v2].Value));
        }

        public int OutDegree(int v)
        {
            return OutNeighbors(v).Count();
        }

        public int InDegree(int v)
        {
            return InNeighbors(v).Count();
        }

        public int? GetEdgeWeight(int from, int to)
        {
            return _edges[from, to];
        }

        public void UpdateEdgeWeight(int from, int to, int newWeight)
        {
            _edges[from, to] = newWeight;
        }

        public int VerticesCount { get; }
        public bool Directed { get; }

        private IEnumerable<int> OutNeighbors(int v)
        {
            for (int v2 = 0; v2 < VerticesCount; v2++)
            {
                if (_edges[v, v2].HasValue)
                    yield return v2;
            }
        }

        private IEnumerable<int> InNeighbors(int v)
        {
            for (int v2 = 0; v2 < VerticesCount; v2++)
            {
                if (_edges[v2, v].HasValue)
                    yield return v2;
            }
        }
    }

    public struct PathInfo
    {
        public int? Previous;
        public int? Distance;

        public PathInfo(int? previous, int? distance)
        {
            Previous = previous;
            Distance = distance;
        }
    }

    public static class GraphExtender
    {
        public static PathInfo[] DijkstraShortestPaths(this Graph g, int startingV)
        {
            int?[] previous = new int?[g.VerticesCount];
            int?[] distance = new int?[g.VerticesCount];
            bool[] wasVertexInQueue = new bool[g.VerticesCount];

            distance[startingV] = 0;
            previous[startingV] = null;

            var verticesQueue = new PriorityQueue<int>((v1, v2) => distance[v1].Value - distance[v2].Value);
            verticesQueue.Enqueue(startingV);
            wasVertexInQueue[startingV] = true;

            while (!verticesQueue.Empty)
            {
                int v = verticesQueue.Dequeue();

                foreach (var e in g.OutEdges(v))
                {
                    int possibleDistance = distance[e.From].Value + e.Weight;

                    if (!distance[e.To].HasValue || distance[e.To] > possibleDistance)
                    {
                        distance[e.To] = possibleDistance;
                        previous[e.To] = e.From;

                        if (verticesQueue.Contains(e.To))
                            verticesQueue.Update(e.To);
                        else
                        {
                            wasVertexInQueue[e.To] = true;
                            verticesQueue.Enqueue(e.To);
                        }
                    }

                }
            }

            var pathInfo = new PathInfo[g.VerticesCount];
            for (var i = 0; i < g.VerticesCount; i++)
                pathInfo[i] = new PathInfo(previous[i], distance[i]);
            return pathInfo;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int testsCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < testsCount; i++)
            {
                int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                int verticesCount = numbers[0],
                    edgesCount = numbers[1];

                var graph = new Graph(verticesCount, false);

                for (int j = 0; j < edgesCount; j++)
                {
                    numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                    int from = numbers[0] - 1,
                        to = numbers[1] - 1,
                        weight = numbers[2];

                    int? previousEdgeWeight = graph.GetEdgeWeight(from, to);
                    if (!previousEdgeWeight.HasValue || previousEdgeWeight > weight)
                        graph.AddEdge(from, to, weight);
                }

                int startVertex = int.Parse(Console.ReadLine()) - 1;

                var pathInfo = graph.DijkstraShortestPaths(startVertex);
                for (int v = 0; v < verticesCount; v++)
                {
                    if (v == startVertex)
                        continue;

                    var distance = pathInfo[v].Distance;
                    if (distance.HasValue)
                        Console.Write($"{distance.Value} ");
                    else
                        Console.Write("-1 ");
                }
                Console.WriteLine(String.Empty);
            }
        }
    }
}
