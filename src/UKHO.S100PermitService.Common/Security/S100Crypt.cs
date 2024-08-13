using System.Security.Cryptography;

namespace UKHO.S100PermitService.Common.Security
{
    public class S100Crypt : IS100Crypt
    {
        private static readonly int KeySize = 128;
        public static readonly int KeySizeEncoded = KeySize / 4;

        protected readonly byte[] Key;
        private readonly byte[] Iv;

        public S100Crypt()
        {
            using var keyGen = new AesCryptoServiceProvider();
            keyGen.KeySize = KeySize;
            keyGen.GenerateKey();
            Key = keyGen.Key;
        }

        public S100Crypt(string keyHexEncoded)
        {
            if (keyHexEncoded.Length != KeySizeEncoded)
            {
                throw new ArgumentException($"Expected encoded key length {KeySizeEncoded}, not {keyHexEncoded.Length}.");
            }

            Key = StringToByteArray(keyHexEncoded);
        }

        public byte[] Decrypt(string encrypted)
        {
            byte[] encryptedByte = StringToByteArray(encrypted);
            // Implement decryption logic using Aes class
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                using ICryptoTransform decryptor = aes.CreateDecryptor();
                return decryptor.TransformFinalBlock(encryptedByte, 0, encryptedByte.Length);
            }
        }

        private static byte[] StringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }

        public byte[] Encrypt(byte[] unencrypted)
        {
            using (var ms = new MemoryStream())
            {
                var cs = Encrypt(ms);
                cs.Write(unencrypted, 0, unencrypted.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        private CryptoStream Encrypt(Stream stream)
        {
            var cipher = new AesCryptoServiceProvider
            {
                Key = Key,
                IV = Iv,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            var encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV);
            return new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
        }
    }
}
