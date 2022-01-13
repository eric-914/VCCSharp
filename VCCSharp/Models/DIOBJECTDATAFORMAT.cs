﻿using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 24, CharSet = CharSet.Ansi)]
    public struct DIOBJECTDATAFORMAT
    {
        [FieldOffset(0)] //--Length=8
        public unsafe _GUID* pguid;

        //--What is hiding @ 8,9,10,11?
        [FieldOffset(8)] 
        public uint unknown;

        [FieldOffset(12)]
        public uint dwOfs;

        [FieldOffset(16)]
        public uint dwType;

        [FieldOffset(20)]
        public uint dwFlags;
    }
}
