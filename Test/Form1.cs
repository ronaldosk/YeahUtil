using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YeahTools;
using YeahTools.GeometricHelper;
using YeahTools.HelperForm;
using YeahAlgorithms.Core;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XMLHelper xmlhelper = new XMLHelper(this.textBox1.Text);
            xmlhelper.CreateClassByXML(@"D:\");
            MessageBox.Show("OK!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(2, 0));
            points.Add(new Point(1, 3));//points.Add(new Point(2, 4));//变形了，结果又有不同
            points.Add(new Point(2, 5));
            points.Add(new Point(5, 1));
            points.Add(new Point(0, 3));
            
            
            Point center = new Point();
            SortHelper.ClockwiseSortPoints(points, out center);
            string outMsg = "";
            foreach(Point pt in points)
            {
                outMsg += string.Format("[{0},{1}]\r\n",pt.X,pt.Y);
            }
            outMsg += string.Format("重心是：[{0},{1}]\r\n", center.X, center.Y);
            MessageBox.Show(outMsg);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Point> points = new List<Point>();
            
            points.Add(new Point(1, 1));//points.Add(new Point(2, 4));//变形了，结果又有不同
            points.Add(new Point(2, 5));
            points.Add(new Point(5, 1));
            points.Add(new Point(2, 0));
            points.Add(new Point(0, 3));

            Point basePoint = SortHelper.FindBasePoint(points);

            string outMsg = string.Format("[{0},{1}]\r\n", basePoint.X, basePoint.Y);
            //foreach (Point pt in points)
            //{
            //    outMsg += string.Format("[{0},{1}]\r\n", pt.X, pt.Y);
            //}
            MessageBox.Show(outMsg);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CodeGenerate codg = new CodeGenerate();
            if (codg.ShowDialog() == DialogResult.OK)
                return;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //int[] iarry = { 1, 22, 5, 77, 33, 65, 45, 8, 894, 84, 8, 1 };
            ////iarry.QSortAsc();
            //iarry.QSortDesc();
            //MessageBox.Show(iarry.ShowAll());

            double[] darry = { 1, 22, 5, 77, 33, 65, 45, 8, 894, 84, 8, 1 };
            //darry.dQSortAsc();
            darry.dQSortDesc();
            MessageBox.Show(darry.ShowAll());
        }
    }
}
