using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SKT

{
    // The struct of an element
    /*
     *-------------*
     |             |
     |hz           |
     |             |
     |      hx     |
     p-------------*
         
         */
    public class Element
    {
        public Point point { get; set; }
        public double hx { get; set; }
        public double hz { get; set; }
        public double px { get; set; }
        public double pz { get; set; }
        public Element()
        {
            px = 0;
            pz = 0;
        }
    }

    public class DirectProblem
    {
        public  List<double> Bx { get; set; } 
        public List<double> Bz { get; set; }       
        public List<Element> elements;
        public int N;
        public double X;
        


        public DirectProblem(List<Element> elements, int N, double X)
        {
            this.elements = elements;
            this.N = N;
            this.X = X;
            Calc();
        }
        private double Calc_Koeff(Element el) // mes(Omega)/4*pi
        {
            return (el.hx * el.hz) / (4 * Math.PI);
        }
        public void Calc()
        {
            Bx = new List<double>();
            Bz = new List<double>();
            if (Bx.Count != 0 || Bz.Count != 0)
            {
                Bx.Clear();
                Bz.Clear();
            }
            for (int i = 0; i < N; i++) 
            {
                Bx.Add(0);
                Bz.Add(0);

                foreach (var element in elements)
                {
                    double x_loc, z_loc, r;
                    x_loc = i * X / (N - 1) - (element.point.X + element.hx / 2);
                    z_loc = - (element.point.Y + element.hz / 2);
                    r = Math.Sqrt(x_loc * x_loc + z_loc * z_loc);

                    Bx[i] += Calc_Koeff(element) / (r * r * r) * (
                        element.px * ((3 * x_loc * x_loc) / (r * r) - 1) +
                        element.pz * (3 * x_loc * z_loc) / (r * r));

                    Bz[i] += Calc_Koeff(element) / (r * r * r) * (
                        element.px * (3 * x_loc * z_loc) / (r * r) +
                        element.pz * ((3 * z_loc * z_loc) / (r * r) - 1));
                }
            }
        }
    }
    public class InverseProblem 
    {
        public List<double> Px { get; set; }
        public List<double> Pz { get; set; }
        public List<Element> elements;
        public int N;
        public double X;


        public InverseProblem(List<Element> elements, int N, double X)
        {
            this.elements = elements;
            this.N = N;
            this.X = X;
            Calc();
        }
        private double Calc_Koeff(Element el) // mes(Omega)/4*pi
        {
            return (el.hx * el.hz) / (4 * Math.PI);
        }
        public void Calc()
        {
            ////////////////////Удали, если что не так/////////////////////
            List<double> Bx;
            List<double> Bz;
            ////////////////////Удали, если что не так/////////////////////
            Bx = new List<double>();
            Bz = new List<double>();
            if (Bx.Count != 0 || Bz.Count != 0)
            {
                Bx.Clear();
                Bz.Clear();
            }
            for (int i = 0; i < N; i++)
            {
                Bx.Add(0);
                Bz.Add(0);

                foreach (var element in elements)
                {
                    double x_loc, z_loc, r,
                        Lxx, Lxz,
                        Lzx, Lzz;

                    x_loc = i * X / (N - 1) - (element.point.X + element.hx / 2);
                    z_loc = -(element.point.Y + element.hz / 2);
                    r = Math.Sqrt(x_loc * x_loc + z_loc * z_loc);

                    Lxx = Calc_Koeff(element) / (r * r * r) * ((3 * x_loc * x_loc) / (r * r) - 1);
                    Lxz = Calc_Koeff(element) / (r * r * r) * (3 * x_loc * z_loc) / (r * r);

                    Bx[i] += element.px * Lxx +
                             element.pz * Lxz;

                    Lzx = Calc_Koeff(element) / (r * r * r) * (3 * x_loc * z_loc) / (r * r);
                    Lzz = Calc_Koeff(element) / (r * r * r) * ((3 * z_loc * z_loc) / (r * r) - 1);
                    Bz[i] += element.px * Lzx +
                             element.pz * Lzz;
                }
            }
        }


    }
}
