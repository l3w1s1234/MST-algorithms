using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Graph
    {
        private Dictionary<String, Vertex> nodes ;

        public Graph()
        {
            nodes = new Dictionary<string, Vertex> { };
        }

        public void addNode (string name,double x, double y)
        {
           Vertex node = new Vertex(name,x,y);
            nodes.Add(name, node);
        }

        //get thew vertex when given a key
        public Vertex GetVertex(String id)
        {
            return nodes[id];
        }


        public void addEdge(string name, double x1, double y1, double x2, double y2)
        {

        }

    }
}
