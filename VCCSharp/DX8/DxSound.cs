// ReSharper disable IdentifierTypo
using DX8;
using DX8.Converters;
using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VCCSharp.Models;
using static System.IntPtr;
using HWND = System.IntPtr;
using LPVOID = System.IntPtr;

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

        void CopyBuffer(uint[] buffer);

        bool Lock(uint offset, ushort length);
        bool Unlock();
    }

    public class DxSound : IDxSound
    {
        private delegate int DirectSoundEnumerateCallbackTemplate(IntPtr pGuid, IntPtr pDescription, IntPtr pModule, IntPtr pContext);

        private readonly IDSound _sound;
        private readonly IDxFactory _factory;

        private IDirectSound _ds;
        private IDirectSoundBuffer _buffer;

        private LPVOID SndPointer1 = Zero;
        private LPVOID SndPointer2 = Zero;

        private byte[] _buffer1 = Array.Empty<byte>();
        private byte[] _buffer2 = Array.Empty<byte>();

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

        private bool CreateDirectSoundBuffer(DSBUFFERDESC bufferDescription)
        {
            _buffer = _factory.CreateSoundBuffer(_ds, bufferDescription);

            return _buffer != null;
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

        private static DSBUFFERDESC CreateBufferDescription(uint length, ref WAVEFORMATEX waveFormat)
        {
            int flags = Define.DSBCAPS_GETCURRENTPOSITION2 | Define.DSBCAPS_LOCSOFTWARE | Define.DSBCAPS_STATIC | Define.DSBCAPS_GLOBALFOCUS;

            return new DSBUFFERDESC
            {
                dwSize = (uint)DSBUFFERDESC.Size,
                dwFlags = (uint)flags,
                dwBufferBytes = length,
                lpwfxFormat = IntPtrConverter.Convert(ref waveFormat)
            };
        }

        public List<string> EnumerateSoundCards()
        {
            var names = new List<string>();

            int Callback(IntPtr pGuid, IntPtr description, IntPtr module, IntPtr context)
            {
                _GUID guid = GuidConverter.ToGuid(pGuid);
                string text = Converter.ToString(description);

                _guids.Add(guid);
                names.Add(text);

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

            _buffer.GetCurrentPosition(ref playCursor, ref writeCursor);

            return playCursor;
        }

        public bool Lock(uint offset, ushort length)
        {
            //--TODO: I'm not really sure why I can't inline s1/s2 here...   "Variable has write usage"
            LPVOID s1 = SndPointer1;
            LPVOID s2 = SndPointer2;

            long result = _buffer.Lock(offset, length, ref s1, ref _sndLength1, ref s2, ref _sndLength2, 0);

            SndPointer1 = s1;
            SndPointer2 = s2;

            return result == Define.S_OK;
        }

        public bool Unlock()
        {
            return _buffer.Unlock(SndPointer1, _sndLength1, SndPointer2, _sndLength2) == Define.S_OK;
        }

        public bool CreateDirectSoundBuffer(ushort bitRate, uint length)
        {
            WAVEFORMATEX waveFormat = CreateWaveFormat(bitRate);
            DSBUFFERDESC soundBuffer = CreateBufferDescription(length, ref waveFormat);

            return CreateDirectSoundBuffer(soundBuffer);
        }

        public unsafe void CopyBuffer(uint[] buffer)
        {
            byte* byteBuffer;
            fixed (uint* p = buffer)
            {
                byteBuffer = (byte*)p;
            }

            Buffer.MemoryCopy(byteBuffer, (byte*)SndPointer1, _sndLength1, _sndLength1);

            if (SndPointer2 != Zero)
            {
                // copy last section of circular buffer if wrapped
                Buffer.MemoryCopy(&byteBuffer[_sndLength1], (byte*)SndPointer2, _sndLength1, _sndLength1);
            }
        }
    }
}
