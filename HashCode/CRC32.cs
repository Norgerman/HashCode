using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace Norgerman.Hash
{
    interface ICRC32
    {
        uint Crc32(uint crc, Span<byte> data);
    }

    class CRC32Software : ICRC32
    {
        private uint[,]? CRC32Table;

        internal CRC32Software(uint polynomial)
        {
            InitCRC32Table(polynomial);
        }

        public unsafe uint Crc32(uint crc, Span<byte> data)
        {
            crc = ~crc;
            var len = data.Length;
            var i = 0;
            fixed (byte* bptr = data)
            {
                var current = (uint*)bptr;
                while (len >= 8)
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        var one = *current++ ^ crc;
                        var two = *current++;
                        unchecked
                        {
                            crc = CRC32Table![7, one & 0xFF] ^
                                CRC32Table[6, (one >> 8) & 0xFF] ^
                                CRC32Table[5, (one >> 16) & 0xFF] ^
                                CRC32Table[4, one >> 24] ^
                                CRC32Table[3, two & 0xFF] ^
                                CRC32Table[2, (two >> 8) & 0xFF] ^
                                CRC32Table[1, (two >> 16) & 0xFF] ^
                                CRC32Table[0, two >> 24];
                        }
                    }
                    else
                    {
                        var one = *current++ ^ Swap(crc);
                        var two = *current++;
                        unchecked
                        {
                            crc = CRC32Table![0, two & 0xFF] ^
                               CRC32Table[1, (two >> 8) & 0xFF] ^
                               CRC32Table[2, (two >> 16) & 0xFF] ^
                               CRC32Table[3, (two >> 24) & 0xFF] ^
                               CRC32Table[4, one & 0xFF] ^
                               CRC32Table[5, (one >> 8) & 0xFF] ^
                               CRC32Table[6, (one >> 16) & 0xFF] ^
                               CRC32Table[7, (one >> 24) & 0xFF];
                        }
                    }

                    len -= 8;
                    i += 8;
                }
            }

            while (len > 0)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ CRC32Table![0, (crc & 0xFF) ^ data[i]];
                    i++;
                    len--;
                }
            }
            return ~crc;
        }

        private static uint Swap(uint x)
        {
            return (x >> 24) |
                ((x >> 8) & 0x0000FF00) |
                ((x << 8) & 0x00FF0000) |
                (x << 24);
        }

        private void InitCRC32Table(uint polynomial)
        {
            if (CRC32Table != null)
                return;
            CRC32Table = new uint[8, 256];
            for (uint i = 0; i <= 0xFF; i++)
            {
                uint crc = i;
                for (uint j = 0; j < 8; j++)
                {
                    crc = (crc >> 1) ^ ((crc & 1) * polynomial);
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

    class CRC32CSSE : ICRC32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe uint Crc32(uint crc, Span<byte> data)
        {
            crc = ~crc;
            var len = data.Length;
            fixed (byte* bptr = data)
            {
                nint current = (nint)bptr;
                if (current % 4 == 0)
                {
                    while (len >= 4)
                    {
                        crc = Sse42.Crc32(crc, *((uint*)(current)));
                        current += 4;
                        len -= 4;
                    }
                }
                if (current % 2 == 0)
                {
                    while (len >= 2)
                    {
                        crc = Sse42.Crc32(crc, *((ushort*)(current)));
                        current += 2;
                        len -= 2;
                    }
                }
                while (len > 0)
                {
                    crc = Sse42.Crc32(crc, *((byte*)(current)));
                    current++;
                    len--;
                }
            }

            return ~crc;
        }
    }

    public class CRC32 : HashAlgorithm
    {
        private uint _hash;
        private ICRC32? _impl;
        private readonly bool _useISCSI;
        public CRC32(bool useISCSI)
        {
            HashSizeValue = 32;
            _useISCSI = useISCSI;
            Initialize();
        }

        public override void Initialize()
        {
            _hash = 0x0;

            if (Sse42.IsSupported && this._useISCSI)
            {
                _impl = new CRC32CSSE();
            }
            else
            {
                _impl = new CRC32Software(this._useISCSI ? 0x82F63B78 : 0xEDB88320);
            }
        }

        unsafe protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this._hash = _impl?.Crc32(this._hash, new Span<byte>(array, ibStart, cbSize)) ?? 0;
        }

        protected override byte[] HashFinal()
        {
            var bytes = BitConverter.GetBytes(_hash);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}
