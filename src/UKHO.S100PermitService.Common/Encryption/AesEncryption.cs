﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace UKHO.S100PermitService.Common.Encryption
{
    [ExcludeFromCodeCoverage]
    public class AesEncryption : IAesEncryption
    {
        private static readonly int _keySize = 128;
        private static readonly int _iv_Length = 16;
        protected readonly Aes aes;

        public AesEncryption()
        {
            using var aes = Aes.Create();
            aes.BlockSize = _keySize;
            aes.KeySize = _keySize;
            aes.IV = new byte[_iv_Length];
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
        }

        public string Decrypt(string hexString, string keyHexEncoded)
        {
            aes.Key = StringToByteArray(keyHexEncoded);

            // decryption
            var encryptedByte = StringToByteArray(hexString);

            using var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            var decryptedText = decrypt.TransformFinalBlock(encryptedByte, 0, encryptedByte.Length);

            return BitConverter.ToString((decryptedText)).Replace("-", "");
        }

        private static byte[] StringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}