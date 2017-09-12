using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subset_Component
{
    class Program
    {
        class Graph : ICloneable
        {
            private int _n;
            private bool[,] _edges;

            public Graph(int n)
            {
                _n = n;
                _edges = new bool[n, n];
            }

            public IEnumerable<int> GetNeighbors(int v)
            {
                for (int otherV = 0; otherV < _n; otherV++)
                {
                    if (_edges[v, otherV])
                        yield return otherV;
                }
            }

            public void AddEdge(int v1, int v2)
            {
                _edges[v1, v2] = _edges[v2, v1] = true;
            }

            public int BFS(bool[] verticesVisited)
            {
                int startingV;
                for (startingV = 0; startingV < _n; startingV++)
                {
                    if (!verticesVisited[startingV])
                        break;
                }

                if (startingV == _n)
                    return 0;

                var verticesQueue = new Queue<int>();
                verticesQueue.Enqueue(startingV);
                verticesVisited[startingV] = true;
                int componentCount = 1;

                while (verticesQueue.Count > 0)
                {
                    int v = verticesQueue.Dequeue();

                    foreach (int neighbor in GetNeighbors(v))
                    {
                        if (verticesVisited[neighbor])
                            continue;

                        verticesQueue.Enqueue(neighbor);
                        verticesVisited[neighbor] = true;
                        componentCount++;
                    }
                }

                return componentCount;
            }


            public object Clone()
            {
                var g = new Graph(_n);
                g._edges = _edges.Clone() as bool[,];
                return g;
            }

            public void AddEdges(ulong bits)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!bits.Bit(j))
                        continue;

                    for (int k = j + 1; k < n; k++)
                    {
                        if (bits.Bit(k))
                            AddEdge(j, k);
                    }
                }
            }

            public int GetComponentsCount()
            {
                int components = 0;
                bool[] verticesVisited = new bool[n];
                while (BFS(verticesVisited) > 0)
                    components++;

                return components;
            }
        }

        private static int n = 64;

        static void Main(string[] args)
        {
            int edgesSets = int.Parse(Console.ReadLine().Trim());


            Graph g = new Graph(n);
            ulong[] numbers = Array.ConvertAll(Console.ReadLine().Trim().Split(' '), ulong.Parse);
            Console.WriteLine(Recursion(numbers, 0, g));
        }

        static int Recursion(ulong[] numbers, int startIndex, Graph g)
        {
            if (startIndex == numbers.Length)
                return g.GetComponentsCount();

            var gClone = g.Clone() as Graph;
            gClone.AddEdges(numbers[startIndex++]);

            return Recursion(numbers, startIndex, gClone) + Recursion(numbers, startIndex, g);
        }
    }

    static class UlongExtender
    {
        public static bool Bit(this ulong number, int position)
        {
            return ((number >> position) & 1) == 1;
        }
    }
}
