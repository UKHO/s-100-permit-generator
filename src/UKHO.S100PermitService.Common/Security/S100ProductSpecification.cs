using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.Common.Security
{
    public class S100ProductSpecification 
    {
        private readonly int nr;

        public S100ProductSpecification(int nr)
        {
            if (nr < 100 || nr > 999)
            {
                throw new ArgumentException("Illegal S-100 product specification number: " + nr);
            }
            this.nr = nr;
        }

        public S100ProductSpecification(string s)
        {
            try
            {
                if (Regex.IsMatch(s, "^[0-9]+$"))
                {
                    this.nr = int.Parse(s);
                }
                else if (s.Length == 4 && s.StartsWith("S"))
                {
                    this.nr = int.Parse(s.Substring(1));
                }
                else if (s.Length == 5 && s.StartsWith("S-"))
                {
                    this.nr = int.Parse(s.Substring(2));
                }
                else
                {
                    int? snr = S100FileName.StandardNumber(s);
                    if (snr == null)
                    {
                        string pattern = ".*S-([0-9]{3}).*";
                        if (Regex.IsMatch(s, pattern))
                        {
                            snr = int.Parse(Regex.Replace(s, pattern, "$1"));
                        }
                    }
                    if (snr == null)
                    {
                        throw new ArgumentException("Could not extract S-100 product specification number from: " + s);
                    }
                    this.nr = snr.Value;
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("Could not extract S-100 product specification number from: " + s);
            }
        }


    }

}
