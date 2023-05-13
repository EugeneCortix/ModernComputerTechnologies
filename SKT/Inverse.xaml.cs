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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SKT
{
    /// <summary>
    /// Логика взаимодействия для Inverse.xaml
    /// </summary>

    public partial class Inverse : Window
    {
        double a = 0; // Only for drawning

        DrawingGroup drawingGroup = new DrawingGroup();
        List<Element> elementsInv;
        List<DataInGrid> datasInGridInv;
        double kx; // Scaling x
        double ky; // Scaling y
        double x;
        double z;

        public DirectProblem CalicDirectProblem;



        public Inverse()
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
            lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height / 2), new Point(graphImage.Width, graphImage.Height / 2))); // x

            myGeometryDrawing.Geometry = lines;
            drawingGroup.Children.Add(myGeometryDrawing);
            graphImage.Source = new DrawingImage(drawingGroup);

        }


        private void fieldButtonInv_Click(object sender, RoutedEventArgs e)
        {
            drawingGroup.Children.Clear();

            buildaxes();
            elementsInv = new List<Element>();
            datasInGridInv = new List<DataInGrid>();
            string yval = yValInv.Text;
            string xval = xValInv.Text;
            if (!(double.TryParse(yval, out double number1)) || !(double.TryParse(xval, out double number2)))
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else
            {
                x = double.Parse(xval);
                z = double.Parse(yval);

                kx = (graphImage.Width - a) / x;
                ky = (graphImage.Height / 2) / z;

                DrawField(x / int.Parse(xCrushInv.Text), z / int.Parse(yCrushInv.Text));

                MakeElements();

                GridOfWInv.ItemsSource = datasInGridInv;

            }
        }
        private void DrawField(double dx, double dz)
        {
            GeometryDrawing myGeometryDrawing = new GeometryDrawing();
            GeometryGroup lines = new GeometryGroup();
            myGeometryDrawing.Pen = new Pen(Brushes.Red, 1);

            lines.Children.Add(new LineGeometry(new Point(a, graphImage.Height / 2 + z * ky), new Point(x * kx - a, graphImage.Height / 2 + z * ky)));
            lines.Children.Add(new LineGeometry(new Point(x * kx - a, graphImage.Height / 2), new Point(x * kx - a, graphImage.Height / 2 + z * ky)));

            for (int i = 1; i < double.Parse(xCrushInv.Text); i++)
            {
                lines.Children.Add(new LineGeometry(new Point(i * dx * kx, graphImage.Height / 2), new Point(i * dx * kx, graphImage.Height / 2 + z * ky)));
            }
            for (int j = 1; j < double.Parse(yCrushInv.Text); j++)
            {
                lines.Children.Add(new LineGeometry(new Point(a, j * dz * ky + graphImage.Height / 2), new Point(x * kx - a, j * dz * ky + graphImage.Height / 2)));
            }

            myGeometryDrawing.Geometry = lines;
            drawingGroup.Children.Add(myGeometryDrawing);
            graphImage.Source = new DrawingImage(drawingGroup);
        }
        private void MakeElements()
        {
            string zcrush = yCrushInv.Text;
            string xcrush = xCrushInv.Text;
            if (!(int.TryParse(zcrush, out int number1)) || !(int.TryParse(xcrush, out int number2)))
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else if (int.Parse(xcrush) <= 0 || int.Parse(zcrush) <= 0)
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
            }
            else
            {
                double dx = x / int.Parse(xcrush);
                double dz = z / int.Parse(zcrush);

                for (int j = 0; j < int.Parse(zcrush); j++)
                    for (int i = 0; i < int.Parse(xcrush); i++)
                    {
                        Element el = new Element();
                        el.point = new Point() { X = i * dx, Y = -(z - j * dz) };
                        el.hx = dx;
                        el.hz = dz;
                        elementsInv.Add(el);
                        datasInGridInv.Add(new DataInGrid(i + j * int.Parse(xcrush), el.px, el.pz));
                    }
            }
        }

        private void CALC_P_Click(object sender, RoutedEventArgs e)
        {
            if (!(int.TryParse(yCrushInv.Text, out int number1)) || !(int.TryParse(xCrushInv.Text, out int number2)))
            {
                MessageBox.Show("Ты что вводишь, дуралей?");
                return;
            }
            else if (CalicDirectProblem.N < elementsInv.Count)
            {
                MessageBox.Show("Сетка слишком подробная, дуралей");
                return;
            }


        }
    }
}
