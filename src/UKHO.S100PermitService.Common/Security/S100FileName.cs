using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UKHO.S100PermitService.Common.Helpers;

namespace UKHO.S100PermitService.Common.Security
{
    public class S100FileName
    {
        private const string PATTERN_1 = @"^([0-9]{3})([A-Z0-9]{4}).*";
        private const string PATTERN_2 = @"^S([0-9]{3})([A-Z0-9]{2}).*";

        public static int? StandardNumber(string fileName)
        {
            if (fileName == null)
            {
                return null;
            }
            fileName = FileUtils.GetBaseName(fileName);
            if (fileName.Length < 5)
            {
                return null;
            }

            if (Regex.IsMatch(fileName, PATTERN_1))
            {
                return int.Parse(Regex.Replace(fileName, PATTERN_1, "$1"));
            }

            if (Regex.IsMatch(fileName, PATTERN_2))
            {
                return int.Parse(Regex.Replace(fileName, PATTERN_2, "$1"));
            }

            // earlier S-101 version in Caris test data. Sorry, this is not good.
            string fileNameWithoutSuffix = FileUtils.GetFileNameWithoutSuffix(fileName);
            if (fileNameWithoutSuffix.Length == 10)
            {
                return 101;
            }

            return null;
        }
    }
}
