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

        long DirectSoundEnumerate(DirectSoundEnumerateCallbackTemplate callback);

        bool CreateDirectSoundBuffer(ushort bitRate, uint length);

        void Stop();
        void Play();
        void Reset();
        ulong ReadPlayCursor();

        unsafe int Lock(uint offset, ushort length, IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2);
        unsafe int Unlock(IntPtr* sp1, uint sl1, IntPtr* sp2, uint sl2);
    }

    public class DxSound : IDxSound
    {
        private readonly IDSound _sound;
        private readonly IDxFactory _factory;

        private IDirectSound _ds;
        private IDirectSoundBuffer _buffer;

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

        private static WAVEFORMATEX CreateWaveFormat(ushort bitRate)
        {
            uint avgBytesPerSec = (uint)(bitRate * Define.BLOCKALIGN);

            // generic wave format structure
            return new WAVEFORMATEX
            {
                wFormatTag = Define.WAVE_FORMAT_PCM,
                nChannels = Define.CHANNELS,
                nSamplesPerSec = bitRate,
                wBitsPerSample = Define.BITSPERSAMPLE,
                nBlockAlign = Define.BLOCKALIGN,
                nAvgBytesPerSec = avgBytesPerSec,
                cbSize = 0
            };
        }

        private static unsafe DSBUFFERDESC CreateBufferDescription(uint length, WAVEFORMATEX* waveFormat)
        {
            int flags = Define.DSBCAPS_GETCURRENTPOSITION2 | Define.DSBCAPS_LOCSOFTWARE | Define.DSBCAPS_STATIC | Define.DSBCAPS_GLOBALFOCUS;

            return new DSBUFFERDESC
            {
                dwSize = (uint)sizeof(DSBUFFERDESC),
                dwFlags = (uint)flags,
                dwBufferBytes = length,
                lpwfxFormat = (IntPtr)waveFormat
            };
        }

        public long DirectSoundEnumerate(DirectSoundEnumerateCallbackTemplate callback)
        {
            IntPtr fn = Marshal.GetFunctionPointerForDelegate(callback);

            return _sound.DirectSoundEnumerate(fn, Zero);
        }

        public void Stop() => _buffer.Stop();
        public void Play() => _buffer.Play(0, 0, Define.DSBPLAY_LOOPING);
        public void Reset() => _buffer.SetCurrentPosition(0);

        public ulong ReadPlayCursor()
        {
            ulong playCursor = 0, writeCursor = 0;

            unsafe
            {
                _buffer.GetCurrentPosition(&playCursor, &writeCursor);
            }

            return playCursor;
        }

        public unsafe int Lock(uint offset, ushort length, IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2) => (int)_buffer.Lock(offset, length, sp1, sl1, sp2, sl2, 0);

        public unsafe int Unlock(IntPtr* sp1, uint sl1, IntPtr* sp2, uint sl2) => (int)_buffer.Unlock(*sp1, sl1, *sp2, sl2);

        public unsafe bool CreateDirectSoundBuffer(ushort bitRate, uint length)
        {
            WAVEFORMATEX waveFormat = CreateWaveFormat(bitRate);

            DSBUFFERDESC soundBuffer = CreateBufferDescription(length, &waveFormat);

            return CreateDirectSoundBuffer(soundBuffer);
        }
    }
}
