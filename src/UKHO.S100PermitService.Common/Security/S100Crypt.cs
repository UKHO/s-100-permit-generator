using System.Security.Cryptography;

namespace UKHO.S100PermitService.Common.Security
{
    public class S100Crypt
    {
        private static readonly int KeySize = 128;
        public static readonly int KeySizeEncoded = KeySize / 4;

        protected readonly Aes Key;

        public S100Crypt(string keyHexEncoded)
        {
            if (keyHexEncoded.Length != KeySizeEncoded)
            {
                throw new ArgumentException($"Expected encoded key length {KeySizeEncoded}, not {keyHexEncoded.Length}.");
            }
            Key = Aes.Create();
            Key.KeySize = KeySize;
            Key.GenerateKey();
            Key!.Key = StringToByteArray(keyHexEncoded);
        }

        public byte[] Decrypt(string encrypted)
        {
            byte[] encryptedByte = StringToByteArray(encrypted);
            // Implement decryption logic using Aes class

            Key.Mode = CipherMode.ECB;
            Key.Padding = PaddingMode.None;
            using ICryptoTransform decryptor = Key.CreateDecryptor();
            return decryptor.TransformFinalBlock(encryptedByte, 0, encryptedByte.Length);
        }

        public byte[] Encrypt(string encrypted)
        {
            byte[] encryptedByte = StringToByteArray(encrypted);
            // Implement decryption logic using Aes class
            Key.Mode = CipherMode.ECB;
            Key.Padding = PaddingMode.None;
            using ICryptoTransform encypt = Key.CreateEncryptor();
            return encypt.TransformFinalBlock(encryptedByte, 0, encryptedByte.Length);
        }

        private byte[] StringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
