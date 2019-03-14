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
    class Vertex : ISerializable
    {
        public String data { get; set; }
        public LinkedList<Edge> neighbours;


        
        public Vertex(String d)
        {
            neighbours = new LinkedList<Edge>();
            data = d;
            
        }

        //set the weight of the edge
        public void setNeighbourWeight(ref Vertex node, float weight)
        {
                foreach (Edge e in neighbours)
                {
                    if (e.node1 == node.data || e.node2 == node.data)
                    {
                        e.weight = weight;
                    }
                }
        }

        //add a neighbour to list
        public void addNeighbour(Edge edge)
        {
            
            neighbours.AddFirst(edge);
        }

        //returns true or false if node is a neighour
        public Boolean hasNeighbour(string n)
        {
            foreach(Edge e in neighbours)
            {
                if(e.node1 == n || e.node2 == n)
                {
                    return true;
                }
            }
            return false;
        }

        //returns if edge already exits
        public Boolean hasEdge(string eID)
         {

            foreach(Edge e in neighbours)
            {
                if(e.data == eID)
                {
                    return true;
                }
            }
            return false;
         }

        //returns true or false if node is any neighour
        public Boolean hasNeighbours()
        {
            if(neighbours == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //get a list of the neighbours  connected tyo this node
        public LinkedList<Edge> getNeighbours()
        {
            return neighbours;
        }

        //get the edge with the edge ID
        public Edge getEdge(string eID)
            {
            Edge edge = null;
            foreach(Edge e in neighbours)
                {
                if(e.data == eID)
                    {
                       edge = e;
                    }
                }
            return edge;
            }

        //remove an edge from the list
        public void removeEdge(String edgeID)
        {
            try
            {
               var edge = getEdge(edgeID);
                if(edge!= null)
                    {
                    neighbours.Remove(edge);
                    }
                   
                
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Removing edge failed");
            }
            

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", data);
            info.AddValue("Neighbours", neighbours);
        }

        public Vertex(SerializationInfo info, StreamingContext context)
        {
            neighbours = (LinkedList<Edge>)info.GetValue("Neighbours", typeof(LinkedList<Edge>));
            data = (string)info.GetValue("Name", typeof(string)); ;
        }
    }
}
