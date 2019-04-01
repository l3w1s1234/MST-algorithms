using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTapplication.simpleGraph;


namespace UnitTestProject
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void TestHasEdge()
        {
            Graph graph = new Graph();

            graph.addNode("_0");
            graph.addNode("_1");
            graph.addEdge(5, "_0", "_1","_0");

            var result = graph.hasEdge(new Edge("_0","_1",0,"_0"));


            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TestAddNodeAndHasNode()
        {
            Graph graph = new Graph();

            graph.addNode("_0");

            var result = graph.hasVertex("_0");
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TesthasAnyNeighbour()
        {
            Vertex vertex = new Vertex("0");
            Edge edge = new Edge("_0", "_1", 4, "_0");

            vertex.neighbours.AddFirst(edge);

            var result = vertex.hasNeighbours();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TesthasNeighbour()
        {
            Vertex vertex = new Vertex("0");
            Edge edge = new Edge("_0", "_1", 4, "_0");

            vertex.neighbours.AddFirst(edge);

            var result = vertex.hasNeighbour("_1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestRemoveEdge()
        {
            Vertex vertex = new Vertex("0");
            Edge edge = new Edge("_0", "_1", 4, "_0");

            vertex.neighbours.AddFirst(edge);


            //remove edge fro graph
            vertex.removeEdge("_0");

            var result = vertex.hasEdge(edge.data);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGetVertex()
        {
            Graph graph = new Graph();
            Vertex vertex = new Vertex("_9");

            graph.addNode("_9");

            var result = graph.GetVertex("_9");

            Assert.AreEqual(vertex.data,result.data);
        }

        [TestMethod]
        public void TestGetVertices()
        {
            Graph graph = new Graph();
            List<Vertex> list = new List<Vertex>();

            list.Add(new Vertex("_0"));
            list.Add(new Vertex("_3"));
            list.Add(new Vertex("_5"));
            list.Add(new Vertex("_9"));

            graph.addNode("_9");
            graph.addNode("_3");
            graph.addNode("_5");
            graph.addNode("_0");


            var result = graph.GetVertices();
          

            Assert.AreEqual(result.Count,list.Count);
        }

        [TestMethod]
        public void TestGetEdges()
        {
            //make test variables
            Graph graph = new Graph();

            List<Edge> list = new List<Edge>();

            list.Add(new Edge("_0","_3",8,"_1"));
            list.Add(new Edge("_9", "_3", 5, "_3"));
            list.Add(new Edge("_0", "_5", 4, "_2"));
            list.Add(new Edge("_5", "_3", 3, "_8"));

            //add values to graph
            graph.addNode("_9");
            graph.addNode("_3");
            graph.addNode("_5");
            graph.addNode("_0");
            graph.addEdge(8, "_0", "_3","_1");
            graph.addEdge(3, "_5", "_3", "_8");
            graph.addEdge(5, "_9", "_3", "_3");
            graph.addEdge(4, "_0", "_5", "_2");
            

            var result = graph.GetEdges();

            Assert.AreEqual(list.Count, result.Count );
        }

        [TestMethod]
        public void TestRemoveEdgeGraph()
        {
            Graph graph = new Graph();

            Edge edge = new Edge("_9", "_3", 5, "_3");
            graph.addNode("_9");
            graph.addNode("_3");

            graph.addEdge(5, "_9", "_3", "_3");


            graph.removeEdge("_9", "_3");
            graph.removeEdge("_3", "_3");

            var result = graph.hasEdge(edge);

            Assert.IsFalse(result);

        }
        [TestMethod]
        public void TestRemoveNode()
        {
            Graph graph = new Graph();

            graph.addNode("_0");

           graph.removeVertex("_0");

            var result = graph.hasVertex("_0");

            Assert.IsFalse(result);

        }

    }
}
