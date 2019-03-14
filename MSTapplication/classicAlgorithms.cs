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
            components[0].edges = components[0].GetEdges().Count;
           
            return components[0];
        }


        //execute the boruvka algorithm
        public void djikstra(ref Graph g)
        {

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
        public void Astar(ref Graph g)
        {

        }
    }
}
