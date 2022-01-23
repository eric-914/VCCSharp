// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Modules.TC1014
{
    public class MemoryPointers
    {
        public class MemoryPointer
        {
            public unsafe byte* Pointer { get; set; }

            public unsafe byte* GetPointer(int offset) => Pointer + offset;

            public unsafe byte this[int index]
            {
                get => Pointer[index];
                set => Pointer[index] = value;
            }
        }

        public MemoryPointer ROM { get; } = new MemoryPointer();
        public MemoryPointer RAM { get; } = new MemoryPointer();

        public MemoryPointer InternalRomBuffer { get; } = new MemoryPointer();

        public unsafe bool ResetRAM(uint size)
        {
            FreeMemory(RAM.Pointer);

            RAM.Pointer = AllocateMemory(size);

            return RAM.Pointer != null;
        }

        public unsafe bool ResetROM(uint size)
        {
            FreeMemory(InternalRomBuffer.Pointer);

            InternalRomBuffer.Pointer = AllocateMemory(size);

            return InternalRomBuffer.Pointer != null;
        }

        public unsafe void ResetROM()
        {
            ROM.Pointer = InternalRomBuffer.Pointer;
        }

        private static unsafe void FreeMemory(byte* target)
        {
            if (target != null)
            {
                Marshal.FreeHGlobal((IntPtr)target);
            }
        }

        private static unsafe byte* AllocateMemory(uint size)
        {
            return (byte*)Marshal.AllocHGlobal((int)size); //malloc(size);
        }

    }
}
