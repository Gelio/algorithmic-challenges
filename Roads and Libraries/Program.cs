using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Roads_and_Libraries
{
    class Program
    {
        static void CountLibrariesAndRoads(int verticesCount, List<int>[] graph, out int libraries, out int roads)
        {
            libraries = roads = 0;
            bool[] verticesVisited = new bool[verticesCount];
            int verticesInComponent,
                startSearchingIndex = 0;

            while ((verticesInComponent = BFS(graph, verticesVisited, ref startSearchingIndex)) > 0)
            {
                libraries++;
                roads += verticesInComponent - 1;
            }
        }

        static int BFS(List<int>[] graph, bool[] vertexVisitedOrEnqueued, ref int startSearchingIndex)
        {
            int startingVertex = -1;
            for (int v = startSearchingIndex; v < vertexVisitedOrEnqueued.Length; v++)
            {
                if (!vertexVisitedOrEnqueued[v])
                {
                    startingVertex = v;
                    break;
                }
            }

            if (startingVertex == -1)
                return 0;

            startSearchingIndex = startingVertex + 1;

            int verticesVisitedCount = 0;
            var vertexQueue = new Queue<int>();
            vertexQueue.Enqueue(startingVertex);
            vertexVisitedOrEnqueued[startingVertex] = true;

            while (vertexQueue.Count > 0)
            {
                int v = vertexQueue.Dequeue();
                verticesVisitedCount++;

                foreach (int neighbor in graph[v])
                {
                    if (vertexVisitedOrEnqueued[neighbor])
                        continue;

                    vertexQueue.Enqueue(neighbor);
                    vertexVisitedOrEnqueued[neighbor] = true;
                }
            }

            return verticesVisitedCount;
        }

        static void Main(string[] args)
        {
            int queries = Convert.ToInt32(Console.ReadLine());
            for (int a0 = 0; a0 < queries; a0++)
            {
                string[] tokens_n = Console.ReadLine().Split(' ');
                int citiesCount = Convert.ToInt32(tokens_n[0]);
                int roadsCount = Convert.ToInt32(tokens_n[1]);
                long libraryCost = Convert.ToInt64(tokens_n[2]);
                long roadCost = Convert.ToInt64(tokens_n[3]);

                List<int>[] graph = new List<int>[citiesCount];
                for (int i=0; i < citiesCount; i++)
                    graph[i] = new List<int>();
    

                for (int a1 = 0; a1 < roadsCount; a1++)
                {
                    string[] tokens_city_1 = Console.ReadLine().Split(' ');
                    int city1 = Convert.ToInt32(tokens_city_1[0]) - 1;
                    int city2 = Convert.ToInt32(tokens_city_1[1]) - 1;

                    graph[city1].Add(city2);
                    graph[city2].Add(city1);
                }

                if (libraryCost <= roadCost)
                {
                    Console.WriteLine(citiesCount * libraryCost);
                    continue;
                }


                int requiredLibraries, requiredRoads;
                CountLibrariesAndRoads(citiesCount, graph, out requiredLibraries, out requiredRoads);
                long cost = requiredLibraries * libraryCost + requiredRoads * roadCost;
                Console.WriteLine(cost);
            }
        }
    }
}
