using System;
using System.Text;
using System.IO;
using System.Reflection;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity.Util.Introspection;

namespace CodeBuilder.TemplateEngine
{
    using Util;
    using YUtils;
    using YUtils.Core;

    public class DefaultEngineAdapter : ITemplateEngine
    {
        private static Logger logger = InternalTrace.GetLogger(typeof(NVelocityEngineAdapter));

        public DefaultEngineAdapter()
        {
        }

        public bool Run(TemplateData templateData)
        {
            try
            {
                string loaderPath = Path.GetDirectoryName(templateData.TemplateFileName);
                string templateFile = Path.GetFileName(templateData.TemplateFileName);

                Encoding encoding = Encoding.GetEncoding(templateData.Encoding);
                if (templateData.Encoding == "UTF-8")
                {
                    encoding = new UTF8Encoding(false);
                }
                //To do
                MessageBoxHelper.Display("templateFile");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("DefaultAdapter:{0}", templateData.CodeFileName), ex);
                return false;
            }
        }
    }
    
}
