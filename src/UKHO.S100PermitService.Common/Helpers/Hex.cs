using System.Text;

namespace UKHO.S100PermitService.Common.Helpers
{
    public static class Hex
    {
        public static string ToString(byte[] data)
        {
            StringBuilder hex = new(data.Length * 2);
            foreach (byte b in data)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        public static byte[] FromString(string encoded)
        {
            int numberChars = encoded.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(encoded.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
