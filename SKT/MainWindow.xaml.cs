// Modern computer technologies
using System;
using System.Collections.Generic;
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

namespace SKT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double a = 10; // Only for drawning
        double kx; // Scaling x
        double ky; // Scaling y
        double x;
        double z;
        Element field = new Element(); // Поле
        List<Element> elements; // Элементы поля
        DrawingGroup drawingGroup = new DrawingGroup();
        public MainWindow()
        {
            InitializeComponent();
            buildaxes();
        }

        private void buildaxes()
        {
            GeometryDrawing myGeometryDrawing = new GeometryDrawing();
            GeometryGroup lines = new GeometryGroup();
            myGeometryDrawing.Pen = new Pen(Brushes.Black, 3);
            lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height), new Point(a, 0))); // z
            lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height/2), new Point(graphImage.Width, graphImage.Height / 2))); // x

            myGeometryDrawing.Geometry = lines;
            drawingGroup.Children.Add(myGeometryDrawing);
            graphImage.Source = new DrawingImage(drawingGroup);
        }

        private void FieldButton_Click(object sender, RoutedEventArgs e)
        {
            drawingGroup.Children.Clear();
            buildaxes();
            elements = new List<Element>();
            string yval = yVal.Text;
            string xval = xVal.Text;
            if(!(double.TryParse(yval, out double number1)) || !(double.TryParse(xval, out double number2)))
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else
            {
                x = double.Parse(xval);
                z = double.Parse(yval);
                // points of the field
                field.p1 = new Point() { X = 0, Y = -z }; 
                field.p2 = new Point() { X = 0, Y = 0 }; 
                field.p3 = new Point() { X = x, Y = -z }; 
                field.p4 = new Point() { X = x, Y = 0 }; 


                kx = (graphImage.Width - a) / x;
                ky = (graphImage.Height/2) / z;

                //Draw Field
                GeometryDrawing myGeometryDrawing = new GeometryDrawing();
                GeometryGroup lines = new GeometryGroup();
                myGeometryDrawing.Pen = new Pen(Brushes.Red, 2);

                //Add lines (field borders)
                lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height/2), new Point(x*kx - a, graphImage.Height / 2))); 
                lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height/ 2 + z * ky), new Point(x*kx - a, graphImage.Height / 2 + z * ky))); 
                lines.Children.Add(new LineGeometry(new Point(x * kx - a, graphImage.Height / 2), new Point(x * kx - a, graphImage.Height / 2 +z*ky))); 
                lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height / 2), new Point(a, graphImage.Height / 2 +z*ky))); 


                myGeometryDrawing.Geometry = lines;
                drawingGroup.Children.Add(myGeometryDrawing);
                graphImage.Source = new DrawingImage(drawingGroup);

                MakeElements();
            }
        }

        // building of elements
        private void MakeElements()
        {
            string zcrush = yCrush.Text;
            string xcrush = xCrush.Text;
            if (!(double.TryParse(zcrush, out double number1)) || !(double.TryParse(xcrush, out double number2)))
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else if (double.Parse(xcrush) <=0 || double.Parse(zcrush) <= 0)
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else
            {
                double dx = x / double.Parse(xcrush);
                double dz = z / double.Parse(zcrush);

                for(int i = 1; i <= double.Parse(xcrush); i++)
                    for(int j = 1; j <= double.Parse(zcrush); j++)
                    {
                        Element el = new Element();
                        el.p1 = new Point() {X = (i - 1) * dx, Y = -j* dz };
                        el.p2 = new Point() {X = (i - 1)*dx, Y = -(j - 1)*dz};
                        el.p3 = new Point() {X = i*dx, Y = -j*dz};
                        el.p4 = new Point() {X = i * dx, Y = -(j -1) * dz };

                        elements.Add(el);
                    }

                // Paint
                GeometryDrawing myGeometryDrawing = new GeometryDrawing();
                GeometryGroup lines = new GeometryGroup();
                myGeometryDrawing.Pen = new Pen(Brushes.Red, 3);
                for (int i = 1; i < double.Parse(xcrush); i++)
                {
                    lines.Children.Add(new LineGeometry(new Point(i*dx*kx, graphImage.Height/2), new Point(i * dx*kx, graphImage.Height / 2 + z * ky)));
                }
                for (int j = 1; j < double.Parse(zcrush); j++)
                {
                    lines.Children.Add(new LineGeometry(new Point(a, j*dz*ky + graphImage.Height / 2), new Point(x*kx - a, j * dz*ky + graphImage.Height / 2)));
                }
                myGeometryDrawing.Geometry = lines;
                drawingGroup.Children.Add(myGeometryDrawing);
                graphImage.Source = new DrawingImage(drawingGroup);
            }
        }
    }

    // The struct of an element
    /*
     p2-----------p4
     |             |
     |             |
     |             |
     |             |
     p1-----------p3
         
         */
    public class Element
    {
        public Point p1 { get; set; }
        public Point p2 { get; set; }
        public Point p3 { get; set; }
        public Point p4 { get; set; }

    }
}
