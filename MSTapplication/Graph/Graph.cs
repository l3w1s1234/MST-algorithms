using System;
using System.Text;
using System.Collections.Generic;

namespace SimpleGraph
{ 
    public class Graph
    {
        LinkedList<Vertex<T>> nodes;

        public Graph()
        {

        }

        public void addNode(Vertex<T> node)
        {
            nodes.AddFirst(node);
        }

        public void addEdge()
        {

        }


    }
}
