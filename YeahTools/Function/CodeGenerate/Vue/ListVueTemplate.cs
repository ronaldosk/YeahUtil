using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTools.Function.CodeGenerate.Vue
{
    /// <summary>
    /// 列表 功能Vue文件模板类
    /// i_前缀代表输入参数，o_前缀代表输出参数，c_前缀代表‘常量’参数
    /// </summary>
    public class ListVueTemplateHelper
    {
        public List<string> input_com_list { get; set; }
    }


    public class VueUIModel
    {
        public string i_title;
        public string elinput =string.Format(@"<el-input :placeholder=""$t('table.title')"" v-model=""listQuery.title"" style =""width: 200px;"" class=""filter -item"" @keyup.enter.native=""handleFilter "" />"); 


    }
}
