using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simpleGraph;

namespace ClassicAlgorithms
{
    class distance
        {
        public Vertex prevVertex;
        public float dist;



        public distance(float d, Vertex v)
            {
            dist = d;
            prevVertex = v;
            }
        }





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

            List<Vertex> visitedNodes = new List<Vertex>();//contains visted nodes and their edge id
            
            List<Vertex> unvisitedNodes = new List<Vertex>();

            Dictionary<Vertex, distance> distances = new Dictionary<Vertex, distance>();


            bool reachedGoal = false;

            
            //check that the source and destination exists
            if(g.hasVertex(src) && g.hasVertex(dest))
            {
                //add the source node has been visited and add its distance
                distances.Add(g.GetVertex(src), new distance(0,g.GetVertex(src)));
                
                

                //set all nodes distances and mark unvisted
                foreach(Vertex v in g.GetVertices())
                {
                    if (v.data != src)
                    {
                        distances.Add(v, new distance(float.MaxValue, null));//set unkown distance to infinity
                    }
                    unvisitedNodes.Add(v);//add to a list of unvisited nodes
                }
                
                var prevNode = g.GetVertex(src);
                Edge minEdge = null;
                var currentNode = g.GetVertex(src);

                //while goal hasnt been reached try and get a suitable goal
                while (!reachedGoal)
                {

                    if (currentNode == g.GetVertex(dest)) { reachedGoal = true; }

                    //find lowest distance and set the current distance that and recored the parent node
                    foreach (Edge e in currentNode.neighbours)
                    {
                        //check that set distances
                        if(e.node1 != currentNode.data)
                        {
                            if (e.weight + distances[currentNode].dist < distances[g.GetVertex(e.node1)].dist && !visitedNodes.Contains(g.GetVertex(e.node2)))
                            {
                                distances[g.GetVertex(e.node1)].dist = e.weight + distances[currentNode].dist;
                                distances[g.GetVertex(e.node1)].prevVertex = currentNode;
                            }
                        }
                        else if(e.node2 != currentNode.data)
                        {
                            if(e.weight + distances[currentNode].dist < distances[g.GetVertex(e.node2)].dist && !visitedNodes.Contains(g.GetVertex(e.node2)))
                            {
                                distances[g.GetVertex(e.node2)].dist = e.weight + distances[currentNode].dist;
                                distances[g.GetVertex(e.node2)].prevVertex = currentNode;
                            }
                        }

                        
                       
                    }

                    visitedNodes.Add(currentNode);
                    unvisitedNodes.Remove(currentNode);
                    Vertex lowestDist = null;

                    //select next node that hasnt been visted that has lowest diastance
                    foreach(KeyValuePair<Vertex,distance>kvp in distances)
                    {
                        if(lowestDist == null && !visitedNodes.Contains(kvp.Key))
                        {
                            lowestDist = kvp.Key;
                        }

                        if(!visitedNodes.Contains(kvp.Key))
                        {
                            if(distances[kvp.Key].dist < distances[lowestDist].dist)
                            {
                                lowestDist = kvp.Key;
                            }
                        }
                       
                    }

                    currentNode = lowestDist;
                }
            }

            //build mst
           foreach(Vertex v in visitedNodes)
            {
                foreach(Edge e in v.neighbours)
                {
                     if(e.node1 != v.data)
                    {
                        if(e.node1 == distances[v].prevVertex.data)
                        {
                            if (!mst.hasVertex(e.node1)) { mst.addNode(e.node1); }
                            if (!mst.hasVertex(e.node2)) { mst.addNode(e.node2); }
                            if (!mst.hasEdge(e)) { mst.addEdge(e.weight,e.node1,e.node2,e.data); }

                        }
                    }
                     else if(e.node2 != v.data)
                    {
                        if (e.node2 == distances[v].prevVertex.data)
                        {
                            if (!mst.hasVertex(e.node1)) { mst.addNode(e.node1); }
                            if (!mst.hasVertex(e.node2)) { mst.addNode(e.node2); }
                            if (!mst.hasEdge(e)) { mst.addEdge(e.weight, e.node1, e.node2, e.data); }
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
