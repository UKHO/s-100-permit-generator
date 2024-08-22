using ICSharpCode.SharpZipLib.Checksum;
using System.Text;

namespace UKHO.S100PermitService.Common.Helpers
{
    public static class Crc32Helper
    {
        public static string Crc32String(string hwIdEncrypted)
        {
            var crc = new Crc32();
            crc.Update(Encoding.UTF8.GetBytes(hwIdEncrypted));
            return crc.Value.ToString("X8");
        }
    }
}
