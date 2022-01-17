using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Interfaces;
using VCCSharp.DX8.Libraries;
using VCCSharp.DX8.Models;
using VCCSharp.Models;
using static System.IntPtr;

namespace VCCSharp.DX8
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int DirectSoundEnumerateCallbackTemplate(IntPtr pGuid, IntPtr pDescription, IntPtr pModule, IntPtr pContext);

    public interface IDxSound
    {
        unsafe bool CreateDirectSound(_GUID* guid);
        bool SetCooperativeLevel(IntPtr hWnd);
        bool CreateDirectSoundBuffer(DSBUFFERDESC bufferDescription);

        long DirectSoundEnumerate(DirectSoundEnumerateCallbackTemplate callback);
        bool LockBuffer(uint offset, ushort length, uint sndLength1, uint sndLength2);
        bool UnlockBuffer(uint sndLength1, uint sndLength2);
        void ClearBuffer(uint sndBuffLength);
        unsafe void CopyBuffer(uint sndLength1, uint* buffer);

        void Stop();
        void Play();
        void Reset();
        void ReadCursors();

        ulong PlayCursor { get; }
        ulong WriteCursor { get; }

        unsafe int Lock(uint offset, ushort length, IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2);
        unsafe int Unlock(IntPtr* sp1, uint sl1, IntPtr* sp2, uint sl2);
    }

    public class DxSound : IDxSound
    {
        private readonly IDSound _sound;
        private readonly IDxFactory _factory;

        private IDirectSound _ds;
        private IDirectSoundBuffer _buffer;

        // ReSharper disable IdentifierTypo
        private DSBUFFERDESC _dsbd;     // direct sound description
        private WAVEFORMATEX _pcmwf;    //generic wave format structure
        // ReSharper restore IdentifierTypo

        public IntPtr SndPointer1;
        public IntPtr SndPointer2;

        public ulong PlayCursor { get; private set; } = 0;
        public ulong WriteCursor { get; private set; } = 0;

        public DxSound(IDSound sound, IDxFactory factory)
        {
            _sound = sound;
            _factory = factory;
        }

        public unsafe bool CreateDirectSound(_GUID* guid)
        {
            _ds = _factory.CreateDirectSound(_sound, guid);

            return _ds != null;
        }

        public bool SetCooperativeLevel(IntPtr hWnd)
        {
            return _ds.SetCooperativeLevel(hWnd, Define.DSSCL_NORMAL) == Define.S_OK;
        }

        public unsafe bool CreateDirectSoundBuffer(DSBUFFERDESC bufferDescription)
        {
            _buffer = _factory.CreateSoundBuffer(_ds, &bufferDescription);

            return (_buffer != null);
        }

        public long DirectSoundEnumerate(DirectSoundEnumerateCallbackTemplate callback)
        {
            IntPtr fn = Marshal.GetFunctionPointerForDelegate(callback);

            return _sound.DirectSoundEnumerate(fn, Zero);
        }

        public unsafe bool LockBuffer(uint offset, ushort length, uint sndLength1, uint sndLength2)
        {
            fixed (IntPtr* sp1 = &SndPointer1)
            {
                fixed (IntPtr* sp2 = &SndPointer2)
                {
                    var result = (int)_buffer.Lock(offset, length, sp1, &sndLength1, sp2, &sndLength2, 0);

                    return result == Define.S_OK;
                }
            }
        }

        public bool UnlockBuffer(uint sndLength1, uint sndLength2)
        {
            return (int)_buffer.Unlock(SndPointer1, sndLength1, SndPointer2, sndLength2) == Define.S_OK;
        }

        public void ClearBuffer(uint sndBuffLength)
        {
            ClearBuffer(SndPointer1, sndBuffLength);
        }

        public unsafe void CopyBuffer(uint sndLength1, uint* buffer)
        {
            byte* byteBuffer = (byte*)buffer;

            CopyBuffer(SndPointer1, byteBuffer, sndLength1);

            if (SndPointer2 != Zero)
            {
                // copy last section of circular buffer if wrapped
                CopyBuffer(SndPointer2, &byteBuffer[sndLength1], sndLength1);
            }
        }

        public void Stop() => _buffer.Stop();
        public void Play() => _buffer.Play(0, 0, Define.DSBPLAY_LOOPING);
        public void Reset() => _buffer.SetCurrentPosition(0);

        public void ReadCursors()
        {
            ulong playCursor = 0, writeCursor = 0;

            unsafe
            {
                _buffer.GetCurrentPosition(&playCursor, &writeCursor);
            }

            PlayCursor = playCursor;
            WriteCursor = writeCursor;
        }

        private static unsafe void CopyBuffer(IntPtr sndPtr, byte* source, uint length)
        {
            byte* buffer = (byte*)sndPtr;

            if (buffer == null)
            {
                throw new Exception("Bad buffer");
            }

            for (int index = 0; index < length; index++)
            {
                buffer[index] = source[index];
            }
        }

        private static unsafe void ClearBuffer(IntPtr sndPtr, uint length)
        {
            byte* buffer = (byte*)sndPtr;

            if (buffer == null)
            {
                throw new Exception("Bad buffer");
            }

            for (int index = 0; index < length; index++)
            {
                buffer[index] = 0;
            }
        }

        public unsafe int Lock(uint offset, ushort length, IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2) => (int)_buffer.Lock(offset, length, sp1, sl1, sp2, sl2, 0);

        public unsafe int Unlock(IntPtr* sp1, uint sl1, IntPtr* sp2, uint sl2) => (int)_buffer.Unlock(*sp1, sl1, *sp2, sl2);
    }
}
