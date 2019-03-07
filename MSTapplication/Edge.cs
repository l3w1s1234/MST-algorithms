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
    class Edge : ISerializable
    {
        public string data { get; set; }
        public Vertex node1 { get; set; }
        public  Vertex node2 { get; set; }
        public float weight { get; set; }

        
        public Edge(ref Vertex n1, ref Vertex n2, float w, String id)
        {
            node1 = n1;
            node2 = n2;
            weight = w;
            data = id;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", data);
            info.AddValue("Node1", node1);
            info.AddValue("Node2", node2);
            info.AddValue("Weight", weight);
        }

        public Edge(SerializationInfo info, StreamingContext context)
        {
            node1 = (Vertex)info.GetValue("Node1", typeof(Vertex));
            node2 = (Vertex)info.GetValue("Node2", typeof(Vertex)); ;
            weight = (float)info.GetValue("Weight", typeof(float)); ;
            data = (string)info.GetValue("Data", typeof(string)); ;
        }
    }
}
