using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.Common.Cache
{
    public interface IManufactureCache
    {
        void CacheAllManufacture();
        string GetManufactureKey(string manufactureId);

    }
}
