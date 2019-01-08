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

namespace MSTapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            }
            else
            {
                placeEdge = false;
                placeNode = true;
            }
            
        }

        //when edge button is clicked
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
            nodeEllipse.Width = 15;
            nodeEllipse.Height = 15;

            //adding event handler for mouse controls
            nodeEllipse.MouseRightButtonDown += new MouseButtonEventHandler(nodeEllipse_MouseRightButtonDown);
            nodeEllipse.MouseEnter += new MouseEventHandler(nodeEllipse_MouseEnter);
            nodeEllipse.MouseLeave += new MouseEventHandler(nodeEllipse_MouseLeave);

            display.Children.Add(nodeEllipse);
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
        }


        //when mouse leaves reset color
        private void nodeEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 0, 0);
            ellipse.Fill = solidColorBrush;
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
            foreach (FrameworkElement element in canvas.Children )

            {

                //get elemnts old left and top
                double old_Left = Canvas.GetLeft(element);
                double old_Top = Canvas.GetTop(element);

               //set left and top
                Canvas.SetLeft(element, old_Left * scale_Width);
                Canvas.SetTop(element, old_Top * scale_Height );


            }
        }



    }
}
