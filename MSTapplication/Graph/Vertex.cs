using System;
using System.Text;
using System.Collections.Generic;

namespace SimpleGraph
{
    public class Vertex<T>
    {
        private T data;
        private LinkedList<Edge> neighbours;

        public Vertex(T d)
        {
            d = data;
        }

        //add a neighbour to list
        public void addNeighbour(ref Edge edge)
        {
            neighbours.AddFirst(edge);
        }
    }
}
