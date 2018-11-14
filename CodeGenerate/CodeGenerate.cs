using CodeBuilder.Configuration;
using CodeBuilder.PhysicalDataModel;
using CodeBuilder.TemplateEngine;
using CodeBuilder.TypeMapping;
using CodeBuilder.WinForm.UI;
using CodeGenerate.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YUtils;
using YUtils.Core;

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
            toolStripStatusLabel1.Text = businessname;
            if (checkBox2.Checked)
                businessname = businessname.IgnorePrefix(this.prefixTxtBox.Text, ',', '，');
            this.textBox1.Text = businessname;
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

            string localizationdefaultfile = Path.Combine(Const.LocalizationFolder, $@"{BusinessName}.xml");

            string[] tempString = { Const.businessName, Const.businessNameLow, Const.appName, }, newString = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString, newString, Const.pathAppServiceFile, appserver);

            string[] tempString2 = { Const.businessName, Const.businessNameLow, Const.appName, }, newString2 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString2, newString2, Const.pathIAppServiceFile, iappserver);


            string entities = "",UIWords = "<text name=\"Projects\">Projects</text>";
            GenerationHelper.GenerateCode(out entities, this.dbTreeView, new string[] { "default" }, this.prefixTxtBox.Text, this.checkBox2.Checked, this.checkBox1.Checked);

            string[] tempString3 = { Const.businessName, Const.businessNameLow, Const.appName, Const.memberlist }, newString3 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename, entities };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString3, newString3, Const.pathDtoListFile, dtolist);

            string[] tempString4 = { Const.businessName, Const.businessNameLow, Const.appName, Const.memberlist }, newString4 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename, entities };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString4, newString4, Const.pathCreateDtoFile, createdto);

            string[] tempString5 = { Const.businessName, Const.businessNameLow, Const.appName, Const.memberlist }, newString5 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename, entities };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString5, newString5, Const.pathEntityFile, entityfile);

            string[] tempString6 = { Const.businessName, Const.businessNameLow, Const.appName, Const.localizations }, newString6 = { BusinessName, BusinessName.LowerCamelCaseName(), packagename, UIWords };
            FileHelper.CreateFileByReplaceKeyFromTemplateFile(tempString6, newString6, Const.pathLocalizationsFile, localizationdefaultfile);

            DirectoryHelper.Open(Const.OutputPath);
            
            try
            {
                //var generationObjects = GenerationHelper.GetGenerationObjects(this.dbTreeView);
                //int genObjectCount = generationObjects.Sum(x => x.Value.Count);
                //if (genObjectCount == 0)
                //{
                //    MessageBoxHelper.DisplayInfo("You should checked a tables or views treenode");
                //    return;
                //}
                //this.genProgressBar.Maximum = genObjectCount * 4;//划重点，这里的4是根据CreateFileByReplaceKeyFromTemplateFile调用次数相关的
                
                //GenerationParameter parameter = new GenerationParameter(
                //    ModelManager.Clone(),
                //    GenerationHelper.GetGenerationObjects(this.dbTreeView),
                //    this.GetGenerationSettings());
                
                //this.codeGeneration.GenerateAsync(parameter, Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                //logger.Error(Resources.GenerateFailure, ex);
            }
        }

        string language = "C#";
        string templateEngine = "default";
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
        
        private void codeGeneration_ProgressChanged(GenerationProgressChangedEventArgs args)
        {
            this.genProgressBar.Value = args.ProgressPercentage;
        }
    }
}
