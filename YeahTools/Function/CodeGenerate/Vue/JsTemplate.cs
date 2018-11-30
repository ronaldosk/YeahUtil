using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUtils;

namespace YeahTools.Function.CodeGenerate.Vue
{
    /// <summary>
    /// JS模板类
    /// i_前缀代表输入参数，o_前缀代表输出参数，c_前缀代表‘常量’参数
    /// </summary>
    public class JsTemplateHelper
    {
        #region ModuleRouteJs 应用模块的路由JS 模板
        public static string i_AreaName = "SampleModule";

        public static string i_routePath = "/table";
        public static string i_titleName = "项目";
        
        //应用模块的路由文件名
        public static string o_JsFileName = $"{i_AreaName.LowerCamelCaseName()}.js";
        //*******index.js文件中要添加的内容
        //引用包的字符串
        public static string o_ImportString = $"import projectMgrRouter from './modules/{i_AreaName.LowerCamelCaseName()}'";
        //右边栏菜单定义
        public static string o_ModuleString = $"{i_AreaName.LowerCamelCaseName()},";
        //*******end
        static string c_AreaRouteJs = string.Format(@"/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout'

const {0}Router = {{
  path: '{1}',
  component: Layout,
  redirect: '/table/complex-table',
  name: '{2}',
  meta: {{
    title: '{3}管理',
    icon: 'table'
  }},
  children: [
    {{
      path: 'create',
      component: () => import('@/views/{1}/create'),
      name: 'CreateArticle',
      meta: {{ title: '新建{3}', icon: 'edit' }}
    }},
    {{
      path: 'list',
      component: () => import('@/views/{1}/complexTable'),
      name: 'ComplexTable',
      meta: {{ title: '{3}列表', icon: 'list' }}
    }}
  ]
}}
export default {0}Router", i_AreaName.LowerCamelCaseName(), i_routePath, i_AreaName.CamelCaseName(), i_titleName);
    }
    #endregion
}
