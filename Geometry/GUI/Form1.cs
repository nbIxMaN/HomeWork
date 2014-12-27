using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Graphics gmc = Graphics.FromHwnd(panel1.Handle);
                gmc.Clear(Color.White);
                List<Point> points = new List<Point>();
                string coordinates = Points.Text;
                foreach (string point in coordinates.Split(';'))
                {
                    double doubleX = Convert.ToDouble(point.Split(',')[0]);
                    double doubleY = Convert.ToDouble(point.Split(',')[1]);
                    int x = Convert.ToInt32(Math.Round(doubleX, 0));
                    int y = Convert.ToInt32(Math.Round(doubleY, 0));
                    points.Add(new Point(x, panel1.Height - y));
                }
                Point[] pointsArray = points.ToArray();
                gmc.FillPolygon(Brushes.Aqua, pointsArray);
                gmc.DrawPolygon(Pens.Aqua, pointsArray);
                points.Clear();
            }
            catch
            {
                Points.Clear();
                Points.AppendText("Please enter points");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<double> points = new List<double>();
                string coordinates = Points.Text;
                foreach (string point in coordinates.Split(';'))
                {
                    double x = Convert.ToDouble(point.Split(',')[0]);
                    double y = Convert.ToDouble(point.Split(',')[1]);
                    points.Add(x);
                    points.Add(panel1.Height - y);
                }
                double[] pointsArray = points.ToArray();
                Geometry.Geometry geom = 
                    new Geometry.Geometry(pointsArray[0], pointsArray[1], pointsArray[2], pointsArray[3], pointsArray[4], pointsArray[5], pointsArray[6], pointsArray[7]);
                if (geom.IsParalel)
                {
                    Log.Clear();
                    Log.AppendText("True");
                }
                else
                {
                    Log.Clear();
                    Log.AppendText("False");
                }
            }
            catch
            {
                Points.Clear();
                Points.AppendText("Please enter points");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            Points.Clear();
            for (int i = 0; i < 3; i++)
            {
                Points.AppendText(r.Next(panel1.Width).ToString());
                Points.AppendText(",");
                Points.AppendText(r.Next(panel1.Height).ToString());
                Points.AppendText(";");
            }
            Points.AppendText(r.Next(panel1.Width).ToString());
            Points.AppendText(",");
            Points.AppendText(r.Next(panel1.Height).ToString());
        }
    }
}
