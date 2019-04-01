using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTapplication.simpleGraph;
using MSTapplication.ClassicAlgorithms;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for CAtests
    /// </summary>
    [TestClass]
    public class CAtests
    {
        [TestMethod]
        public void TestBoruvka()
        {
            float expected = 10;

            Graph graph = new Graph();
            classicAlgorithms ca = new classicAlgorithms();

            graph.addNode("_0");
            graph.addNode("_1");
            graph.addNode("_2");
            graph.addNode("_3");
            graph.addNode("_4");

            graph.addEdge(3, "_0", "_4", "_1");
            graph.addEdge(21, "_0", "_3", "_2");
            graph.addEdge(3, "_1", "_2", "_3");
            graph.addEdge(31, "_1", "_4", "_4");
            graph.addEdge(2, "_2", "_4", "_5");
            graph.addEdge(2, "_3", "_4", "_6");

            var result = ca.boruvka(ref graph);

            Assert.AreEqual(expected,result.getGraphWeight());
        }

        [TestMethod]
        public void TestDijkstra()
        {
            float expected = 10;

            Graph graph = new Graph();
            classicAlgorithms ca = new classicAlgorithms();

            graph.addNode("_0");
            graph.addNode("_1");
            graph.addNode("_2");
            graph.addNode("_3");
            graph.addNode("_4");

            graph.addEdge(3, "_0", "_4", "_1");
            graph.addEdge(21, "_0", "_3", "_2");
            graph.addEdge(3, "_1", "_2", "_3");
            graph.addEdge(31, "_1", "_4", "_4");
            graph.addEdge(2, "_2", "_4", "_5");
            graph.addEdge(2, "_3", "_4", "_6");

            var result = ca.dijkstra("_0","_1",ref graph);

            Assert.AreEqual(expected, result.getGraphWeight());

        }

        [TestMethod]
        public void TestDijkstraNoDest()
        {
            float expected = 10;

            Graph graph = new Graph();
            classicAlgorithms ca = new classicAlgorithms();

            graph.addNode("_0");
            graph.addNode("_1");
            graph.addNode("_2");
            graph.addNode("_3");
            graph.addNode("_4");

            graph.addEdge(3, "_0", "_4", "_1");
            graph.addEdge(21, "_0", "_3", "_2");
            graph.addEdge(3, "_1", "_2", "_3");
            graph.addEdge(31, "_1", "_4", "_4");
            graph.addEdge(2, "_2", "_4", "_5");
            graph.addEdge(2, "_3", "_4", "_6");

            var result = ca.dijkstraNoDest("_0",ref graph);

            Assert.AreEqual(expected, result.getGraphWeight());
        }

        [TestMethod]
        public void TestKruskal()
        {
            float expected = 10;

            Graph graph = new Graph();
            classicAlgorithms ca = new classicAlgorithms();

            graph.addNode("_0");
            graph.addNode("_1");
            graph.addNode("_2");
            graph.addNode("_3");
            graph.addNode("_4");

            graph.addEdge(3, "_0", "_4", "_1");
            graph.addEdge(21, "_0", "_3", "_2");
            graph.addEdge(3, "_1", "_2", "_3");
            graph.addEdge(31, "_1", "_4", "_4");
            graph.addEdge(2, "_2", "_4", "_5");
            graph.addEdge(2, "_3", "_4", "_6");

            var result = ca.kruskal(ref graph);

            Assert.AreEqual(expected, result.getGraphWeight());
        }
    }
}
