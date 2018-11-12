using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUtils.Core
{
    public class FileHelper
    {
        public static void CreateFileByReplaceKeyFromTemplateFile(string[] tempString, string[] newString, string templateFile, string destinationFile)
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
