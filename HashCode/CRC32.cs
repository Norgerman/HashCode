using System;
using System.Security.Cryptography;

namespace Norgerman.Hash
{
    public class CRC32 : HashAlgorithm
    {
        private const uint Polynomial = 0xEDB88320;

        static private uint[,] CRC32Table;

        private uint hash;

        public override int HashSize
        {
            get
            {
                return 32;
            }
        }

        static CRC32()
        {
            InitCRC32Table();
        }

        public CRC32()
        {
            Initialize();
        }

        public override void Initialize()
        {
            hash = 0x0;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int len = cbSize;
            uint crc = ~hash;
            int i = ibStart;
            while (len >= 8)
            {
                uint one = BitConverter.ToUInt32(array, i) ^ crc;
                i += 4;
                uint two = BitConverter.ToUInt32(array, i);
                i += 4;
                unchecked
                {
                    crc = CRC32Table[7, one & 0xFF] ^
                        CRC32Table[6, (one >> 8) & 0xFF] ^
                        CRC32Table[5, (one >> 16) & 0xFF] ^
                        CRC32Table[4, one >> 24] ^
                        CRC32Table[3, two & 0xFF] ^
                        CRC32Table[2, (two >> 8) & 0xFF] ^
                        CRC32Table[1, (two >> 16) & 0xFF] ^
                        CRC32Table[0, two >> 24];
                }

                len -= 8;
            }

            while (i < cbSize)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ CRC32Table[0, (crc & 0xFF) ^ array[i]];
                    i++;
                }
            }

            hash = ~crc;
        }

        protected override byte[] HashFinal()
        {
            return new byte[]
            {
                (byte)((hash >> 24) & 0xff),
                (byte)((hash >> 16) & 0xff),
                (byte)((hash >> 8) & 0xff),
                (byte)(hash & 0xff)
            };
        }

        private static void InitCRC32Table()
        {
            if (CRC32Table != null)
                return;
            CRC32Table = new uint[8, 256];
            for (uint i = 0; i <= 0xFF; i++)
            {
                uint crc = i;
                for (uint j = 0; j < 8; j++)
                {
                    crc = (crc >> 1) ^ ((crc & 1) * Polynomial);
                }
                CRC32Table[0, i] = crc;
            }

            for (uint i = 0; i <= 0xFF; i++)
            {
                CRC32Table[1, i] = (CRC32Table[0, i] >> 8) ^ CRC32Table[0, CRC32Table[0, i] & 0xFF];
                CRC32Table[2, i] = (CRC32Table[1, i] >> 8) ^ CRC32Table[0, CRC32Table[1, i] & 0xFF];
                CRC32Table[3, i] = (CRC32Table[2, i] >> 8) ^ CRC32Table[0, CRC32Table[2, i] & 0xFF];
                CRC32Table[4, i] = (CRC32Table[3, i] >> 8) ^ CRC32Table[0, CRC32Table[3, i] & 0xFF];
                CRC32Table[5, i] = (CRC32Table[4, i] >> 8) ^ CRC32Table[0, CRC32Table[4, i] & 0xFF];
                CRC32Table[6, i] = (CRC32Table[5, i] >> 8) ^ CRC32Table[0, CRC32Table[5, i] & 0xFF];
                CRC32Table[7, i] = (CRC32Table[6, i] >> 8) ^ CRC32Table[0, CRC32Table[6, i] & 0xFF];
            }
        }
    }
}
