using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFS
{
    class Graph
    {
        private int _n;
        private bool[,] _edges;

        public Graph(int n)
        {
            _n = n;
            _edges = new bool[n, n];
        }

        public void AddEdge(int v1, int v2)
        {
            _edges[v1, v2] = _edges[v2, v1] = true;
        }

        public IEnumerable<int> GetNeighbors(int v)
        {
            for (int otherV = 0; otherV < _n; otherV++)
            {
                if (_edges[v, otherV])
                    yield return otherV;
            }
        }

        public void BFS(int startV, out int?[] previousVertex, out int[] distance)
        {
            var vertexQueue = new Queue<int>(_n);
            bool[] wasVertexInQueue = new bool[_n];
            previousVertex = new int?[_n];
            distance = new int[_n];
            for (int v = 0; v < _n; v++)
            {
                previousVertex[v] = null;
                distance[v] = int.MaxValue;
            }

            vertexQueue.Enqueue(startV);
            wasVertexInQueue[startV] = true;
            previousVertex[startV] = null;
            distance[startV] = 0;

            while (vertexQueue.Count > 0)
            {
                int v = vertexQueue.Dequeue();
                int nextDistance = distance[v] + 1;

                foreach (int neighbor in GetNeighbors(v))
                {
                    if (!wasVertexInQueue[neighbor])
                    {
                        wasVertexInQueue[neighbor] = true;
                        vertexQueue.Enqueue(neighbor);
                    }

                    if (nextDistance < distance[neighbor])
                    {
                        distance[neighbor] = nextDistance;
                        previousVertex[neighbor] = v;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int queries = int.Parse(Console.ReadLine());
            for (int i = 0; i < queries; i++)
            {
                int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                int n = numbers[0],
                    edges = numbers[1];
                Graph g = new Graph(n);

                for (int j = 0; j < edges; j++)
                {
                    numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                    g.AddEdge(numbers[0] - 1, numbers[1] - 1);
                }

                int startV = int.Parse(Console.ReadLine()) - 1;
                int?[] previousVertex;
                int[] distance;

                g.BFS(startV, out previousVertex, out distance);

                List<int> distanceList = distance.Select(number =>
                {
                    if (number == int.MaxValue)
                        return -1;

                    return number * 6;
                }).ToList();
                distanceList.RemoveAt(startV);

                Console.WriteLine(string.Join(" ", distanceList.Select(number => number.ToString()).ToArray()));
            }
        }
    }
}
