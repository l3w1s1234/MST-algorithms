using System;

namespace SimpleGraph
{
    public class Edge
    {
        private Vertex<T> node1;
        private Vertex<T> node2;
        private int weight;

        public Edge(ref Vertex<T> n1, ref Vertex<T> n2, ref int w)
        {
            node1 = n1;
            node2 = n2;
            weight = w;
        }
        

    }
}
