using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CodeBuilder.PhysicalDataModel;
using CodeBuilder.Configuration;
using CodeBuilder.TemplateEngine;

namespace CodeBuilder.WinForm.UI
{
    public class GenerationHelper
    {
        public static bool IsValidPackageName(string packageName)
        {
            if (string.IsNullOrEmpty(packageName) ||
                packageName.Trim().Length == 0) return false;

            return Regex.IsMatch(packageName, "[a-zA-Z]+");
        }

        public static Dictionary<string, List<string>> GetGenerationObjects(TreeView treeView)
        {
            Dictionary<string, List<string>> generationObjects = new Dictionary<string, List<string>>();
            foreach (TreeNode parentNode in treeView.Nodes)
            {
                generationObjects.Add(parentNode.Index.ToString(), GetCheckedTags(parentNode.Nodes));
            }

            return generationObjects;
        }

        public static List<String> GetCheckedTags(TreeNodeCollection nodes)
        {
            List<string> tags = new List<string>();
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag != null)
                {
                    if (node.Tag is BaseTable)
                    {
                        BaseTable cols = node.Tag as BaseTable;
                        tags.Add(cols.DisplayName);
                    }
                    else
                        tags.Add(node.Tag.ToString());
                }
                GetCheckedTags(node.Nodes, tags);
            }
            return tags;
        }

        private static void GetCheckedTags(TreeNodeCollection nodes, List<String> tags)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag != null)
                {
                    if (node.Tag is BaseTable)
                    {
                        BaseTable cols = node.Tag as BaseTable;
                        tags.Add(cols.DisplayName);
                    }
                    else
                        tags.Add(node.Tag.ToString());
                }
                GetCheckedTags(node.Nodes, tags);
            }
        }

        public static void GenerateCode(out string entitys,TreeView treeView, string[] templateNames,string tablePrefix, bool isOmitTablePrefix, bool isCamelCaseName)
        {
            entitys = "";
            GenerationParameter parameter = new GenerationParameter(
                   ModelManager.Clone(),
                   GetGenerationObjects(treeView),
                   GetGenerationSettings(templateNames,tablePrefix, isOmitTablePrefix, isCamelCaseName));

            foreach (string modelId in parameter.GenerationObjects.Keys)
            {
                foreach (string objId in parameter.GenerationObjects[modelId])
                {
                    IMetaData modelObject = ModelManager.GetModelObject(parameter.Models[modelId], objId);
                    TemplateData templateData = TemplateDataBuilder.Build(modelObject, parameter.Settings,
                            "default", parameter.Models[modelId].Database, modelId);

                    if (templateData == null ) throw new ArgumentNullException("Can not create template data!");
                    string currentCodeFileName = templateData == null ? string.Empty : templateData.CodeFileName;

                    if (modelObject is Table)
                    {
                        Table table = modelObject as Table;
                        foreach (var column in table.Columns.Values)
                        {
                            string langType = column.LanguageType;
                            string defaultValue = column.LanguageDefaultValue;
                            string typeAlias = column.LanguageTypeAlias;

                            entitys += string.Format("public {0} {1} {{get;set;}}\r\n\r\n\t\t", langType, column.DisplayName);
                        }
                    }
                    else if (modelObject is PhysicalDataModel.View)
                    {
                        PhysicalDataModel.View view = modelObject as PhysicalDataModel.View;
                        foreach (var column in view.Columns.Values)
                        {
                            string langType = column.LanguageType;
                            string defaultValue = column.LanguageDefaultValue;
                            string typeAlias = column.LanguageTypeAlias;
                            entitys += string.Format("public {0} {1} {{get;set;}}}\r\n\r\n\t\t", langType, column.DisplayName);
                        }
                    }
                }
            }
        }

        static string language = "C#";
        static string templateEngine = "default";
        static string packagename = "SunRoseWMS";
        static string author = "Ronaldosk Ye";
        static string version = "1.0";
        static string codeFileEncoding = "UTF-8";


        private static GenerationSettings GetGenerationSettings(string[] templateNames,string tablePrefix ,bool isOmitTablePrefix, bool isCamelCaseName)
        {
            GenerationSettings settings = new GenerationSettings(language, templateEngine, packagename, tablePrefix, author, version,
                templateNames ,codeFileEncoding, isOmitTablePrefix, isCamelCaseName);
            return settings;
        }
    }
}
