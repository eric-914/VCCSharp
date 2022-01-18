using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DX8;
using VCCSharp.Models;
using static System.IntPtr;

namespace VCCSharp.DX8
{
    public delegate void SoundEnumerateCallback(string description);

    public interface IDxSound
    {
        bool CreateDirectSound(int index);
        bool SetCooperativeLevel(IntPtr hWnd);

        void EnumerateSoundCards(SoundEnumerateCallback callback);

        bool CreateDirectSoundBuffer(ushort bitRate, uint length);

        void Stop();
        void Play();
        void Reset();
        ulong ReadPlayCursor();

        void ClearBuffer(uint length);
        unsafe void CopyBuffer(uint* buffer);

        bool Lock(uint offset, ushort length);
        bool Unlock();
    }

    public class DxSound : IDxSound
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DirectSoundEnumerateCallbackTemplate(IntPtr pGuid, IntPtr pDescription, IntPtr pModule, IntPtr pContext);

        private readonly IDSound _sound;
        private readonly IDxFactory _factory;

        private IDirectSound _ds;
        private IDirectSoundBuffer _buffer;

        public IntPtr SndPointer1;
        public IntPtr SndPointer2;

        private uint _sndLength1;
        private uint _sndLength2;

        // ReSharper disable once IdentifierTypo
        private readonly List<_GUID> _guids = new List<_GUID>();

        public DxSound(IDSound sound, IDxFactory factory)
        {
            _sound = sound;
            _factory = factory;
        }

        public DxSound() : this(new DSound(), new DxFactory()) { }

        public bool CreateDirectSound(int index)
        {
            _GUID guid = _guids[index];

            unsafe
            {
                _ds = _factory.CreateDirectSound(_sound, &guid);
            }

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

        public void EnumerateSoundCards(SoundEnumerateCallback callback)
        {
            int count = 0;

            int Callback(IntPtr pGuid, IntPtr description, IntPtr module, IntPtr context)
            {
                unsafe
                {
                    _GUID guid = pGuid != Zero ? *(_GUID*)pGuid : new _GUID();
                    string text = Converter.ToString((byte*)description);

                    _guids.Add(guid);
                    callback(text);
                }

                return ++count < Define.MAXCARDS ? Define.TRUE : Define.FALSE;
            }

            IntPtr fn = Marshal.GetFunctionPointerForDelegate((DirectSoundEnumerateCallbackTemplate)Callback);

            _sound.DirectSoundEnumerate(fn, Zero);
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

        public unsafe bool Lock(uint offset, ushort length)
        {
            long LockBuffer(IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2) 
                => _buffer.Lock(offset, length, sp1, sl1, sp2, sl2, 0);

            fixed (uint* sl1 = &_sndLength1)
            {
                fixed (uint* sl2 = &_sndLength2)
                {
                    IntPtr s1 = SndPointer1;
                    IntPtr s2 = SndPointer2;

                    var result = LockBuffer(&s1, sl1, &s2, sl2);

                    SndPointer1 = s1;
                    SndPointer2 = s2;

                    return result == Define.S_OK;
                }
            }
        }

        public unsafe bool Unlock()
        {
            long UnlockBuffer(IntPtr* sp1, IntPtr* sp2) 
                => _buffer.Unlock(*sp1, _sndLength1, *sp2, _sndLength2);

            fixed (IntPtr* sp1 = &SndPointer1)
            {
                fixed (IntPtr* sp2 = &SndPointer2)
                {
                    return UnlockBuffer(sp1, sp2) == Define.S_OK;
                }
            }
        }

        public unsafe bool CreateDirectSoundBuffer(ushort bitRate, uint length)
        {
            WAVEFORMATEX waveFormat = CreateWaveFormat(bitRate);
            DSBUFFERDESC soundBuffer = CreateBufferDescription(length, &waveFormat);

            return CreateDirectSoundBuffer(soundBuffer);
        }

        public unsafe void ClearBuffer(uint length)
        {
            byte* buffer = (byte*)SndPointer1;

            if (buffer == null)
            {
                throw new Exception("Bad buffer");
            }

            for (int index = 0; index < length; index++)
            {
                buffer[index] = 0;
            }
        }

        public unsafe void CopyBuffer(uint* buffer)
        {
            byte* byteBuffer = (byte*)buffer;

            void Copy(IntPtr sndPtr, byte* source, uint length)
            {
                byte* target = (byte*)sndPtr;

                if (target == null)
                {
                    throw new Exception("Bad buffer");
                }

                for (int index = 0; index < length; index++)
                {
                    target[index] = source[index];
                }
            }

            Copy(SndPointer1, byteBuffer, _sndLength1);

            if (SndPointer2 != Zero)
            {
                // copy last section of circular buffer if wrapped
                Copy(SndPointer2, &byteBuffer[_sndLength1], _sndLength1);
            }
        }
    }
}
