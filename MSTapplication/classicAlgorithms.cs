using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simpleGraph;

namespace ClassicAlgorithms
{
    class classicAlgorithms
    {
      
        public classicAlgorithms()
        {

        }

        //execute the boruvka algorithm and returns mst
        public Graph boruvka(ref Graph g)
        {
            List<Graph> components = new List<Graph>();

            //set up each component with single vertices
            foreach(Vertex v in g.GetVertices())
            {
                var graph = new Graph();
                graph.addNode(v.data);
                components.Add(graph);
            }


            //loop through while there are more than 1 component
            while (components.Count > 1)
            {
                //loop through each component to fine the minimum edge
                foreach (Graph graph in components)
                {
                    Edge minEdge = null;

                    //check each vertex in componenet
                    foreach (Vertex v in graph.GetVertices())
                    {
                        //try and find the minimum edge not connected to the component
                        foreach (Edge edge in g.GetVertex(v.data).neighbours)
                        {
                            if (minEdge == null) minEdge = edge;

                            //check that the edge isnt already contained within the graph and compare weight
                            if (v.data == edge.node1)
                            {
                                if (!v.hasEdge(edge.data))
                                {
                                    if (minEdge.weight > edge.weight)
                                    {
                                        minEdge = edge;
                                    }


                                }
                            }
                            else if (v.data == edge.node2)
                            {
                                if (!v.hasEdge(edge.data))
                                {
                                    if (minEdge.weight > edge.weight)
                                    {
                                        minEdge = edge;
                                    }
                                }
                            }
                        }
                       
                    }

                    //add edge and nodes to components
                    if(minEdge != null)
                    {
                        if (graph.hasVertex(minEdge.node1))
                        {
                            if (!graph.hasVertex(minEdge.node2))
                            {
                                graph.addNode(minEdge.node2);
                            }
                            if(!graph.GetVertex(minEdge.node1).hasEdge(minEdge.data))
                                {
                                graph.addEdge(minEdge.weight,minEdge.node1,minEdge.node2,minEdge.data);
                                }
                            
                        }
                        else if (graph.hasVertex(minEdge.node2))
                        {
                            if (!graph.hasVertex(minEdge.node1))
                            {
                                graph.addNode(minEdge.node1);
                            }
                            if(!graph.GetVertex(minEdge.node2).hasEdge(minEdge.data))
                                {
                                graph.addEdge(minEdge.weight,minEdge.node1,minEdge.node2,minEdge.data);
                                }
                            
                        }
                    }
                    
                    

                }


                
                //merge components if there are 2 left
                if (components.Count == 2)
                {
                    components[0].mergeGraph(components[1]);
                }
                int prevComp = components.Count - 1;
                //merge components that are connected to each other
                while (prevComp > 0)
                {
                    
                    //loop down components
                    for (int i = components.Count -2 ; i >= 0; i--)
                    {

                        if (i != prevComp)
                        {
                            if (components[i].containsVertices(components[prevComp].GetVertices()))
                            {
                                components[i].mergeGraph(components[prevComp]);
                                components.RemoveAt(prevComp);
                                prevComp--;
                            }
                        }
                       
                        
                    }

                     prevComp--;

                }
            }

            //check that there are no more than 2 edges connected to 1 node
            foreach (Vertex n in components[0].GetVertices())
            {
                if (n.neighbours.Count > 2)
                {
                    Edge maxEdge = null;
                    while(n.neighbours.Count > 2)
                    {
                        foreach(Edge e in n.neighbours)
                        {
                            if (maxEdge == null) maxEdge = e;
                            else if(maxEdge.weight < e.weight)
                            {
                                maxEdge = e;
                            }

                        }
                        n.neighbours.Remove(maxEdge);
                    }

                }
            }
            components[0].edges = components[0].GetEdges().Count;
           
            return components[0];
        }


        //execute the boruvka algorithm
        public Graph dijkstra(string src, string dest, ref Graph g)
        {
            Graph mst = new Graph();

            Dictionary<Vertex,bool> visitedNodes = new Dictionary<Vertex,bool>();
            List<Edge> viableEdges = new List<Edge>();
            Dictionary<Vertex,float> distances = new Dictionary<Vertex, float>();


            bool reachedGoal = false;

            
            //check that the source and destination exists
            if(g.hasVertex(src) && g.hasVertex(dest))
            {
                //add the source node has been visited and add its distance
                visitedNodes.Add(g.GetVertex(src), true);
                distances.Add(g.GetVertex(src), 0);
                var currentNode = g.GetVertex(src);
                var prevNode = g.GetVertex(src);

                //set all nodes distances and mark unvisted
                foreach(Vertex v in g.GetVertices())
                {
                    if (v.data != src)
                    {
                        distances.Add(v, float.MaxValue);
                        visitedNodes.Add(v, false);
                    }
                    
                    
                }


                

                //while goal hasnt been reached try and get a suitable goal
                while(!reachedGoal)
                {
                    //check that current node is goal
                    if (currentNode.data == dest)
                    {
                        reachedGoal = true;
                    }

                    //check neighbours of current node
                    foreach (Edge e in currentNode.neighbours)
                    {
                        //get weights and assign the tentative distance
                        if(currentNode.data == e.node1)
                        {
                            if (e.weight < distances[g.GetVertex(e.node2)] && visitedNodes[g.GetVertex(e.node2)] == false)
                            {
                                distances[g.GetVertex(e.node2)] = e.weight + distances[currentNode];
                            }

                        }
                        else
                        {

                            if (e.weight < distances[g.GetVertex(e.node1)] && visitedNodes[g.GetVertex(e.node1)] == false)
                            {
                                distances[g.GetVertex(e.node1)] = e.weight + distances[currentNode];
                            }
                        }


                    }

                    //change current node to visited
                    if (visitedNodes[currentNode] == false)
                    {
                        visitedNodes[currentNode] = true;
                    }

                    prevNode = currentNode;

                    //select next node to check
                    foreach (KeyValuePair<Vertex,float>kvp in distances)
                    {
                        if(kvp.Value != float.MaxValue)//if it is not equal to infinity
                        {
                            if(kvp.Key != currentNode && visitedNodes[kvp.Key] == false && kvp.Value<distances[currentNode])
                            {
                                currentNode = kvp.Key;
                            }
                            else if(visitedNodes[currentNode] == true)
                            {
                                currentNode = kvp.Key;
                            }
                        }
                    }
                }
            }

            //build mst
            foreach(KeyValuePair<Vertex,bool> kvp in visitedNodes)
            {
                if(kvp.Value == true)
                {
                    if (!mst.hasVertex(kvp.Key.data)) mst.addNode(kvp.Key.data);
                    //add edges and nodes
                    foreach (Edge e in kvp.Key.neighbours)
                    {
                        

                        if(!mst.hasEdge(e))
                        {
                            if(e.node1 == kvp.Key.data)
                            {
                                if(visitedNodes[g.GetVertex(e.node2)] == true)
                                {
                                    if(!mst.hasVertex(e.node2)) mst.addNode(e.node2);
                                    mst.addEdge(e.weight,e.node1,e.node2,e.data);
                                }
                            }
                            else if(e.node2 == kvp.Key.data)
                            {
                                if (visitedNodes[g.GetVertex(e.node1)] == true)
                                {
                                    if (!mst.hasVertex(e.node1)) mst.addNode(e.node1);
                                    mst.addEdge(e.weight, e.node1, e.node2, e.data);
                                }
                            }
                        }
                    }
                }
            }


            return mst;
        }

        //execute the boruvka algorithm
        public void kruskal(ref Graph g)
        {

        }

        //execute the boruvka algorithm
        public void prim(ref Graph g)
        {

        }

        //execute the boruvka algorithm
        public void aStar(ref Graph g)
        {

        }
    }
}
