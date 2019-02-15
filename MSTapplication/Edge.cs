using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Edge
    {
        public string data { get; set; }
        public Vertex node1 { get; set; }
        public  Vertex node2 { get; set; }
        private float weight { get; set; }

        public Edge(ref Vertex n1, ref Vertex n2, float w, String id)
        {
            node1 = n1;
            node2 = n2;
            weight = w;
            data = id;
        }

    }
}
