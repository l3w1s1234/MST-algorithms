using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace simpleGraph
{
    [Serializable()]
    class Graph : ISerializable
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
            Edge edge = new Edge(n1.data, n2.data, weight,id);
            n1.addNeighbour(ref edge);
            n2.addNeighbour(ref edge);
        }

        //get all the ids for the edges the node is connected to
        public List<String> getEdgeIDs(String nodeID)
        {
            var node = nodes[nodeID];
            List<string> edgeIDs = new List<string>();

            foreach(Edge e in node.neighbours)
            {
                edgeIDs.Add(e.data);
            }

            return edgeIDs;
        }

        //remove all edges in the called vertex
        public void removeEdges(String nodeID)
        {
            var node = nodes[nodeID];


            foreach(Edge e in node.neighbours)
            {
                if(e.node1 == node.data)
                {
                  nodes[e.node2].removeEdge(e.data);
                }
                else
                {
                    nodes[e.node1].removeEdge(e.data);
                }
            }
            node.neighbours.Clear();

        }

        //remove called edge in the called vertex
        public void removeEdge(String nodeID, String edgeID)
        {
            var node = nodes[nodeID];

            node.removeEdge(edgeID);
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

        //for serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Nodes", nodes);
        }

        public Graph(SerializationInfo info, StreamingContext context)
        {
            nodes = (Dictionary<string, Vertex>)info.GetValue("Nodes", typeof(Dictionary<string, Vertex>));
        }
    }
}
