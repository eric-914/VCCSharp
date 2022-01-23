﻿namespace VCCSharp.Modules.TC1014
{
    public class BytePointer
    {
        private byte[] _buffer;
        private readonly int _offset;

        public BytePointer()
        {
            _offset = 0;
        }

        private BytePointer(byte[] buffer, int offset)
        {
            _buffer = buffer;
            _offset = offset;
        }

        public BytePointer GetBytePointer(int offset) => new BytePointer(_buffer, offset);

        public byte this[int index]
        {
            get => _buffer[index + _offset];
            set => _buffer[index + _offset] = value;
        }

        public void Reset(uint size)
        {
            _buffer = new byte[size];
        }

        public void Reset(BytePointer source)
        {
            _buffer = source._buffer;
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