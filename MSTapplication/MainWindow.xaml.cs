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

namespace MSTapplication
{
  
    public partial class MainWindow : Window
    
    {
        Gmap_Window window = new Gmap_Window();

        private bool placeNode = false;
        private bool placeEdge = false;

        private Point nodePoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        //openGmap window when checked
        private void gmap_Checked(object sender, RoutedEventArgs e)
        {
            window = new Gmap_Window();
            window.Show();
        }

        //close Gmap window when unchecked
        private void gmap_Unchecked(object sender, RoutedEventArgs e)
        {
            window.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        //for saving a graph
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
            }
            else
            {
                placeEdge = false;
                placeNode = true;
            }
            
        }

        private void EdgeButton_Click(object sender, RoutedEventArgs e)
        {
            if (placeEdge)
            {
                placeEdge = false;
            }
            else
            {
                placeEdge = true;
                placeNode = false;
            }
        }


        //for when the mouse has clicked inside canvas
        private void mouseClickCanvas(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (placeNode)
            {
                nodePoint = Mouse.GetPosition(display);
                addNode();
            }
            else if (placeEdge)
            {


            }
        }


        //create a new node 
        private void addNode()
        {
            Ellipse nodeEllipse = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();

            solidColorBrush.Color = Color.FromRgb(255,0,0);

            nodeEllipse.Fill = solidColorBrush;

            nodeEllipse.SetValue(Canvas.LeftProperty, nodePoint.X);
            nodeEllipse.SetValue(Canvas.TopProperty, nodePoint.Y);

            

            nodeEllipse.StrokeThickness = 2;
            nodeEllipse.Stroke = Brushes.Black;
            nodeEllipse.Width = 10;
            nodeEllipse.Height = 10;

            //adding event handler for right mouse down:
            nodeEllipse.MouseRightButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseRightButtonDown);
            nodeEllipse.MouseEnter += new MouseEventHandler(nodeEllipse_MouseEnter);
            nodeEllipse.MouseLeave += new MouseEventHandler(nodeEllipse_MouseLeave);

            display.Children.Add(nodeEllipse);
        }

        //remove nodes on right button down
        void nodeEllipse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            display.Children.Remove(ellipse);
        }

        //highlight nodes when mouse hovers over them
        void nodeEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            ellipse.Fill = solidColorBrush;
        }


        //when mouse leaves reset color
        void nodeEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 0, 0);
            ellipse.Fill = solidColorBrush;
        }




    }
}
