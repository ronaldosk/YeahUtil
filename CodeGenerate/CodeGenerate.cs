using CodeBuilder.Configuration;
using CodeBuilder.WinForm.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YUtils;

namespace CodeGenerate
{
    public partial class CodeGenerate : Form
    {
        public CodeGenerate()
        {
            InitializeComponent();

            this.dgvView.Rows.Add();
            this.dgvView.Rows[0].Cells[0].Value = "Index";

            this.dgvView.Rows.Add();
            this.dgvView.Rows[1].Cells[0].Value = "CreateOrUpdateModal";
            this.dgvView.Rows[1].Cells[1].Value = 1;

            CombDataSourceItems();
            this.comboBox1.SelectedIndex = 0;


        }

        #region BusinessName 属性

        private string _businessName;

        /// <summary>
        /// 设置或取得 BusinessName 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string BusinessName
        {
            get => _businessName;

            set
            {
                _businessName = value;
                //if (Set(ref _businessName, value))
                {
                    ServiceFolder = BusinessName + "s";
                    ServiceInterfaceName = $"I{BusinessName}AppService";
                    ServiceName = $"{BusinessName}AppService";
                    ViewFolder = $@"App\Main\views\{ServiceFolder.LowerFirstChar()}";
                }
            }
        }

        #endregion

        #region ServiceFolder 属性

        private string _serviceFolder;

        /// <summary>
        /// 设置或取得 ServiceFolder 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string ServiceFolder
        {
            get => _serviceFolder;

            set
            {
                _serviceFolder = value;
            }
        }

        #endregion

        #region ServiceInterfaceName 属性

        private string _serviceInterfaceName;

        /// <summary>
        /// 设置或取得 ServiceInterfaceName 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string ServiceInterfaceName
        {
            get => _serviceInterfaceName;

            set
            {
                _serviceInterfaceName = value;
            }
        }

        #endregion

        #region ServiceName 属性

        private string _serviceName;

        /// <summary>
        /// 设置或取得 ServiceName 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string ServiceName
        {
            get => _serviceName;

            set
            {
                _serviceName = value;
            }
        }

        #endregion

        #region ViewFolder 属性

        private string _viewFolder;

        /// <summary>
        /// 设置或取得 ViewFolder 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string ViewFolder
        {
            get => _viewFolder;

            set
            {
                _viewFolder = value;
            }
        }

        #endregion

        #region ViewFiles 属性

        private List<string> _viewFiles;

        /// <summary>
        /// 设置或取得 ViewFiles 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public List<string> ViewFiles
        {
            get => _viewFiles;

            set
            {
                _viewFiles = value;
            }
        }

        #endregion
        

        private void textBox1_Validated(object sender, EventArgs e)
        {
            BusinessName = this.textBox1.Text;

            if (this.checkBox1.Checked)
                BusinessName = BusinessName.CamelCaseName();
            this.textBox2.Text = ServiceFolder;
            this.textBox3.Text = ServiceInterfaceName;
            this.textBox4.Text = ServiceName;
            this.textBox7.Text = ViewFolder;
            this.textBox1.Text = BusinessName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dbTreeView.Nodes.Clear();
            TreeNode rootNode = ExportModelHelper.Export(this.comboBox1.Text, this.dbTreeView);
            rootNode.ExpandAll();
            this.dbTreeView.SelectedNode = rootNode;
        }

        
        private void CombDataSourceItems()
        {
            this.comboBox1.Items.Clear();

            foreach (DataSourceElement dataSource in ConfigManager.DataSourceSection.DataSources)
            {
                this.comboBox1.Items.Add(dataSource.Name);
                
            }
        }

        private void dbTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.textBox1.Text = this.dbTreeView.SelectedNode.Text;
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void tsmi_DataSourceCfg_Click(object sender, EventArgs e)
        {
            DataSourceConfig dscfgdlg = new DataSourceConfig();
            dscfgdlg.ShowDialog();
        }
    }
}
