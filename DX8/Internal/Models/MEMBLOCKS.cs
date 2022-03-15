// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System;
using System.Runtime.InteropServices;

namespace DX8.Internal.Models
{
    /// <summary>
    /// This whole situation is about how to block out a size of memory in a structure in C# in a managed sense.
    /// As well as sizeof(...) block structures being a problem as well.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal static class MEMBLOCK
    {
        public const int DDSURFACEDESC = 120; //sizeof(DDSURFACEDESC);
        public const int DIJOYSTATE2 = 272; //sizeof(DIJOYSTATE2);
        public const int DIPROPHEADER = 16; //sizeof(DIPROPHEADER);
        public const int DIPROPRANGE = 24; //sizeof(DIPROPRANGE);
        public const int DSBUFFERDESC = 40; //sizeof(DSBUFFERDESC);
    }

    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    internal struct BYTE8
    {
        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return B0;
                    case 1: return B1;
                    case 2: return B2;
                    case 3: return B3;
                    case 4: return B4;
                    case 5: return B5;
                    case 6: return B6;
                    case 7: return B7;
                }

                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (index)
                {
                    case 0: B0 = value; break;
                    case 1: B1 = value; break;
                    case 2: B2 = value; break;
                    case 3: B3 = value; break;
                    case 4: B4 = value; break;
                    case 5: B5 = value; break;
                    case 6: B6 = value; break;
                    case 7: B7 = value; break;
                }

            }
        }

        [FieldOffset(0)]
        private byte B0;

        [FieldOffset(1)]
        private byte B1;

        [FieldOffset(2)]
        private byte B2;

        [FieldOffset(3)]
        private byte B3;

        [FieldOffset(4)]
        private byte B4;

        [FieldOffset(5)]
        private byte B5;

        [FieldOffset(6)]
        private byte B6;

        [FieldOffset(7)]
        private byte B7;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    internal struct INT2
    {
        [FieldOffset(0)]
        public int I0;

        [FieldOffset(1)]
        public int I1;
    }

    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    internal struct UINT4
    {
        [FieldOffset(0)]
        public uint UI0;
        
        [FieldOffset(4)]
        public uint UI1;
        
        [FieldOffset(8)]
        public uint UI2;

        [FieldOffset(12)]
        public uint UI3;
    }
}
