using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MSTapplication
{
    namespace simpleGraph
    {
        [Serializable()]
        class Graph : ISerializable
        {
            private Dictionary<string, Vertex> nodes;
            public int edges = 0;

            public Graph()
            {
                nodes = new Dictionary<string, Vertex> { };
            }

            //add node to graph
            public void addNode(string name)
            {
                Vertex node = new Vertex(name);
                nodes.Add(name, node);
            }

            //add a node by Vertex
            public void insertNode(Vertex v)
            {
                try
                {
                    nodes.Add(v.data, v);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Node with same key already exists");
                }

            }

            //get thew vertex when given a key
            public Vertex GetVertex(String id)
            {
                return nodes[id];
            }

            //check that graph contains a list of vertices
            public bool containsVertices(List<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    if (!nodes.ContainsKey(v.data)) return false;
                }
                return true;
            }

            //check that graph contains a list of vertices
            public bool containsAnyVertex(List<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    if (nodes.ContainsKey(v.data)) return true;
                }
                return false;
            }

            //returns a random vertex
            public Vertex getRandomVertex()
            {
                Random rand = new Random();

                var i = rand.Next(0, nodes.Count);

                return nodes.Values.ElementAt(i);
            }

            //get a list of all nodes in 
            public List<Vertex> GetVertices()
            {
                var vertices = new List<Vertex>();

                foreach (KeyValuePair<string, Vertex> entry in nodes)
                {
                    vertices.Add(entry.Value);
                }
                return vertices;
            }

            //get all the edges within the graph
            public List<Edge> GetEdges()
            {
                var edgeList = new List<Edge>();
                bool containsItem = false;
                //get all the edges
                foreach (KeyValuePair<string, Vertex> kvp in nodes)
                {
                    foreach (Edge e in kvp.Value.neighbours)
                    {
                        if (edgeList.Count == 0)
                        {
                            edgeList.Add(e);
                        }
                        else
                        {
                            foreach (Edge e2 in edgeList)
                            {
                                if (e2.data == e.data)
                                {
                                    containsItem = true;
                                }
                            }
                            if (containsItem == false)
                            {
                                edgeList.Add(e);
                            }
                        }
                        containsItem = false;
                    }

                }

                return edgeList;
            }



            //add edge to nodes by using the name of both nodes
            public void addEdge(float weight, String n1ID, String n2ID, String id)
            {
                var n1 = nodes[n1ID];
                var n2 = nodes[n2ID];
                Edge edge = new Edge(n1.data, n2.data, weight, id);
                n1.addNeighbour(edge);
                n2.addNeighbour(edge);
                edges++;
            }

            //get all the ids for the edges the node is connected to
            public List<String> getEdgeIDs(String nodeID)
            {
                var node = nodes[nodeID];
                List<string> edgeIDs = new List<string>();

                foreach (Edge e in node.neighbours)
                {
                    edgeIDs.Add(e.data);
                }

                return edgeIDs;
            }


            //check that vertex exists
            public bool hasVertex(string nodeID)
            {
                if (nodes.ContainsKey(nodeID))
                {
                    return true;
                }
                return false;
            }

            //remove Vertex
            public void removeVertex(string nID)
            {
                if (nodes.ContainsKey(nID)) nodes.Remove(nID);
            }


            //remove all edges in the called vertex
            public void removeEdges(String nodeID)
            {
                var node = nodes[nodeID];


                foreach (Edge e in node.neighbours)
                {
                    if (e.node1 == node.data)
                    {
                        nodes[e.node2].removeEdge(e.data);
                        edges--;
                    }
                    else
                    {
                        nodes[e.node1].removeEdge(e.data);
                        edges--;
                    }
                }
                node.neighbours.Clear();

            }

            public bool hasEdge(Edge e)
            {
                if (nodes.ContainsKey(e.node1))
                {
                    if (nodes[e.node1].hasEdge(e.data)) return true;
                }


                return false;
            }

            //remove called edge in the called vertex
            public void removeEdge(String nodeID, String edgeID)
            {
                var node = nodes[nodeID];

                node.removeEdge(edgeID);
            }

            //used to add another graphs nodes to the graph
            public void mergeGraph(Graph graph)
            {
                foreach (KeyValuePair<string, Vertex> n in graph.nodes)
                {
                    if (!nodes.ContainsKey(n.Key))
                    {
                        nodes.Add(n.Key, n.Value);
                    }
                    else if (nodes.ContainsKey(n.Key))
                    {
                        //add all the edges relatedto that node
                        foreach (Edge e in n.Value.neighbours)
                        {
                            if (!nodes[n.Value.data].hasEdge(e.data))
                            {
                                if (!nodes.ContainsKey(e.node1)) { addNode(e.node1); }
                                if (!nodes.ContainsKey(e.node2)) { addNode(e.node2); }
                                this.addEdge(e.weight, e.node1, e.node2, e.data);

                            }
                        }
                    }
                }

            }

            //return the maximum weight of graph
            public float getGraphWeight()
            {
                float weight = 0;
                foreach (Edge e in this.GetEdges())
                {
                    weight += e.weight;
                }


                return weight;
            }

            //for serialization
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Nodes", nodes);
                info.AddValue("Edges", edges);
            }

            public Graph(SerializationInfo info, StreamingContext context)
            {
                nodes = (Dictionary<string, Vertex>)info.GetValue("Nodes", typeof(Dictionary<string, Vertex>));
                edges = (int)info.GetValue("Edges", typeof(int));
            }
        }
    }
}

