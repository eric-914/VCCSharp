using DX8;
using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VCCSharp.Models;
using HWND = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.DX8
{
    public interface IDxSound
    {
        bool CreateDirectSound(int index);
        bool SetCooperativeLevel(HWND hWnd);

        List<string> EnumerateSoundCards();

        bool CreateDirectSoundBuffer(ushort bitRate, uint length);

        void Stop();
        void Play();
        void Reset();
        uint ReadPlayCursor();

        void ClearBuffer(uint length);
        void CopyBuffer(uint[] buffer);

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

        public IntPtr SndPointer1 = Zero;
        public IntPtr SndPointer2 = Zero;

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
            _ds = _factory.CreateDirectSound(_sound, _guids[index]);

            return _ds != null;
        }

        public bool SetCooperativeLevel(HWND hWnd)
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
                dwSize = (uint)DSBUFFERDESC.Size,
                dwFlags = (uint)flags,
                dwBufferBytes = length,
                lpwfxFormat = (IntPtr)waveFormat
            };
        }

        public List<string> EnumerateSoundCards()
        {
            List<string> names = new List<string>();

            int Callback(IntPtr pGuid, IntPtr description, IntPtr module, IntPtr context)
            {
                unsafe
                {
                    _GUID guid = pGuid != Zero ? *(_GUID*)pGuid : new _GUID();
                    string text = Converter.ToString((byte*)description);

                    _guids.Add(guid);
                    names.Add(text);
                }

                return names.Count < Define.MAXCARDS ? Define.TRUE : Define.FALSE;
            }

            IntPtr fn = Marshal.GetFunctionPointerForDelegate((DirectSoundEnumerateCallbackTemplate)Callback);

            _sound.DirectSoundEnumerate(fn, Zero);

            return names;
        }

        public void Stop() => _buffer.Stop();
        public void Play() => _buffer.Play(0, 0, Define.DSBPLAY_LOOPING);
        public void Reset() => _buffer.SetCurrentPosition(0);

        public uint ReadPlayCursor()
        {
            uint playCursor = 0, writeCursor = 0;

            unsafe
            {
                _buffer.GetCurrentPosition(&playCursor, &writeCursor);
            }

            return playCursor;
        }

        public unsafe bool Lock(uint offset, ushort length)
        {
            long LockBuffer(IntPtr* sp1, IntPtr* sp2) 
                => _buffer.Lock(offset, length, sp1, ref _sndLength1, sp2, ref _sndLength2, 0);

            IntPtr s1 = SndPointer1;
            IntPtr s2 = SndPointer2;

            var result = LockBuffer(&s1, &s2);

            SndPointer1 = s1;
            SndPointer2 = s2;

            return result == Define.S_OK;
        }

        public bool Unlock()
        {
            return _buffer.Unlock(SndPointer1, _sndLength1, SndPointer2, _sndLength2) == Define.S_OK;
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

        public unsafe void CopyBuffer(uint[] buffer)
        {
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

            byte* byteBuffer;
            fixed (uint* p = buffer)
            {
                byteBuffer = (byte*)p;
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
