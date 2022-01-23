using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Modules.TC1014
{
    public class MemoryPointer
    {
        private unsafe byte* _pointer;
        private readonly int _offset;

        public MemoryPointer()
        {
            _offset = 0;
        }

        private unsafe MemoryPointer(byte* pointer, int offset)
        {
            _pointer = pointer;
            _offset = offset;
        }

        public unsafe byte* GetPointer() => _pointer;
        public unsafe MemoryPointer GetMemoryPointer(int offset) => new MemoryPointer(_pointer, offset);

        public unsafe byte this[int index]
        {
            get => _pointer[index + _offset];
            set => _pointer[index + _offset] = value;
        }

        public unsafe bool Reset(uint size)
        {
            FreeMemory(_pointer);

            _pointer = AllocateMemory(size);

            return _pointer != null;
        }

        public unsafe void Reset(MemoryPointer source)
        {
            _pointer = source.GetPointer();
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

    public class WideMemoryPointer
    {
        private readonly MemoryPointer _source;

        public ushort this[long index] => (ushort)((_source[(int)(index << 1) + 1] << 8) | _source[(int)(index << 1)]);

        public WideMemoryPointer(MemoryPointer source)
        {
            _source = source;
        }
    }
}