using System.Security.Cryptography;

namespace UKHO.S100PermitService.Common.Helpers
{
    public class CRC32Helper
    {
        public static string Crc32String(byte[] data)
        {
            return Crc32(data).ToString("X");
        }

        public static uint Crc32(byte[] data)
        {
            using (var crc = new CRC32())
            {
                return BitConverter.ToUInt32(crc.ComputeHash(data), 0);
            }
        }
    }

    // CRC32 implementation
    public class CRC32 : HashAlgorithm
    {
        private const uint Polynomial = 0xEDB88320;
        private readonly uint[] table;
        private uint value = 0xFFFFFFFF;

        public CRC32()
        {
            table = GenerateTable();
        }

        public override void Initialize()
        {
            value = 0xFFFFFFFF;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < ibStart + cbSize; i++)
            {
                value = (value >> 8) ^ table[(value & 0xFF) ^ array[i]];
            }
        }

        protected override byte[] HashFinal()
        {
            value ^= 0xFFFFFFFF;
            return BitConverter.GetBytes(value);
        }

        private uint[] GenerateTable()
        {
            uint[] table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint entry = i;
                for (int j = 0; j < 8; j++)
                {
                    entry = (entry & 1) == 1 ? (entry >> 1) ^ Polynomial : entry >> 1;
                }
                table[i] = entry;
            }
            return table;
        }
    }
}
