using ICSharpCode.SharpZipLib.Checksum;
using System.Text;
using UKHO.S100PermitService.Common.Security;
namespace UKHO.S100PermitService.Common.Services
{
    public class S100Service : IS100Service
    {
        public S100Service()
        {

        }

        public string GetDecryptedHwdId(string upn, string mid, string key)
        {
            S100Manufacturer s100Manufacturer = new(mid, key);

            return s100Manufacturer.Decrypt(upn[..32]);
        }

        public string GetEncryptedDataKey(string hwId, string dataKey, string fileName, int edtn)
        {
            S100DataPermit dp = new(fileName, edtn, new DateTime(), new S100ProductSpecification(101));

            dp.Create(dataKey, hwId);

            return dp.GetEncryptedDataKey();
        }

        public string GetUserPermitNumber(string mId, string mKey, string hwId)
        {
            S100Manufacturer manufacturer = new(mId, mKey);

            string hwIdEncrypted = manufacturer.Encrypt(hwId);

            var crc = new Crc32();
            crc.Update(Encoding.UTF8.GetBytes(hwIdEncrypted));
            var calculatedCrc = crc.Value.ToString("X8");

            var upn = hwIdEncrypted + calculatedCrc + mId;
            return upn;
        }

    }
}
