using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using simpleGraph;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using ClassicAlgorithms;
using GA;

namespace MSTapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    
{
        //name of the graph
        Graph mainGraph = new Graph();
        classicAlgorithms ca = new classicAlgorithms();
        GeneticAlgorithm ga = new GeneticAlgorithm();

        Gmap_Window window = new Gmap_Window();
        
        //checkers
        private bool placeNode = false;

        //this is so that placing nodes isnt re-enabled when hovering over node
        private bool nodeHoverPlaceable = false;

        
        private bool dragable = false;

        //makes manipulation of shapes easier, allows to keep track of theses things
        private Dictionary<String,Line> drawableEdges = new Dictionary<String, Line>();
        private Dictionary<String,Ellipse> drawableNodes = new Dictionary<String, Ellipse>();

        private Dictionary<String, Label> edgeWeights = new Dictionary<String, Label>();
        private Dictionary<String, Label> nodeNames = new Dictionary<String, Label>();

        private Dictionary<String, TextBox> changeWeight = new Dictionary<String, TextBox>();

        //used to identify the shapes and what they are linked to on the Graph
        private string nodeID = "_0";
        private string edgeID = "_0";

        //keeps the original postion of the node before it hasbeen moved
        Point originalPosition;


        public MainWindow()
        {
            InitializeComponent();
        }

        //perform ga on graph
        private void Ga_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromRgb(0, 0, 0);
            foreach (KeyValuePair<string,Line> kvp in drawableEdges)
            {
                kvp.Value.Stroke = black;
            }

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            Graph mst = null;

            //as long as there are edges perform algorithm
            if (mainGraph.GetEdges().Count != 0)
            {
                
                try
                {
                    mst = ga.run(int.Parse(popSize.Text), ref mainGraph, int.Parse(iterations.Text));
                    //colour the edges
                    foreach (Vertex v in mst.GetVertices())
                    {
                        foreach (Edge edge in v.neighbours)
                        {
                            drawableEdges[edge.data].Stroke = solidColorBrush;
                        }
                    }

                    //show mst weight
                    mstWeight.Content = mst.getGraphWeight();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed starting algorithm");
                }

            }
        }


        //perform Dijkstra on graph
        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromRgb(0, 0, 0);
            foreach (KeyValuePair<string, Line> kvp in drawableEdges)
            {
                kvp.Value.Stroke = black;
            }

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            Graph mst = null;

            //as long as there are edges perform algorithm
            if (mainGraph.GetEdges().Count != 0)
            {

                try
                {
                    mst = ca.dijkstra(source.Text, destination.Text ,ref mainGraph);
                    //colour the edges
                    foreach (Vertex v in mst.GetVertices())
                    {
                        foreach (Edge edge in v.neighbours)
                        {
                            drawableEdges[edge.data].Stroke = solidColorBrush;
                        }
                    }

                    //show mst weight
                    mstWeight.Content = mst.getGraphWeight();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed starting algorithm");
                }

            }
        }


        private void DijkstraNoDest_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromRgb(0, 0, 0);
            foreach (KeyValuePair<string, Line> kvp in drawableEdges)
            {
                kvp.Value.Stroke = black;
            }

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            Graph mst = null;

            //as long as there are edges perform algorithm
            if (mainGraph.GetEdges().Count != 0)
            {

                try
                {
                    mst = ca.dijkstraNoDest(source2.Text, ref mainGraph);
                    //colour the edges
                    foreach (Vertex v in mst.GetVertices())
                    {
                        foreach (Edge edge in v.neighbours)
                        {
                            drawableEdges[edge.data].Stroke = solidColorBrush;
                        }
                    }

                    //show mst weight
                    mstWeight.Content = mst.getGraphWeight();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed starting algorithm");
                }

            }
        }

        //perform kruskal algorithm
        private void Kruskal_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromRgb(0, 0, 0);
            foreach (KeyValuePair<string, Line> kvp in drawableEdges)
            {
                kvp.Value.Stroke = black;
            }

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            Graph mst = null;

            //as long as there are edges perform algorithm
            if (mainGraph.GetEdges().Count != 0)
            {

                try
                {
                    mst = ca.kruskal(ref mainGraph);
                    //colour the edges
                    foreach (Vertex v in mst.GetVertices())
                    {
                        foreach (Edge edge in v.neighbours)
                        {
                            drawableEdges[edge.data].Stroke = solidColorBrush;
                        }
                    }

                    //show mst weight
                    mstWeight.Content = mst.getGraphWeight();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed starting algorithm");
                }

            }
        }

        //perform boruvka Algorithm
        private void boruvka_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromRgb(0, 0, 0);
            foreach (KeyValuePair<string, Line> kvp in drawableEdges)
            {
                kvp.Value.Stroke = black;
            }

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            Graph mst = null;

            //as long as there are edges perform algorithm
            if (mainGraph.GetEdges().Count != 0)
            {
                mst = ca.boruvka(ref mainGraph);

                //colour the edges
                foreach (Vertex v in mst.GetVertices())
                {
                    foreach (Edge edge in v.neighbours)
                    {
                        drawableEdges[edge.data].Stroke = solidColorBrush;
                    }
                }

                //show mst weight
                mstWeight.Content = mst.getGraphWeight();

            }
        }

        //used to change id for nodes
        private void incrementID(ref string s)
        {
            int i = 0;
            String number = Regex.Match(s, @"\d+").ToString();

            try { Int32.TryParse(number, out i);
                i++;
                s = "_" + i.ToString();
            }
            catch { System.Diagnostics.Debug.WriteLine("increment Failed"); }

            
        }

        //get the id's integer
        private int idInt(ref string s)
        {
            int i = 0;
            String number = Regex.Match(s, @"\d").ToString();

            Int32.TryParse(number, out i);

            return i;      
        }

        private void gmap_Checked(object sender, RoutedEventArgs e)
        {
            window = new Gmap_Window();
            window.Show();
        }
        private void gmap_Unchecked(object sender, RoutedEventArgs e)
        {
            window.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    
        //for saving graph data
        private void saveGraph_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save= new SaveFileDialog();
            save.Filter = "Graph Data|*.dat|Json|*.json";
            save.Title = "Save Graph Data";
            save.ShowDialog();
            
            if (save.FileName != "" && mainGraph != null)
            {

                try
                {
                    switch (save.FilterIndex)
                    {
                        case 1:
                            Stream fs = (FileStream)save.OpenFile();
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fs, mainGraph);
                            fs.Close();
                            break;
                        case 2:
                            using (StreamWriter file = new StreamWriter(save.OpenFile()))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                
                                serializer.Serialize(file, mainGraph);
                            }
                            break;
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Save failed");
                }
            }
            
        }

        //for saving graph coordinates
        private void saveCoor_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Graph Coordinates|*.txt";
            save.Title = "Save a Graph Coordinates";
            save.ShowDialog();

            if (save.FileName != "" && mainGraph != null)
            {

                try
                {
                    StreamWriter sw = new StreamWriter(save.OpenFile());
                   foreach(KeyValuePair<string,Ellipse> kvp in drawableNodes)
                    {
                        var x = Canvas.GetLeft(kvp.Value);
                        var y = Canvas.GetTop(kvp.Value);
                        sw.WriteLine(kvp.Key + "," + x + "," + y);
                    }
                    sw.Close();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Save failed");
                }
            }

        }

        //load graph data
        private void loadGraph_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Graph Data|*.dat|Graph Json|*.json|All Files|*.dat;*.json";
            openFile.Title = "Open a Graph Data file";
            openFile.ShowDialog();

            var ext = System.IO.Path.GetExtension(openFile.FileName);
            if (openFile.FileName != "")
            {
                //clear previous graph data
                clearGraph();

                //check file extension and convert file accordingly
                if(ext == ".dat")
                {
                    try
                    {
                        Stream fs = (FileStream)openFile.OpenFile();
                        var bf = new BinaryFormatter();
                        mainGraph = (Graph)bf.Deserialize(fs);
                        fs.Close();
                        //put graph to screen using random coordinates
                        randomEllipseEdges();
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("Load failed");
                    }

                }
                 else if(ext == ".json")
                {
                    TextReader reader = null;
                    try
                    {
                        reader = new StreamReader(openFile.OpenFile());
                        var fileContents = reader.ReadToEnd();
                        mainGraph = JsonConvert.DeserializeObject<Graph>(fileContents);
                        reader.Close();
                        //put graph to screen using random coordinates
                        randomEllipseEdges();
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("Load failed");
                    }
                }

            }

            graphWeight.Content = mainGraph.getGraphWeight();
        }

        //load coordiunate data
        private void loadCoor_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Graph Coordinate|*.txt";
            openFile.Title = "Open Graph coordinates";
            openFile.ShowDialog();

            //if the file name isnt empty try reading the file
            if (openFile.FileName != "")
            {

                try
                {
                    using (var sr = new StreamReader(openFile.OpenFile()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] words = line.Split(',');

                            var x = double.Parse(words[1]);
                            var y = double.Parse(words[2]);

                            //change the coordinates of the nodes
                            drawableNodes[words[0]].SetValue(Canvas.LeftProperty, x);
                            drawableNodes[words[0]].SetValue(Canvas.TopProperty, y);

                            nodeNames[words[0]].SetValue(Canvas.LeftProperty, x - (drawableNodes[words[0]].Width + 10));
                            nodeNames[words[0]].SetValue(Canvas.TopProperty, y - drawableNodes[words[0]].Height);
                        }
                    }

                    //change the edges coordinates
                    foreach (Vertex n in mainGraph.GetVertices())
                    {
                        updateEdge(n);
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Load failed");
                }

            }
        }

        //clear everything from graph
        private void clearGraph()
        {
            //clear all
            ga = new GeneticAlgorithm();
            mainGraph = null;          
            display.Children.Clear();
            drawableEdges.Clear();
            drawableNodes.Clear();
            nodeNames.Clear();
            edgeWeights.Clear();
            nodeID = "_0";
            edgeID = "_0";
            graphWeight.Content = "0";
        }

        //randomly generate the graph placement on the graph display
        private void randomEllipseEdges()
        {
            Random random = new Random();

            //draw ellipses and edges
            foreach (Vertex node in mainGraph.GetVertices())
            {
                
                Ellipse nodeEllipse = new Ellipse();

                Label nodeName = new Label();

                nodeName.Name = node.data;
                nodeName.Content = node.data;
                nodeName.FontSize = 10;

                SolidColorBrush solidColorBrush = new SolidColorBrush();

                solidColorBrush.Color = Color.FromRgb(255, 0, 0);

                nodeEllipse.Fill = solidColorBrush;

                double x = (double)random.Next(0,(int)display.ActualWidth);
                double y = (double)random.Next(0, (int)display.ActualHeight);

                nodeEllipse.StrokeThickness = 2;
                nodeEllipse.Stroke = Brushes.Black;
                nodeEllipse.Width = 15;
                nodeEllipse.Height = 15;

                nodeEllipse.SetValue(Canvas.LeftProperty, x - (nodeEllipse.Width / 2));
                nodeEllipse.SetValue(Canvas.TopProperty, y - (nodeEllipse.Height / 2));

                nodeName.SetValue(Canvas.LeftProperty, x - (nodeEllipse.Width+5));
                nodeName.SetValue(Canvas.TopProperty, y - nodeEllipse.Height);

                drawableNodes.Add(node.data, nodeEllipse);
                nodeNames.Add(node.data, nodeName);

                nodeEllipse.Name = node.data;

                

                //adding event handler for mouse controls
                nodeEllipse.MouseRightButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseRightButtonDown);
                nodeEllipse.MouseLeftButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseLeftButtonDown);
                nodeEllipse.MouseLeftButtonUp += new MouseButtonEventHandler(nodeEllipse_MouseLeftButtonUp);
                nodeEllipse.MouseEnter += new MouseEventHandler(nodeEllipse_MouseEnter);
                nodeEllipse.MouseLeave += new MouseEventHandler(nodeEllipse_MouseLeave);
                nodeEllipse.MouseMove += new MouseEventHandler(nodeEllipse_MouseMove);

                //add to canvas
                display.Children.Add(nodeEllipse);
                display.Children.Add(nodeName);

            }

            //draw edges
            foreach (Vertex node in mainGraph.GetVertices())
            {
                foreach (Edge e in node.neighbours)
                {
                    if (!drawableEdges.ContainsKey(e.data))
                    {
                        Label weight = new Label();

                        weight.Name = e.data;
                        weight.Content = e.weight;
                        weight.FontSize = 10;
                        
                        var x1 = Canvas.GetLeft(drawableNodes[e.node1]) + (drawableNodes[e.node1].Width / 2);
                        var y1 = Canvas.GetTop(drawableNodes[e.node1]) + (drawableNodes[e.node1].Height / 2);
                        var x2 = Canvas.GetLeft(drawableNodes[e.node2]) + (drawableNodes[e.node2].Width / 2);
                        var y2 = Canvas.GetTop(drawableNodes[e.node2]) + (drawableNodes[e.node2].Height / 2);

                        var edge = getEdge(x1, y1, x2, y2);

                        weight.SetValue(Canvas.LeftProperty, (x1 + x2) / 2);
                        weight.SetValue(Canvas.TopProperty, ((y1 + y2) / 2)-5);
                        edge.Name = e.data;

                        edgeWeights.Add(e.data, weight);
                        drawableEdges.Add(e.data, edge);
                        display.Children.Add(weight);
                        display.Children.Add(edge);
                    }
                }
            }
                

            //get the current id for the nodes and edges        
            nodeID = "_" + drawableNodes.Count.ToString();
            edgeID = "_" + drawableEdges.Count.ToString();
        }
    
        //get an edge made with xy coor
        private Line getEdge(double x1, double y1, double x2, double y2)
        {
            Line edge = new Line();

            edge.Stroke = Brushes.Black;
            edge.X1 = x1;
            edge.Y1 = y1;
            edge.X2 = x2;
            edge.Y2 = y2;
            edge.StrokeThickness = 2;

            return edge;
            
        }
       
        //when node button is pressed
        private void nodeButton_Click(object sender, RoutedEventArgs e)
        {
            if(placeNode)
            {
                placeNode = false;
                nodeHoverPlaceable = false;
            }
            else
            {
                
                placeNode = true;
                nodeHoverPlaceable = true;
            }
            
        }

        //when edge has been added
        private void addEdge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //get node data
                var node1 = mainGraph.GetVertex(Node1.Text);
                var node2 = mainGraph.GetVertex(Node2.Text);
                float weight = float.Parse(Weight.Text);

                //check edge doesnt exist and then add to graph and draw
                if(!node1.hasNeighbour(node2.data))
                {
                    var x1 = Canvas.GetLeft(drawableNodes[node1.data]) + (drawableNodes[node1.data].Width / 2);
                    var y1 = Canvas.GetTop(drawableNodes[node1.data]) + (drawableNodes[node1.data].Height / 2);

                    var x2 = Canvas.GetLeft(drawableNodes[node2.data]) + (drawableNodes[node2.data].Width / 2);
                    var y2 = Canvas.GetTop(drawableNodes[node2.data]) + (drawableNodes[node2.data].Height / 2);

                    mainGraph.addEdge(weight, node1.data, node2.data,edgeID);
                    drawEdge(x1,y1,x2,y2,weight.ToString());
                }
                graphWeight.Content = mainGraph.getGraphWeight();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Adding edge failed");
            }
        }

        //for when the mouse has clicked inside canvas
        private void mouseClickCanvas(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (placeNode)
            {
                var mousePos = e.GetPosition(display);
                addNode(mousePos);
            }
            
        }

        //draw Edge
        private void drawEdge(double x1, double y1, double x2, double y2,string w)
        {
            Line edge = new Line();

            edge.Stroke = Brushes.Black;

            edge.X1 = x1;
            edge.Y1 = y1;
            edge.X2 = x2;
            edge.Y2 = y2;

            edge.StrokeThickness = 2;

            edge.Name=edgeID;

            Label weight = new Label();
            weight.Name = edge.Name;
            weight.Content = w;
            weight.FontSize = 10;

            weight.SetValue(Canvas.LeftProperty, (x1 + x2) / 2);
            weight.SetValue(Canvas.TopProperty, ((y1 + y2) / 2) - 5);
            

            edgeWeights.Add(edge.Name, weight);
            drawableEdges.Add(edge.Name, edge);
            incrementID(ref edgeID);


            display.Children.Add(edge);
            display.Children.Add(weight);
        }


        //create a new node 
        private void addNode(Point mousePos)
        {
            Ellipse nodeEllipse = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();


            Label nodeName = new Label();
            nodeName.Name = nodeID;
            nodeName.Content = nodeID;
            nodeName.FontSize = 10;

            solidColorBrush.Color = Color.FromRgb(255,0,0);
            
            nodeEllipse.Fill = solidColorBrush;

            double x = mousePos.X;
            double y = mousePos.Y;

            nodeEllipse.StrokeThickness = 2;
            nodeEllipse.Stroke = Brushes.Black;
            nodeEllipse.Width = 15;
            nodeEllipse.Height = 15;

            nodeEllipse.SetValue(Canvas.LeftProperty, x - (nodeEllipse.Width / 2));
            nodeEllipse.SetValue(Canvas.TopProperty, y - (nodeEllipse.Height / 2));

            nodeName.SetValue(Canvas.LeftProperty, x - (nodeEllipse.Width+5));
            nodeName.SetValue(Canvas.TopProperty, y - nodeEllipse.Height);

            //add to graph
            mainGraph.addNode(nodeID);
            drawableNodes.Add(nodeID, nodeEllipse);
            nodeNames.Add(nodeID, nodeName);

            nodeEllipse.Name = nodeID;
            incrementID(ref nodeID);
         
            //adding event handler for mouse controls
            nodeEllipse.MouseRightButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseRightButtonDown);
            nodeEllipse.MouseLeftButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseLeftButtonDown);
            nodeEllipse.MouseLeftButtonUp += new MouseButtonEventHandler(nodeEllipse_MouseLeftButtonUp);
            nodeEllipse.MouseEnter += new MouseEventHandler(nodeEllipse_MouseEnter);
            nodeEllipse.MouseLeave += new MouseEventHandler(nodeEllipse_MouseLeave);
            nodeEllipse.MouseMove += new MouseEventHandler(nodeEllipse_MouseMove);

            display.Children.Add(nodeEllipse);
            display.Children.Add(nodeName);

        }

        //display the neigbours within the node and weights
        private void displayNeighbours(ref Vertex node)
        {
            //get all edges
            foreach(Edge e in node.neighbours)
            {
                var sp = new StackPanel();
                var sp2 = new StackPanel();
                var label1 = new Label();
                var label2 = new Label();
                var textbox = new TextBox();
                var button = new Button();

                sp2.Orientation = Orientation.Horizontal;
                button.Content = "Update";
                button.Click += new RoutedEventHandler(setEdgeWeight);

                textbox.Text = e.weight.ToString();
                textbox.Margin = new Thickness(5);

                var n = e.node1;
                button.Name = n;
                

                if (e.node1 == node.data)
                {
                    n = e.node2;
                    button.Name = n;
                }

                changeWeight.Add(button.Name, textbox);

                label1.Content = "Node: " + n;
                label2.Content = "Weight: ";

                //add elements to stack panels and listbox
                sp2.Children.Add(label2);
                sp.Children.Add(label1);
                sp.Children.Add(sp2);
                sp2.Children.Add(textbox);
                sp.Children.Add(button); 

                nodeNeighbours.Items.Add(sp);
            }
        }

        //set the edge weight
        private void setEdgeWeight(object sender, EventArgs e)
        {
            Button but = sender as Button;
            var tb = changeWeight[but.Name];
            

            var node = mainGraph.GetVertex(but.Name);

            try
            {
                node.setNeighbourWeight(ref node, float.Parse(tb.Text));
                var edge = node.getNeighbourEdge(node.data);
                edgeWeights[edge.data].Content = edge.weight;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Changing Weight Failed");
            }

            graphWeight.Content = mainGraph.getGraphWeight();

        }

        //controls if node has been pressed 
        private void nodeEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(display);

            dragable = true;
            Ellipse ellipse = sender as Ellipse;

            //show name of ellipse
            nodeName.Text = ellipse.Name;

            var node = mainGraph.GetVertex(ellipse.Name);

            //show nodes neighbours
            changeWeight.Clear();
            nodeNeighbours.Items.Clear();
            displayNeighbours(ref node);
            

            originalPosition = e.GetPosition(display);
            ellipse.CaptureMouse();
        }

        //controls if node has been released
        private void nodeEllipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            dragable = false;
            Ellipse ellipse = sender as Ellipse;
            ellipse.ReleaseMouseCapture();

            //keep element within canvas
            if (!display.IsMouseOver)
            {
                ellipse.SetValue(Canvas.LeftProperty, originalPosition.X - (ellipse.Width+5));
                ellipse.SetValue(Canvas.TopProperty, originalPosition.Y - ellipse.Height);

                var node = mainGraph.GetVertex(ellipse.Name);

                nodeName.SetValue(Canvas.LeftProperty, originalPosition.X - ellipse.Width);
                nodeName.SetValue(Canvas.TopProperty, originalPosition.Y - ellipse.Height);

                updateEdge(node);
            }
            
        }

        //redraw edge postion
       private void updateEdge(Vertex v)
        {

            foreach (Edge e in v.neighbours)
            {

                drawableEdges[e.data].X1 = Canvas.GetLeft(drawableNodes[e.node1]) + (drawableNodes[e.node1].Width/2);
                drawableEdges[e.data].Y1 = Canvas.GetTop(drawableNodes[e.node1]) + (drawableNodes[e.node1].Height / 2);

                drawableEdges[e.data].X2 = Canvas.GetLeft(drawableNodes[e.node2]) + (drawableNodes[e.node2].Width / 2);
                drawableEdges[e.data].Y2 = Canvas.GetTop(drawableNodes[e.node2]) + (drawableNodes[e.node2].Height / 2);


                edgeWeights[e.data].SetValue(Canvas.LeftProperty, (drawableEdges[e.data].X1 + drawableEdges[e.data].X2) / 2);
                edgeWeights[e.data].SetValue(Canvas.TopProperty, ((drawableEdges[e.data].Y1 + drawableEdges[e.data].Y2) / 2) - 5);
            }
            
            
        }

        //controls nodes movement
        private void nodeEllipse_MouseMove(object sender, MouseEventArgs e)
        {

            if (!dragable) return;

            Canvas canvas = sender as Canvas;
            Ellipse ellipse = sender as Ellipse;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(display);

            // center the rect on the mouse
            double left = mousePos.X - (ellipse.ActualWidth / 2);
            double top = mousePos.Y - (ellipse.ActualHeight / 2);

            //update node in graph
            var node = mainGraph.GetVertex(ellipse.Name);

            nodeNames[ellipse.Name].SetValue(Canvas.LeftProperty, left - (ellipse.Width +4));
            nodeNames[ellipse.Name].SetValue(Canvas.TopProperty, top - ellipse.Height);

            //check that there are any connected edges and update their postion
            if (node.hasNeighbours())
            {
                updateEdge(node);
            }
            

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
        }

        //remove nodes on right button down
        private void nodeEllipse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            var edgeIDs = mainGraph.getEdgeIDs(ellipse.Name);
            var nID = nodeNames[ellipse.Name];

            
            //remove from all arrays
            mainGraph.removeEdges(ellipse.Name);
            mainGraph.removeVertex(ellipse.Name);
            drawableNodes.Remove(ellipse.Name);
            nodeNames.Remove(ellipse.Name);

            foreach(String edge in edgeIDs)
            {
                display.Children.Remove(drawableEdges[edge]);
                display.Children.Remove(edgeWeights[edge]);
                drawableEdges.Remove(edge);
                edgeWeights.Remove(edge);
            }

            graphWeight.Content = mainGraph.getGraphWeight();

            display.Children.Remove(ellipse);
            display.Children.Remove(nID);

            
        }

        //highlight nodes when mouse hovers over them
        private void nodeEllipse_MouseEnter(object sender, MouseEventArgs e)
        {

            Ellipse ellipse = sender as Ellipse;
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            ellipse.Fill = solidColorBrush;

            //cant place node while hovering over node only does this while in place node mode  
            if(placeNode)
            {
                placeNode = false;
            }
        }


        //when mouse leaves reset color
        private void nodeEllipse_MouseLeave(object sender, MouseEventArgs e)
        {

            Ellipse ellipse = sender as Ellipse;
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 0, 0);
            ellipse.Fill = solidColorBrush;

            
            //cant place node while hovering over node only does this while in place node mode  
            if (nodeHoverPlaceable)
            {
                placeNode = true;
            }
         
        }

        //resize all elements within canvas on resize
        private void displaySizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SizeChangedEventArgs canvasChangedArgs = e;
            //check that there has been a change in size
            if (canvasChangedArgs.PreviousSize.Width == 0) return;

             //set new sizes and old
            double old_Height = canvasChangedArgs.PreviousSize.Height;
            double new_Height = canvasChangedArgs.NewSize.Height;
            double old_Width = canvasChangedArgs.PreviousSize.Width;
            double new_Width = canvasChangedArgs.NewSize.Width;

            double scale_Width = new_Width / old_Width;
            double scale_Height = new_Height / old_Height;

            //change elements in canvas
            foreach (KeyValuePair<string, Ellipse> element in drawableNodes)
            {
                var node = mainGraph.GetVertex(element.Value.Name);

                //get elemnts old left and top
                double old_Left = Canvas.GetLeft(element.Value);
                double old_Top = Canvas.GetTop(element.Value);

                
               //set left and top
                Canvas.SetLeft(element.Value, old_Left * scale_Width);
                Canvas.SetTop(element.Value, old_Top * scale_Height );
                nodeNames[element.Value.Name].SetValue(Canvas.LeftProperty, (old_Left * scale_Width) - (element.Value.Width + 4));
                nodeNames[element.Value.Name].SetValue(Canvas.TopProperty, (old_Top * scale_Height)-element.Value.Height);

            }

            //update lines/edges
            foreach(Vertex n in mainGraph.GetVertices())
            {
                updateEdge(n);
            }
            
        }

      
    }
}
