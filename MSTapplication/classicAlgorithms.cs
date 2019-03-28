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
            List<KeyValuePair<int,int>> mergeIndex = new List<KeyValuePair<int, int>>();

            //set up each component with single vertices
            foreach(Vertex v in g.GetVertices())
            {
                var graph = new Graph();
                graph.addNode(v.data);
                components.Add(graph);
            }

            Edge minEdge;

            //loop through while there are more than 1 component
            while (components.Count > 1)
            {

             
                //loop through each component
                foreach(Graph comp in components)
                {
                    minEdge = null;
                        //find the smallest edge
                        foreach (Vertex v in comp.GetVertices())
                        {
                            foreach (Edge e in g.GetVertex(v.data).neighbours)
                            {
                                if (minEdge == null) minEdge = e;
                                
                                if(v.data != e.node1)
                                {

                                    if (minEdge.weight > e.weight && !comp.hasVertex(e.node1))
                                    {
                                        minEdge = e;
                                    }
                                }
                                else
                                {
                                    if (minEdge.weight > e.weight && !comp.hasVertex(e.node2) )
                                    {
                                        minEdge = e;
                                    }
                                }
                            }

                        }   

                        //add edge to component
                        if(minEdge != null)
                        {
                            if (!comp.hasVertex(minEdge.node1)) { comp.addNode(minEdge.node1); }
                            if (!comp.hasVertex(minEdge.node2)) { comp.addNode(minEdge.node2); }
                            if (!comp.hasEdge(minEdge))
                            {
                                comp.addEdge(minEdge.weight, minEdge.node1, minEdge.node2, minEdge.data);
                            }

                            if(components.IndexOf(comp) > 0)
                        {
                            for (int k = 0; k < components.IndexOf(comp); k++)
                            {
                                if (components[k].hasVertex(minEdge.node1) && components[k].hasVertex(minEdge.node2))
                                {
                                    mergeIndex.Add(new KeyValuePair<int, int>(components.IndexOf(comp),k));
                                    break;
                                }
                                
                            }
                        }
                            
                        }
                   
                }


               


                //remove components that are connected to other components
                if(mergeIndex.Count > 0)
                {
                    for(int k = mergeIndex.Count - 1; k>=0; k--)
                    {
                        components[mergeIndex[k].Value].mergeGraph(components[mergeIndex[k].Key]);
                    }
                    for (int k = mergeIndex.Count - 1; k >= 0; k--)
                    {
                        components.RemoveAt(mergeIndex[k].Key);
                    }
                        mergeIndex.Clear();
                }


                //merge the last two components
                if(components.Count == 2)
                {
                    Edge min = null;
                    //select the component with least edges
                    if(components[0].edges > components[1].edges)
                    {
                        //find the smallest edge
                        foreach (Vertex v in components[1].GetVertices())
                        {
                            foreach (Edge e in g.GetVertex(v.data).neighbours)
                            {
                                if (!components[0].hasEdge(e) && !components[1].hasEdge(e))
                                {
                                    if (min == null)
                                    {
                                        min = e;
                                    }
                                    else if (min.weight > e.weight) min = e;
                                }
                            }
                        }
                        
                        components[0].mergeGraph(components[1]);
                        components[0].addEdge(min.weight, min.node1, min.node2, min.data);
                        components.RemoveAt(1);
                    }
                    else
                    {
                        //find the smallest edge
                        foreach (Vertex v in components[0].GetVertices())
                        {
                            foreach (Edge e in g.GetVertex(v.data).neighbours)
                            {

                                if (!components[1].hasEdge(e) && !components[0].hasEdge(e))
                                {
                                    if (min == null)
                                    {
                                        min = e;
                                    }
                                    else if (min.weight > e.weight) min = e;
                                }
                                
                            }
                        }
                       
                        components[1].mergeGraph(components[0]);
                        components[1].addEdge(min.weight, min.node1, min.node2, min.data);
                        components.RemoveAt(0);

                    }
                }

                

            }
            return components[0];
        }


        //execute the Dijkstra algorithm
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

        //execute dijkstra without a goal
        public Graph dijkstraNoDest(string src, ref Graph g)
        {
            Graph mst = new Graph();

            List<Vertex> visitedNodes = new List<Vertex>();//contains visted nodes and their edge id

            List<Vertex> unvisitedNodes = new List<Vertex>();

            Dictionary<Vertex, distance> distances = new Dictionary<Vertex, distance>();

            //check that the source and destination exists
            if (g.hasVertex(src))
            {
                //add the source node has been visited and add its distance
                distances.Add(g.GetVertex(src), new distance(0, g.GetVertex(src)));



                //set all nodes distances and mark unvisted
                foreach (Vertex v in g.GetVertices())
                {
                    if (v.data != src)
                    {
                        distances.Add(v, new distance(float.MaxValue, null));//set unkown distance to infinity
                    }
                    unvisitedNodes.Add(v);//add to a list of unvisited nodes
                }

                var prevNode = g.GetVertex(src);
                var currentNode = g.GetVertex(src);

                //while goal hasnt been reached try and get a suitable goal
                while (unvisitedNodes.Count > 0)
                {

                    

                    //find lowest distance and set the current distance that and recored the parent node
                    foreach (Edge e in currentNode.neighbours)
                    {
                        //check that set distances
                        if (e.node1 != currentNode.data)
                        {
                            if (e.weight + distances[currentNode].dist < distances[g.GetVertex(e.node1)].dist && !visitedNodes.Contains(g.GetVertex(e.node2)))
                            {
                                distances[g.GetVertex(e.node1)].dist = e.weight + distances[currentNode].dist;
                                distances[g.GetVertex(e.node1)].prevVertex = currentNode;
                            }
                        }
                        else if (e.node2 != currentNode.data)
                        {
                            if (e.weight + distances[currentNode].dist < distances[g.GetVertex(e.node2)].dist && !visitedNodes.Contains(g.GetVertex(e.node2)))
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
                    foreach (KeyValuePair<Vertex, distance> kvp in distances)
                    {
                        if (lowestDist == null && !visitedNodes.Contains(kvp.Key))
                        {
                            lowestDist = kvp.Key;
                        }

                        if (!visitedNodes.Contains(kvp.Key))
                        {
                            if (distances[kvp.Key].dist < distances[lowestDist].dist)
                            {
                                lowestDist = kvp.Key;
                            }
                        }

                    }

                    currentNode = lowestDist;
                }
            }

            //build mst
            foreach (Vertex v in visitedNodes)
            {
                foreach (Edge e in v.neighbours)
                {
                    if (e.node1 != v.data)
                    {
                        if (e.node1 == distances[v].prevVertex.data)
                        {
                            if (!mst.hasVertex(e.node1)) { mst.addNode(e.node1); }
                            if (!mst.hasVertex(e.node2)) { mst.addNode(e.node2); }
                            if (!mst.hasEdge(e)) { mst.addEdge(e.weight, e.node1, e.node2, e.data); }

                        }
                    }
                    else if (e.node2 != v.data)
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

        //execute the kruskal algorithm
        public Graph kruskal(ref Graph g)
        {
            Graph mst = null;
            List<Edge> edges = g.GetEdges();

            Edge[] orderedEdges = new Edge[edges.Count];
            Edge minEdge;
            int i = 0;
            //sort edges
            while (edges.Count != 0)
            {
                minEdge = null;
                foreach(Edge e in edges)
                {
                    if(minEdge == null)
                    {
                        minEdge = e;
                    }
                    else if(e.weight < minEdge.weight)
                    {
                        minEdge = e;
                    }
                }

                edges.Remove(minEdge);
                orderedEdges[i] = minEdge;
                i++;
            }
            
            //build mst
            for(int k =0; k<orderedEdges.Length-1;k++)
            {
                if(mst == null)
                {
                    mst = new Graph();
                    mst.addNode(orderedEdges[k].node1);
                    mst.addNode(orderedEdges[k].node2);
                    mst.addEdge(orderedEdges[k].weight,orderedEdges[k].node1, orderedEdges[k].node2, orderedEdges[k].data);
                }

               
               if (mst.hasVertex(orderedEdges[k].node2) && mst.hasVertex(orderedEdges[k].node1))
                {
                    if (mst.GetVertex(orderedEdges[k].node2).neighbours.Count <= 2 && mst.GetVertex(orderedEdges[k].node1).neighbours.Count <= 2)
                    {
                        if (!mst.hasVertex(orderedEdges[k].node1)) { mst.addNode(orderedEdges[k].node1); }
                        if (!mst.hasVertex(orderedEdges[k].node2)) { mst.addNode(orderedEdges[k].node2); }

                        if (!mst.hasEdge(orderedEdges[k])) {
                            mst.addEdge(orderedEdges[k].weight, orderedEdges[k].node1, orderedEdges[k].node2, orderedEdges[k].data);
                            
                        }
                       if(checkCycle(mst, mst.GetVertex(orderedEdges[k].node1))){
                            mst.removeEdge(orderedEdges[k].node1, orderedEdges[k].data);
                            mst.removeEdge(orderedEdges[k].node2, orderedEdges[k].data);
                        }
                    }
                }
                else if (!mst.hasVertex(orderedEdges[k].node1) || !mst.hasVertex(orderedEdges[k].node2))
                {
                    if (!mst.hasVertex(orderedEdges[k].node1)) { mst.addNode(orderedEdges[k].node1); }
                    if (!mst.hasVertex(orderedEdges[k].node2)) { mst.addNode(orderedEdges[k].node2); }

                    if (!mst.hasEdge(orderedEdges[k])) {
                        mst.addEdge(orderedEdges[k].weight, orderedEdges[k].node1, orderedEdges[k].node2, orderedEdges[k].data); }
                }

               
            }

           
            

            return mst;

        }

        //meant to be used with kruskal, but can be used with other algorithms to check for cycles
        public bool checkCycle(Graph g, Vertex start)
        {//check for a cycle
            Dictionary<Vertex, bool> visited = new Dictionary<Vertex, bool>();
            Queue<KeyValuePair<Vertex,Vertex>> uncheckedRoutes = new Queue<KeyValuePair<Vertex,Vertex>>();

            bool noCycle = false;
            foreach (Vertex n in g.GetVertices())
            {
                visited.Add(n, false);
            }
            Vertex v = start;
            Vertex prevNode = start;
            Edge maxEdge = null;
            //check for a cycle
            while (!noCycle)
            {

                noCycle = false;
                foreach (Edge e in v.neighbours)
                {
                    if (maxEdge == null) maxEdge = e;

                    if (e.node1 != v.data)
                    {
                        if (visited[g.GetVertex(e.node1)] == true && e.node1 != prevNode.data)
                        {
                            if (maxEdge.weight < e.weight) maxEdge = e;
                            return true;
                        }
                        else if (g.GetVertex(e.node1).neighbours.Count > 1 && e.node1 != prevNode.data)
                        {
                            if (maxEdge.weight < e.weight) maxEdge = e;
                            var kvp = new KeyValuePair<Vertex,Vertex> (v, g.GetVertex(e.node1));
                            uncheckedRoutes.Enqueue(kvp);
                            
                        }
                        else if (g.GetVertex(e.node1).neighbours.Count <=1 )
                        {
                            visited[g.GetVertex(e.node1)] = true;
                        }
                        

                    }
                    else
                    {
                        if (visited[g.GetVertex(e.node2)] == true && e.node2 != prevNode.data)
                        {
                            if (maxEdge.weight < e.weight) maxEdge = e;
                            return true;
                        }
                        else if (g.GetVertex(e.node2).neighbours.Count > 1 && e.node2 != prevNode.data)
                        {
                            if (maxEdge.weight < e.weight) maxEdge = e;
                            var kvp = new KeyValuePair<Vertex, Vertex>(v, g.GetVertex(e.node2));
                            uncheckedRoutes.Enqueue(kvp);
                        }
                        else if (g.GetVertex(e.node2).neighbours.Count <= 1)
                        {
                            maxEdge = null;
                            visited[g.GetVertex(e.node2)] = true;
                        }
                    }
                }
                visited[v] = true;//set the node to visited

                //check to see all nodes have been checked
                noCycle = true;
                foreach(KeyValuePair<Vertex,bool>kvp in visited)
                {
                    if (kvp.Value == false)
                    {
                        noCycle = false;
                    }
                        
                }
                
                //if there is still cycles check from an unchecked path
                if(noCycle == false)
                {
                    if (uncheckedRoutes.Count != 0)
                    {
                        v = uncheckedRoutes.Peek().Value;
                        prevNode = uncheckedRoutes.Dequeue().Key;
                    }
                    else {
                        noCycle = true;
                    }
                        
                    
                    
                }

                
            }
            return false;
        }


        //execute the Prim algorithm
        public void prim(ref Graph g)
        {

        }

        //execute the aStar algorithm
        public void aStar(ref Graph g)
        {

        }
    }
}
