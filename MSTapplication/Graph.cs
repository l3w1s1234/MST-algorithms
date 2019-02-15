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

        //add node to graph
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

        //add edge to nodes by using the name of both nodes
        public void addEdge(float weight, String n1ID, String n2ID, String id)
        {
            var n1 = nodes[n1ID];
            var n2 = nodes[n2ID];
            Edge edge = new Edge(ref n1, ref n2, weight,id);
            n1.addNeighbour(ref edge);
            n2.addNeighbour(ref edge);
        }

    }
}
