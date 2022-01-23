using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Modules.TC1014
{
    public class BytePointer
    {
        private unsafe byte* _pointer;
        private readonly int _offset;

        public BytePointer()
        {
            _offset = 0;
        }

        private unsafe BytePointer(byte* pointer, int offset)
        {
            _pointer = pointer;
            _offset = offset;
        }

        public unsafe byte* GetPointer() => _pointer;
        public unsafe BytePointer GetMemoryPointer(int offset) => new BytePointer(_pointer, offset);

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

        public unsafe void Reset(BytePointer source)
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

    public class ShortPointer
    {
        private readonly BytePointer _source;

        public ushort this[long index] => (ushort)((_source[(int)(index << 1) + 1] << 8) | _source[(int)(index << 1)]);

        public ShortPointer(BytePointer source)
        {
            _source = source;
        }
    }
}