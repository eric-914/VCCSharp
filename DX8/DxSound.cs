// ReSharper disable IdentifierTypo

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DX8.Internal;
using DX8.Internal.Converters;
using DX8.Internal.Interfaces;
using DX8.Internal.Libraries;
using DX8.Internal.Models;
using static System.IntPtr;
using HWND = System.IntPtr;
using LPVOID = System.IntPtr;

namespace DX8
{
    public interface IDxSound
    {
        bool CreateDirectSound(int index);
        bool SetCooperativeLevel(HWND hWnd);

        List<string> EnumerateSoundCards();

        bool CreateDirectSoundBuffer(ushort bitRate, int length);

        void Stop();
        void Play();
        void Reset();
        int ReadPlayCursor();

        void CopyBuffer(int[] buffer);

        bool Lock(int offset, int length);
        bool Unlock();
    }

    public class DxSound : IDxSound
    {
        private delegate int DirectSoundEnumerateCallbackTemplate(IntPtr pGuid, IntPtr pDescription, IntPtr pModule, IntPtr pContext);

        private readonly IDSound _sound;
        private readonly IDxFactory _factory;

        private IDirectSound _ds;
        private IDirectSoundBuffer _buffer;

        private LPVOID _sndPointer1 = Zero;
        private LPVOID _sndPointer2 = Zero;

        private uint _sndLength1;
        private uint _sndLength2;

        // ReSharper disable once IdentifierTypo
        private readonly List<_GUID> _guids = new List<_GUID>();

        internal DxSound(IDSound sound, IDxFactory factory)
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
            return _ds.SetCooperativeLevel(hWnd, DxDefine.DSSCL_NORMAL) == DxDefine.S_OK;
        }

        private bool CreateDirectSoundBuffer(DSBUFFERDESC bufferDescription)
        {
            _buffer = _factory.CreateSoundBuffer(_ds, bufferDescription);

            return _buffer != null;
        }

        private static WAVEFORMATEX CreateWaveFormat(ushort bitRate)
        {
            uint avgBytesPerSec = (uint)(bitRate * DxDefine.BLOCKALIGN);

            // generic wave format structure
            return new WAVEFORMATEX
            {
                wFormatTag = DxDefine.WAVE_FORMAT_PCM,
                nChannels = DxDefine.CHANNELS,
                nSamplesPerSec = bitRate,
                wBitsPerSample = DxDefine.BITSPERSAMPLE,
                nBlockAlign = DxDefine.BLOCKALIGN,
                nAvgBytesPerSec = avgBytesPerSec,
                cbSize = 0
            };
        }

        private static DSBUFFERDESC CreateBufferDescription(int length, ref WAVEFORMATEX waveFormat)
        {
            int flags = DxDefine.DSBCAPS_GETCURRENTPOSITION2 | DxDefine.DSBCAPS_LOCSOFTWARE | DxDefine.DSBCAPS_STATIC | DxDefine.DSBCAPS_GLOBALFOCUS;

            return new DSBUFFERDESC
            {
                dwSize = (uint)DSBUFFERDESC.Size,
                dwFlags = (uint)flags,
                dwBufferBytes = (uint)length,
                lpwfxFormat = IntPtrConverter.Convert(ref waveFormat)
            };
        }

        public List<string> EnumerateSoundCards()
        {
            var names = new List<string>();

            int Callback(IntPtr pGuid, IntPtr description, IntPtr module, IntPtr context)
            {
                _GUID guid = GuidConverter.ToGuid(pGuid);
                string text = StringConverter.ToString(description);

                _guids.Add(guid);
                names.Add(text);

                return names.Count < DxDefine.MAXCARDS ? DxDefine.TRUE : DxDefine.FALSE;
            }

            IntPtr fn = Marshal.GetFunctionPointerForDelegate((DirectSoundEnumerateCallbackTemplate)Callback);

            _sound.DirectSoundEnumerate(fn, Zero);

            return names;
        }

        public void Stop() => _buffer.Stop();
        public void Play() => _buffer.Play(0, 0, DxDefine.DSBPLAY_LOOPING);
        public void Reset() => _buffer.SetCurrentPosition(0);

        public int ReadPlayCursor()
        {
            uint playCursor = 0, writeCursor = 0;

            _buffer.GetCurrentPosition(ref playCursor, ref writeCursor);

            return (int)playCursor;
        }

        public bool Lock(int offset, int length)
        {
            //--TODO: I'm not really sure why I can't inline s1/s2 here...   "Variable has write usage"
            LPVOID s1 = _sndPointer1;
            LPVOID s2 = _sndPointer2;

            long result = _buffer.Lock((uint)offset, (uint)length, ref s1, ref _sndLength1, ref s2, ref _sndLength2, 0);

            _sndPointer1 = s1;
            _sndPointer2 = s2;

            return result == DxDefine.S_OK;
        }

        public bool Unlock()
        {
            return _buffer.Unlock(_sndPointer1, _sndLength1, _sndPointer2, _sndLength2) == DxDefine.S_OK;
        }

        public bool CreateDirectSoundBuffer(ushort bitRate, int length)
        {
            WAVEFORMATEX waveFormat = CreateWaveFormat(bitRate);
            DSBUFFERDESC soundBuffer = CreateBufferDescription(length, ref waveFormat);

            return CreateDirectSoundBuffer(soundBuffer);
        }

        public void CopyBuffer(int[] buffer)
        {
            Marshal.Copy(buffer, 0, _sndPointer1, (int)(_sndLength1 >> 2));

            if (_sndPointer2 != Zero)
            {
                Marshal.Copy(buffer, (int)(_sndLength1 >> 2), _sndPointer2, (int)(_sndLength2 >> 2));
            }
        }
    }
}
