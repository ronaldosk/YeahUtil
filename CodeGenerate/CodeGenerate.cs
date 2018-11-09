using CodeBuilder.Configuration;
using CodeBuilder.PhysicalDataModel;
using CodeBuilder.WinForm.UI;
using CodeGenerate.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
                    ServiceFolder = $@"\Application\{BusinessName}s";//BusinessName + "s";
                    ServiceInterfaceName = $"I{BusinessName}AppService.cs";
                    ServiceName = $"{BusinessName}AppService.cs";
                    ViewFolder = $@"\Views\{ServiceFolder.LowerFirstChar()}";
                    DtoFolder = $@"{ServiceFolder}\Dtos";
                    EntityFolder = $@"\Core\{BusinessName}s";
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

        #region DtoFolder 属性

        private string _dtoFolder;

        /// <summary>
        /// 设置或取得 DtoFolder 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string DtoFolder
        {
            get => _dtoFolder;

            set
            {
                _dtoFolder = value;
            }
        }

        #endregion

        #region EntityFolder 属性

        private string _entityFolder;

        /// <summary>
        /// 设置或取得 EntityFolder 属性
        /// 更改属性值并引发PropertyChanged事件
        /// </summary>
        public string EntityFolder
        {
            get => _entityFolder;

            set
            {
                _entityFolder = value;
            }
        }

        #endregion


        private void textBox1_Validated(object sender, EventArgs e)
        {
            _businessName = this.textBox1.Text;

            if (this.checkBox1.Checked)
                BusinessName = BusinessName.CamelCaseName();
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
            string businessname = this.dbTreeView.SelectedNode.Text;
            if (checkBox2.Checked)
                businessname.IgnorePrefix(this.prefixTxtBox.Text, ',', '，');
            this.textBox1.Text = businessname;
            toolStripStatusLabel1.Text = this.textBox1.Text;
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
            Reflesh_dgvColms();
            Reflesh_dgvIndexs();
        }

        void Reflesh_dgvColms()
        {
            if (this.dbTreeView.SelectedNode.Tag is BaseTable)
            {
                BaseTable cols = this.dbTreeView.SelectedNode.Tag as BaseTable;
                this.textBox1.Tag = cols;//划重点，关联
                dgvColms.Rows.Clear();
                foreach (var col in cols.Columns)
                {
                    int index = dgvColms.Rows.Add();
                    dgvColms.Rows[index].Cells[2].Value = col.Value.DisplayName;
                    dgvColms.Rows[index].Cells[3].Value = col.Value.Id;
                    dgvColms.Rows[index].Cells[4].Value = col.Value.Comment;
                    dgvColms.Rows[index].Cells[5].Value = col.Value.DataType;
                    dgvColms.Rows[index].Cells[6].Value = col.Value.DataType == "nvarchar" ? col.Value.Length / 2 : col.Value.Length;
                    dgvColms.Rows[index].Cells[7].Value = (col.Value.DataType == "float" || col.Value.DataType == "double") ? "2" : "";
                    dgvColms.Rows[index].Cells[8].Value = col.Value.IsNullable ? "N" : "Y";
                    dgvColms.Rows[index].Cells[9].Value = "Admin";

                }
            }
        }

        void Reflesh_dgvIndexs()
        {
            if (this.dbTreeView.SelectedNode.Tag is BaseTable)
            {
                BaseTable cols = this.dbTreeView.SelectedNode.Tag as BaseTable;
                dgvIndexs.Rows.Clear();
                foreach (var col in cols.Columns)
                {
                    int index = dgvIndexs.Rows.Add();
                    dgvIndexs.Rows[index].Cells[1].Value = col.Value.DisplayName;
                }
            }
        }

        private void tsmi_DataSourceCfg_Click(object sender, EventArgs e)
        {
            DataSourceConfig dscfgdlg = new DataSourceConfig();
            dscfgdlg.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BusinessName = this.textBox1.Text;
            string appserver = Const.OutputPath + Path.Combine(ServiceFolder, ServiceName);
            string iappserver = Const.OutputPath + Path.Combine(ServiceFolder, ServiceInterfaceName);
            string dtolist = Const.OutputPath + Path.Combine(DtoFolder, string.Format("{0}ListDto.cs", BusinessName));
            string createdto = Const.OutputPath + Path.Combine(DtoFolder, string.Format("Create{0}Input.cs", BusinessName));

            string entityfile = Const.OutputPath + Path.Combine(EntityFolder, string.Format("{0}.cs", BusinessName));

            string[] tempString = { "[BusinessName]", "[businessName]", "[AppName]" }, newString = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            CreateFileByReplaceKeyFromTemplateFile(tempString, newString, Const.pathAppServiceFile, appserver);

            string[] tempString2 = { "[BusinessName]", "[businessName]", "[AppName]" }, newString2 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            CreateFileByReplaceKeyFromTemplateFile(tempString2, newString2, Const.pathIAppServiceFile, iappserver);


            string[] tempString3 = { "[BusinessName]", "[businessName]", "[AppName]" }, newString3 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            CreateFileByReplaceKeyFromTemplateFile(tempString3, newString3, Const.pathDtoListFile, dtolist);

            //string[] tempString4 = { "[BusinessName]", "[businessName]", "[AppName]" }, newString4 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            //CreateFileByReplaceKeyFromTemplateFile(tempString4, newString4, Const.pathCreateDtoFile, dtolist);

            string[] tempString5 = { "[BusinessName]", "[businessName]", "[AppName]" }, newString5 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            CreateFileByReplaceKeyFromTemplateFile(tempString5, newString5, Const.pathEntityFile, entityfile);

            DirectoryHelper.Open(Const.OutputPath);

            GenerationParameter parameter = new GenerationParameter(
                ModelManager.Clone(),
                GenerationHelper.GetGenerationObjects(this.dbTreeView),
                this.GetGenerationSettings());
            try
            {
                this.codeGeneration.GenerateAsync(parameter, Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                //logger.Error(Resources.GenerateFailure, ex);
            }
        }

        string language = "C#";
        string templateEngine = "NVelocity";
        string packagename = "SunRoseWMS";
        string author = "Ronaldosk Ye";
        string version = "1.0";
        string codeFileEncoding = "UTF-8";


        private GenerationSettings GetGenerationSettings()
        {
            GenerationSettings settings = new GenerationSettings(language,templateEngine, packagename, this.prefixTxtBox.Text,author, version,
                new string[] { "default" } ,
                codeFileEncoding, this.checkBox2.Checked, this.checkBox1.Checked);
            return settings;
        }

        private void CreateFileByReplaceKeyFromTemplateFile(string[] tempString, string[] newString, string templateFile, string destinationFile)
        {
            string contents = File.ReadAllText(templateFile);
            for (int i = 0; i < tempString.Length; i++)
                contents = contents.Replace(tempString[i], newString[i]);
            if (File.Exists(destinationFile))
                File.Delete(destinationFile);
            if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
            File.WriteAllText(destinationFile, contents);
        }
    }
}
