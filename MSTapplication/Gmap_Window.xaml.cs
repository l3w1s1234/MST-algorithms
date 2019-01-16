using GMap.NET;

using GMap.NET.WindowsPresentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MSTapplication
{
    
    /// <summary>
    /// Interaction logic for Gmap.xaml
    /// </summary>
    public partial class Gmap_Window : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //checkers for placing nodes and edges
        private bool placeNode = false;
        private bool placeEdge = false;

        private double longitude;
        private double latitude;

        public Gmap_Window()
        {
            InitializeComponent();
            //Gmap settings
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(55.955154, -3.188248);
            gmap.MinZoom = 0;
            gmap.MaxZoom = 24;
            gmap.Zoom = 9;
            
            Loaded += ToolWindow_Loaded;
        }
       

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text file|*.txt";
            save.Title = "Save an Text File";
            save.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (save.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs = (System.IO.FileStream)save.OpenFile();


                fs.Close();
            }
        }


        private void NodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (placeNode)
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
        private void mouseClickGmap(object sender, MouseButtonEventArgs e)
        {
          
           
            if (placeNode)
            {
                longitude = gmap.FromLocalToLatLng(Convert.ToInt32(e.GetPosition(gmap).X), Convert.ToInt32(e.GetPosition(gmap).Y)).Lng;
                latitude = gmap.FromLocalToLatLng(Convert.ToInt32(e.GetPosition(gmap).X), Convert.ToInt32(e.GetPosition(gmap).Y)).Lat;
                addNode();
            }
            else if (placeEdge)
            {


            }
        }


        //create a new node 
        private void addNode()
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 0, 0);

            GMapMarker node = new GMapMarker(new PointLatLng(latitude, longitude));
            node.Shape = new Ellipse
            {
                Width = 15,
                Height = 15,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = solidColorBrush
            };
           
            
            //add marker to map
            gmap.Markers.Add(node);
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromRgb(255, 255, 0);
            item.Shape = new Ellipse
            {
                Width = 15,
                Height = 15,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = solidColorBrush
            };
        }

    }
}
