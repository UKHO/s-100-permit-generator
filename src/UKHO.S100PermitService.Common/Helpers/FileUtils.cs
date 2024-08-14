using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.Common.Helpers
{
    public class FileUtils
    {
        /// <summary>
        /// Return the basename of the given filename.
        /// </summary>
        public static string GetBaseName(string fileNameWithPath)
        {
            string tmp = fileNameWithPath;
            int i = tmp.LastIndexOf('/');
            if (i == -1)
            {
                i = tmp.LastIndexOf('\\');
            }
            if (i != -1)
            {
                tmp = tmp.Substring(i + 1);
            }
            return tmp;
        }

        /// <summary>
        /// Return the basename of the given filename without suffix. "a/bc/file.txt" ->
        /// "file".
        /// </summary>
        public static string GetFileNameWithoutSuffix(string fileName)
        {
            string s = GetBaseName(fileName);
            int p = s.IndexOf('.');
            if (p > 0)
            {
                s = s.Substring(0, p);
            }
            return s;
        }
    }
}
