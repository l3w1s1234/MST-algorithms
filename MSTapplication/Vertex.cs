﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGraph
{
    class Vertex
    {
        public String data { get; set; }
        public LinkedList<Edge> neighbours;
        public double X {get; set;}
        public double Y { get; set; }

        public Vertex(String d, double x, double y)
        {
            neighbours = new LinkedList<Edge>();
            data = d;
            X = x;
            Y = y;
        }

        //set the weight of the edge
        public void setNeighbourWeight(ref Vertex node, float weight)
        {
                foreach (Edge e in neighbours)
                {
                    if (e.node1 == node || e.node2 == node)
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
                if(e.node1 == n || e.node2 == n)
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
        public LinkedList<Edge> getNeighbour()
        {
            return neighbours;
        }

      
    }
}
