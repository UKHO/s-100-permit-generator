using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.Common.Security
{
    public interface IS100DataPermit
    {
        S100DataPermit Create(string fileName, int edtn, DateTime permitEndDate, string dataKey, string hwId,
            S100ProductSpecification productSpecification);

        string GetEncryptedDataKey();
    }
}
