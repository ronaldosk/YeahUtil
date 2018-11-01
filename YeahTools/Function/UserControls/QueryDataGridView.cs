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
    public partial class QueryDataGridView : UserControl
    {
        public DataGridView dgv {
            get { return this.dataGridView1; }
            set { this.dataGridView1 = value; }
        }
        public QueryDataGridView()
        {
            InitializeComponent();
        }
    }
}
