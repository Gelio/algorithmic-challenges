using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Journey_to_the_Moon
{
    class UnionFind
    {
        class Node
        {
            public int v;
            public Node parent;

            public Node(int v, Node parent = null)
            {
                this.v = v;
                this.parent = parent;
            }
        }

        private Node[] nodes;

        public UnionFind(int n)
        {
            nodes = new Node[n];
            for (int i = 0; i < n; i++)
                nodes[i] = new Node(i);
        }

        public int Find(int v)
        {
            var node = nodes[v];

            while (node.parent != null)
                node = node.parent;

            return node.v;
        }

        public void Union(int v1, int v2)
        {
            int v1Root = Find(v1);
            nodes[v1Root].parent = nodes[v2];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int peopleCount = numbers[0],
                pairsCount = numbers[1];
            
            var unionFind = new UnionFind(peopleCount);

            for (int i = 0; i < pairsCount; i++)
            {
                numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                int person1 = numbers[0],
                    person2 = numbers[1];

                unionFind.Union(person1, person2);
            }


            int[] countrySizes = new int[peopleCount];

            for (int i = 0; i < peopleCount; i++)
            {
                int personCountry = unionFind.Find(i);
                countrySizes[personCountry]++;
            }

            long possiblePairs = 0;

            for (int i = 0; i < peopleCount; i++)
            {
                for (int j = i + 1; j < peopleCount; j++)
                    possiblePairs += countrySizes[i] * countrySizes[j];
            }

            Console.WriteLine(possiblePairs);
        }
    }
}
