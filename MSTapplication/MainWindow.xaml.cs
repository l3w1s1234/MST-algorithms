﻿using Microsoft.Win32;
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

namespace MSTapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    
{
        //name of the graph
        Graph mainGraph = new Graph(); 

        Gmap_Window window = new Gmap_Window();

        //checkers
        private bool placeNode = false;

        //this is so that placing nodes isnt re-enabled when hovering over node
        private bool nodeHoverPlaceable = false;

        
        private bool dragable = false;

        //makes manipulation of shapes easier, allows to keep track of theses things
        private Dictionary<String,Line> drawableEdges = new Dictionary<String, Line>();
        private Dictionary<String,Ellipse> drawableNodes = new Dictionary<String, Ellipse>();
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

        //used to change id for nodes
        private void incrementID(ref string s)
        {
            int i = 0;
            String number = Regex.Match(s, @"\d").ToString();

            try { Int32.TryParse(number, out i);
                i++;
                s = "_" + i.ToString();
            }
            catch { System.Diagnostics.Debug.WriteLine("increment Failed"); }

            
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

        private void saveGraph_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save= new SaveFileDialog();
            save.Filter = "Text file|*.txt";
            save.Title = "Save a Text File";
            save.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (save.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs = (System.IO.FileStream)save.OpenFile();
                
           
                fs.Close();
            }
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
                if(!node1.hasNeighbour(node2))
                {
                    var x1 = Canvas.GetLeft(drawableNodes[node1.data]) + (drawableNodes[node1.data].Width / 2);
                    var y1 = Canvas.GetTop(drawableNodes[node1.data]) + (drawableNodes[node1.data].Height / 2);

                    var x2 = Canvas.GetLeft(drawableNodes[node2.data]) + (drawableNodes[node2.data].Width / 2);
                    var y2 = Canvas.GetTop(drawableNodes[node2.data]) + (drawableNodes[node2.data].Height / 2);

                    mainGraph.addEdge(weight, node1.data, node2.data,edgeID);
                    drawEdge(x1,y1,x2,y2);
                }
                
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
        private void drawEdge(double x1, double y1, double x2, double y2)
        {
            Line edge = new Line();
            var t = new TextBlock();

            edge.Stroke = Brushes.Black;

            edge.X1 = x1;
            edge.Y1 = y1;
            edge.X2 = x2;
            edge.Y2 = y2;

            edge.StrokeThickness = 2;

            edge.Name=edgeID;

            drawableEdges.Add(edge.Name, edge);
            incrementID(ref edgeID);


            display.Children.Add(edge);
        }

        //create a new node 
        private void addNode(Point mousePos)
        {
            Ellipse nodeEllipse = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();

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

            //add to graph
            mainGraph.addNode(nodeID);
            drawableNodes.Add(nodeID, nodeEllipse);

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
                button.Name = n.data;
                

                if (e.node1 == node)
                {
                    n = e.node2;
                    button.Name = n.data;
                }

                changeWeight.Add(button.Name, textbox);

                label1.Content = "Node: " + n.data;
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
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Changing Weight Failed");
            }
            


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
                ellipse.SetValue(Canvas.LeftProperty, originalPosition.X - (ellipse.Width / 2));
                ellipse.SetValue(Canvas.TopProperty, originalPosition.Y - (ellipse.Height / 2));

                var node = mainGraph.GetVertex(ellipse.Name);

                updateEdge(node);
            }
            
        }

        //redraw edge postion
       private void updateEdge(Vertex v)
        {

            foreach (Edge e in v.neighbours)
            {

                drawableEdges[e.data].X1 = Canvas.GetLeft(drawableNodes[e.node1.data]) + (drawableNodes[e.node1.data].Width/2);
                drawableEdges[e.data].Y1 = Canvas.GetTop(drawableNodes[e.node1.data]) + (drawableNodes[e.node1.data].Height / 2);

                drawableEdges[e.data].X2 = Canvas.GetLeft(drawableNodes[e.node2.data]) + (drawableNodes[e.node2.data].Width / 2);
                drawableEdges[e.data].Y2 = Canvas.GetTop(drawableNodes[e.node2.data]) + (drawableNodes[e.node2.data].Height / 2);

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

            //check that there are any connected edges and update their postion
            if(node.hasNeighbours())
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
            display.Children.Remove(ellipse);
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

                
            }

            //update lines/edges
            foreach(Vertex n in mainGraph.GetVertices())
            {
                updateEdge(n);
            }
            
        }



    }
}
