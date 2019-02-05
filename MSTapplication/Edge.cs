using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Edge
    {
        private Vertex node1;
        private Vertex node2;
        private int weight;

        public Edge(ref Vertex n1, ref Vertex n2, ref int w)
        {
            node1 = n1;
            node2 = n2;
            weight = w;
        }

    }
}
