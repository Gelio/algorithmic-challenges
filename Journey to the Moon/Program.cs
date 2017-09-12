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
            public int Count = 1;
            public int Parent = -1;

            public Node(int v)
            {
                this.v = v;
            }
        }

        private readonly Node[] _nodes;
        private readonly bool[] _isRoot;

        public UnionFind(int n)
        {
            _nodes = new Node[n];
            _isRoot = new bool[n];
            for (int i = 0; i < n; i++)
            {
                _nodes[i] = new Node(i);
                _isRoot[i] = true;
            }
        }

        public int Find(int v)
        {
            var node = _nodes[v];

            if (node.Parent == -1)
                return node.v;

            int rootV = Find(node.Parent);
            node.Parent = rootV;

            return rootV;
        }

        public void Union(int v1, int v2)
        {
            int v1Root = Find(v1);
            int v2Root = Find(v2);

            if (v1Root == v2Root)
                return;

            _nodes[v1Root].Parent = v2Root;
            _nodes[v2Root].Count += _nodes[v1Root].Count;
            _isRoot[v1Root] = false;
        }

        public IEnumerable<int> GetRoots()
        {
            for (int v = 0; v < _isRoot.Length; v++)
            {
                if (_isRoot[v])
                    yield return v;
            }
        }

        public int GetCount(int v)
        {
            return _nodes[v].Count;
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

            List<int> countrySizes = new List<int>();
            foreach (int root in unionFind.GetRoots())
                countrySizes.Add(unionFind.GetCount(root));

            long possiblePairs = 0;

            for (int i = 0; i < countrySizes.Count; i++)
            {
                for (int j = i + 1; j < countrySizes.Count; j++)
                    possiblePairs += countrySizes[i] * countrySizes[j];
            }

            Console.WriteLine(possiblePairs);
        }
    }
}
