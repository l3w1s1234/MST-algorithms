using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Vertex
    {
        private String data { get; set; }
        private LinkedList<Edge> neighbours;
        float x {get; set;}
        float y { get; set; }

        public Vertex(String d, double x, double y)
        {
            d = data;
        }

        //add a neighbour to list
        public void addNeighbour(ref Edge edge)
        {
            neighbours.AddFirst(edge);
        }

        //get a list of the neighbours  connected tyo this node
        public LinkedList<Edge> getNeighbour()
        {
            return neighbours;
        }
    }
}
