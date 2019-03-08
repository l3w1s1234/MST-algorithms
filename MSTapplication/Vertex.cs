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
        public void addNeighbour(ref Edge edge)
        {
            neighbours.AddFirst(edge);
        }

        //returns true or false if node is a neighour
        public Boolean hasNeighbour(Vertex n)
        {
            foreach(Edge e in neighbours)
            {
                if(e.node1 == n.data || e.node2 == n.data)
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

        

        //remove an edge from the list
        public void removeEdge(String edgeID)
        {
            try
            {
                foreach (Edge e in neighbours)
                {
                    if (e.data == edgeID)
                    {
                        neighbours.Remove(e);
                    }
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
