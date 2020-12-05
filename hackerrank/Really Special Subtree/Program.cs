using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Really_Special_Subtree
{
    class Program
    {
        struct Edge : IComparable
        {
            public int V1;
            public int V2;
            public int Weight;

            public Edge(int v1, int v2, int weight)
            {
                V1 = v1;
                V2 = v2;
                Weight = weight;
            }

            public int CompareTo(object obj)
            {
                if (!(obj is Edge))
                    return 1;

                var e = (Edge) obj;
                if (e.Weight == Weight)
                    return V1 + V2 - (e.V1 + e.V2);

                return Weight - e.Weight;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Edge))
                    return false;

                var e = (Edge) obj;
                return e.V1 == V1 && e.V2 == V2 && e.Weight == Weight;
            }

            public override int GetHashCode()
            {
                var hashCode = 894064202;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + V1.GetHashCode();
                hashCode = hashCode * -1521134295 + V2.GetHashCode();
                hashCode = hashCode * -1521134295 + Weight.GetHashCode();
                return hashCode;
            }
        }

        class UnionFind
        {
            private readonly int[] _parent; // Non-negative -> parent index. Negative -> cluster number
            
            public UnionFind(int n)
            {
                _parent = new int[n];

                for (var i = 0; i < n; i++)
                    _parent[i] = - (i + 1);
            }

            public void Union(int v, int u)
            {
                var root1 = FindRoot(v);
                var root2 = FindRoot(u);

                if (root1 == root2)
                    return;

                _parent[root1] = root2;
            }

            public int Find(int v)
            {
                var rootIndex = FindRoot(v);
                return _parent[rootIndex];
            }

            private int FindRoot(int v)
            {
                if (_parent[v] < 0)
                    return v;

                int rootIndex = _parent[v];
                while (_parent[rootIndex] >= 0)
                    rootIndex = _parent[rootIndex];

                _parent[v] = rootIndex;
                return rootIndex;
            }
        }

        class PriorityQueue<T> where T : IComparable
        {
            private readonly List<T> _queue = new List<T>();
            public int Count => _queue.Count;
            public bool Empty => Count == 0;

            public void Enqueue(T element)
            {
                int index;
                for (index = 0; index < _queue.Count; index++)
                {
                    if (element.CompareTo(_queue[index]) < 0)
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

            private void EnsureQueueNotEmpty()
            {
                if (!Empty)
                    return;

                throw new InvalidOperationException("Queue is empty");
            }
        }

        static void Main(string[] args)
        {
            int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int verticesCount = numbers[0],
                edgesCount = numbers[1];

            if (verticesCount == 0)
            {
                Console.WriteLine(0);
                return;
            }

            var edgesQueue = new PriorityQueue<Edge>();

            for (int i = 0; i < edgesCount; i++)
            {
                numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Edge e = new Edge(numbers[0] - 1, numbers[1] - 1, numbers[2]);
                edgesQueue.Enqueue(e);
            }

            var initialEdge = edgesQueue.Dequeue();
            int treeWeight = initialEdge.Weight;

            int nodesInTree = 2;

            var unionFind = new UnionFind(verticesCount);
            unionFind.Union(initialEdge.V1, initialEdge.V2);

            while (nodesInTree < verticesCount && !edgesQueue.Empty)
            {
                Edge e = edgesQueue.Dequeue();
                if (unionFind.Find(e.V1) == unionFind.Find(e.V2))
                    continue;

                unionFind.Union(e.V1, e.V2);
                treeWeight += e.Weight;
                nodesInTree++;
            }

            if (nodesInTree < verticesCount)
                Console.WriteLine("Tree is not complete");

            Console.WriteLine(treeWeight);
        }
    }
}
