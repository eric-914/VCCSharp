// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// This whole situation is about how to block out a size of memory in a structure in C# in a managed sense.
    /// As well as sizeof(...) block structures being a problem as well.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static class MEMBLOCK
    {
        public static unsafe int DDSURFACEDESC => sizeof(DDSURFACEDESC);
        public static unsafe int DIJOYSTATE2 => sizeof(DIJOYSTATE2);
        public static unsafe int DIPROPHEADER => sizeof(DIPROPHEADER);
        public static unsafe int DIPROPRANGE => sizeof(DIPROPRANGE);
        public static unsafe int DSBUFFERDESC => sizeof(DSBUFFERDESC);
    }

    [StructLayout(LayoutKind.Explicit, Size = 260, CharSet = CharSet.Ansi)]
    public struct BYTE260
    {
        public unsafe byte this[int index]
        {
            get => B[index];
            set => B[index] = value;
        }

        [FieldOffset(0)]
        public unsafe fixed byte B[260];
    }

    [StructLayout(LayoutKind.Explicit, Size = 128, CharSet = CharSet.Ansi)]
    public struct BYTE128
    {
        public unsafe byte this[int index]
        {
            get => B[index];
            set => B[index] = value;
        }

        [FieldOffset(0)]
        public unsafe fixed byte B[128];
    }

    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct BYTE8
    {
        public unsafe byte this[int index]
        {
            get => B[index];
            set => B[index] = value;
        }

        [FieldOffset(0)]
        public unsafe fixed byte B[8];
    }

    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct INT2
    {
        public unsafe int this[int index]
        {
            get => I[index];
            set => I[index] = value;
        }

        [FieldOffset(0)]
        public unsafe fixed int I[2];
    }

    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    public struct UINT4
    {
        public unsafe uint this[int index]
        {
            get => UI[index];
            set => UI[index] = value;
        }

        [FieldOffset(0)]
        public unsafe fixed uint UI[4];
    }
}