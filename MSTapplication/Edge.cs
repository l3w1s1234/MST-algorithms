using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;


namespace MSTapplication
{

    namespace simpleGraph
    {
        [Serializable()]
        class Edge : ISerializable
        {

            public string data { get; set; }
            public string node1 { get; set; }
            public string node2 { get; set; }
            public float weight { get; set; }


            public Edge(string n1ID, string n2ID, float w, String id)
            {
                node1 = n1ID;
                node2 = n2ID;
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
                node1 = (string)info.GetValue("Node1", typeof(string));
                node2 = (string)info.GetValue("Node2", typeof(string)); ;
                weight = (float)info.GetValue("Weight", typeof(float)); ;
                data = (string)info.GetValue("Data", typeof(string)); ;
            }
        }
    }
}

   
