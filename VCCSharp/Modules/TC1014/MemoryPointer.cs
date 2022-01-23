using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Modules.TC1014
{
    public class MemoryPointer
    {
        private unsafe byte* Pointer { get; set; }

        public unsafe byte* GetPointer(int offset) => Pointer + offset;

        public unsafe byte this[int index]
        {
            get => Pointer[index];
            set => Pointer[index] = value;
        }

        public unsafe bool Reset(uint size)
        {
            FreeMemory(Pointer);

            Pointer = AllocateMemory(size);

            return Pointer != null;
        }

        public unsafe void Reset(MemoryPointer source)
        {
            Pointer = source.Pointer;
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