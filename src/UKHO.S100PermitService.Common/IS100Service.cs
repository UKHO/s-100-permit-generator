using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.Common
{
    public interface IS100Service
    {
        string GetDecryptedHwdId(string upn, string mid, string key);
        string GetEncryptedDataKey(string hwId, string dataKey, string fileName, int edtn);
        string GetUserPermitNumber(string mId, string mKey, string hwId);
    }
}
