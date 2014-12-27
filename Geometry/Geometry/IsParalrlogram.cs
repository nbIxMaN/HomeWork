using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Geometry
    {
        struct Point
        {
            public double x;
            public double y;
            public Point(double x1, double y1)
            {
                x = x1;
                y = y1;
            }
        }
        private Point a;
        private Point b;
        private Point c;
        private Point d;
        private double abk = 0;
        private double bck = 0;
        private double cdk = 0;
        private double dak = 0;
        public Geometry(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            a = new Point(x1, y1);
            b = new Point(x2, y2);
            c = new Point(x3, y3);
            d = new Point(x4, y4);
        }
        public bool IsParalel
        {
            get
            {
                abk = (a.y - b.y) / (a.x - b.x);
                bck = (b.y - c.y) / (b.x - c.x);
                cdk = (c.y - d.y) / (c.x - d.x);
                dak = (d.y - a.y) / (d.x - a.x);
                return (((Math.Abs(abk - cdk)) < 0.0000001) && ((Math.Abs(bck - dak)) < 0.0000001)) ||
                       (((Math.Abs(bck - dak)) < 0.0000001) && Double.IsInfinity(abk) && Double.IsInfinity(cdk));
            }
        }
    }
}
