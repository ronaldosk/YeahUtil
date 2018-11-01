using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YUtils;

namespace YeahTools.Function.UserControls
{
    public partial class LabelTextBox : UserControl
    {
        public string LabelText
        {
            get { return this.label1.Text;}
            set { this.label1.Text = value; }
        }
        public string TextValue {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }
        

        public LabelTextBox()
        {
            InitializeComponent();
            //MessageBox.Show(TextValue.GetASCIILength().ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.ForeColor = System.Drawing.SystemColors.InfoText;
        }

        
    }
}
