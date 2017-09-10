using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Even_Tree
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = ReadNumbersFromConsole();
            int nodes = numbers[0];
            int edgesCount = numbers[1];

            bool[,] edges = new bool[nodes, nodes];

            for (int i = 0; i < edgesCount; i++)
            {
                numbers = ReadNumbersFromConsole();
                int vFrom = numbers[0] - 1,
                    vTo = numbers[1] - 1;
                edges[vFrom, vTo] = true;
                edges[vTo, vFrom] = true;
            }

            Console.WriteLine(DFS(nodes, edgesCount, edges));
        }

        static int[] ReadNumbersFromConsole()
        {
            return Array.ConvertAll(Console.ReadLine().Trim().Split(' '), int.Parse);
        }

        static int DFS(int nodes, int edgesCount, bool[,] edges)
        {
            var edgesStack = new Stack<Tuple<int, int>>(edgesCount);
            var nodesStack = new Stack<int>(nodes);
            bool[] wasNodeVisited = new bool[nodes];
            int[] nodeValues = new int[nodes];

            edgesStack.Push(new Tuple<int, int>(0, 0));

            while (edgesStack.Count > 0)
            {
                var edge = edgesStack.Pop();
                int vTo = edge.Item2;

                nodesStack.Push(vTo);
                wasNodeVisited[vTo] = true;
                foreach (int neighbor in GetNeighbors(edges, vTo))
                {
                    if (!wasNodeVisited[neighbor])
                        edgesStack.Push(new Tuple<int, int>(vTo, neighbor));
                }

                while (nodesStack.Count > 0 && (edgesStack.Count == 0 || nodesStack.Peek() != edgesStack.Peek().Item1))
                {
                    int v = nodesStack.Pop();
                    int valueSum = 1;
                    foreach (int neighbor in GetNeighbors(edges, v))
                        valueSum += nodeValues[neighbor];

                    nodeValues[v] = valueSum;
                }
            }

            return nodeValues.Where(x => x % 2 == 0).Count() - 1;
        }

        static IEnumerable<int> GetNeighbors(bool[,] edges, int v)
        {
            int n = edges.GetLength(0);
            for (int otherV = 0; otherV < n; otherV++)
            {
                if (edges[v, otherV])
                    yield return otherV;
            }
        }
}
}
