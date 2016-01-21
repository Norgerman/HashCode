using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CRC32
{
    public class CRC32Provider
    {
        private const uint Polynomial = 0xEDB88320;

        private uint[,] CRC32Table;

        private bool complete;

        private uint hash;

        public uint Hash
        {
            get
            {
                if (complete)
                    return hash;
                else
                    throw new CryptographicUnexpectedOperationException();
            }
        }

        public CRC32Provider()
        {
            Initialize();
            InitCRC32Table();
        }

        public void TransformBlock(byte[] inputBuffer, int offset, long count)
        {
            long len = count;
            uint crc = ~hash;
            int i = offset;
            while (len >= 8)
            {
                uint one = BitConverter.ToUInt32(inputBuffer, i) ^ crc;
                i += 4;
                uint two = BitConverter.ToUInt32(inputBuffer, i);
                i += 4;

                crc = CRC32Table[7, one & 0xFF] ^
                      CRC32Table[6, (one >> 8) & 0xFF] ^
                      CRC32Table[5, (one >> 16) & 0xFF] ^
                      CRC32Table[4, one >> 24] ^
                      CRC32Table[3, two & 0xFF] ^
                      CRC32Table[2, (two >> 8) & 0xFF] ^
                      CRC32Table[1, (two >> 16) & 0xFF] ^
                      CRC32Table[0, two >> 24];

                len -= 8;
            }

            while (i < count)
            {
                crc = (crc >> 8) ^ CRC32Table[0, (crc & 0xFF) ^ inputBuffer[i]];
                i++;
            }

            hash = ~crc;
        }

        public void TransformFinalBlock(byte[] inputBuffer, int offset, long count)
        {
            TransformBlock(inputBuffer, offset, count);
            complete = true;
        }

        public void Initialize()
        {
            hash = 0x0;
            complete = false;
        }

        private void InitCRC32Table()
        {
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
