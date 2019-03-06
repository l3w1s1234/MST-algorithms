using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Graph
    {
        private Dictionary<string, Vertex> nodes ;

        public Graph()
        {
            nodes = new Dictionary<string, Vertex> { };
        }

        //add node to graph
        public void addNode (string name)
        {
           Vertex node = new Vertex(name);
            nodes.Add(name, node);
        }

        //get thew vertex when given a key
        public Vertex GetVertex(String id)
        {
            return nodes[id];
        }

        //get a list of all nodes in 
        public List<Vertex> GetVertices()
        {
            var vertices = new List<Vertex>();

            foreach(KeyValuePair<string,Vertex> entry in nodes)
            {
                vertices.Add(entry.Value);
            }
            return vertices;
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

        //used to add another graphs nodes to the graphs
        public void mergeGraph(Graph graph)
        {
            foreach (KeyValuePair<string, Vertex> n in graph.nodes)
            {
                if(!nodes.ContainsKey(n.Key))
                {
                    nodes.Add(n.Key, n.Value);
                }
            }

        }

    }
}
