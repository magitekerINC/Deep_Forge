using System;
using System.Collections.Generic;
using UnityEngine;


namespace DeepForge.Utility
{
    public class Node<T>
    {
        T data;

        public Node(T _data)
        {
            data = _data;
        }
    }  

    public class Graph<T>
    {



        public class SpanTree
        {
            private class Edge
            {
                Node<T> from;
                Node<T> to;
            
                public Edge(Node<T> _from, Node<T> _to)
                {
                    from = _from;
                    to = _to;
                }
            
            }

            private Graph<T> graph;
            private List<Node<T>> tree = new List<Node<T>>();

            private int edgeCount = 0;
            public int Count
            {
                get { return edgeCount; }
            }

            public SpanTree(Graph<T> _data)
            {
                graph = _data;
            }

            private bool Construct(Node<T> start)
            {
                bool success = false;

                return success;
            }
        }

        private Dictionary<Node<T>, List<Node<T>>> edges = new Dictionary<Node<T>, List<Node<T>>>();
        private int edgeCount = 0;
        public int Edges
        {
            get { return edgeCount; }
        }
        private int nodeCount = 0;
        public int Nodes
        {
            get
            {
                return nodeCount;
            }
        }

        public Graph()
        {

        }

        public bool Contains(Node<T> n)
        {
            return edges.ContainsKey(n);
        }

        public void AddAll(params Node<T>[] nodes)
        {
            foreach (Node<T> n in nodes)
                Add(n);
        }

        public void Add(Node<T> nNode)
        {
            if (Contains(nNode))
                return;

            edges.Add(nNode, new List<Node<T>>());
            nodeCount++;

        }

        public void AddEdge(Node<T> from, Node<T> to)
        {
            if (from == to || !Contains(from) ||
                !Contains(to))
                return;

            edges[from].Add(to);
            edges[to].Add(from);

            edgeCount += 2;
        }

        public void RemoveEdge(Node<T> from, Node<T> to)
        {
            if (from == to || edgeCount == 0 ||
                !Contains(from) ||
                !Contains(to) ||
                !edges[from].Contains(to))
                return;

            edges[from].Remove(to);
            edges[to].Remove(from);
            edgeCount -= 2;
        }

        public List<Node<T>> GetEdges(Node<T> from)
        {
            if (edgeCount == 0 || !Contains(from))
                return new List<Node<T>>();

            return edges[from];
        }

        public void Remove(Node<T> n)
        {
            if (!Contains(n))
                return;

            edgeCount -= edges[n].Count;
            foreach (Node<T> curr in edges.Keys)
            {
                if (edges[curr].Contains(n))
                {
                    edges[curr].Remove(n);
                    edgeCount--;
                }
            }

            edges.Remove(n);
            nodeCount--;
        }


    }

  
}
