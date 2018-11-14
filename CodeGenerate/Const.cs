using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUtils.Core;

namespace CodeGenerate.Template
{
    public class Const
    {
        /// <summary>
        /// [AppName]
        /// </summary>
        public static string appName = "[AppName]";//namespace 
        /// <summary>
        /// [BusinessName]
        /// </summary>
        public static string businessName = "[BusinessName]";
        /// <summary>
        /// [businessName] business实例的名称，规则是首字母小写
        /// </summary>
        public static string businessNameLow = "[businessName]";
        /// <summary>
        /// [MemberList]
        /// </summary>
        public static string memberlist = "[MemberList]";
        /// <summary>
        /// [NewWords]
        /// </summary>
        public static string localizations = "[NewWords]";

        public static string AppPath = AssemblyHelper.GetDirectoryName(typeof(Const));
        public static string OutputPath = $@"{AppPath}\Output";
        public static string AppTempFolder = @"\Template\Application";
        public static string DtoTempFolder = @"\Template\Application\Dtos";
        public static string CoreTempFolder = @"\Template\Core";
        public static string EntityTempFolder = @"\Template\EntityFrameworCore";

        public static string LocalizationFolder = $@"{OutputPath}\Core\Localization\SourceFiles";

        public static string AppServiceFile = "BusinessNameAppService.svr";
        public static string IAppServiceFile = "IBusinessNameAppService.isvr";
        public static string EntityFile = "BusinessName.entity";
        public static string ListDtoFile = "BusinessNameListDto.dto";
        public static string CreateFile = "CreateBusinessNameInput.create";
        public static string LocalizationsFile = "default.localizations";


        public static string pathAppServiceFile = AppPath + Path.Combine(AppTempFolder, AppServiceFile);
        public static string pathIAppServiceFile = AppPath + Path.Combine(AppTempFolder, IAppServiceFile);
        public static string pathEntityFile = AppPath + Path.Combine(CoreTempFolder, EntityFile);
        public static string pathDtoListFile = AppPath + Path.Combine(DtoTempFolder, ListDtoFile);
        public static string pathCreateDtoFile = AppPath + Path.Combine(DtoTempFolder, CreateFile);
        public static string pathLocalizationsFile = AppPath + Path.Combine(CoreTempFolder, LocalizationsFile);




    }
}
