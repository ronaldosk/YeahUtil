using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YeahTools.Function.UserControls
{
    public partial class LabelCombox : UserControl
    {
        public string LabelText
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }
        public string CombValue
        {
            get { return this.comboBox1.Text; }
        }

        public LabelCombox()
        {
            InitializeComponent();
        }

        public void InitialCombbox(List<string> itmList)
        {
            foreach(var itm in itmList)
            {
                this.comboBox1.Items.Add(itm);
            }
            this.comboBox1.SelectedIndex = 0;
        }

    }
}
