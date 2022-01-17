using System;
using VCCSharp.DX8;
using VCCSharp.DX8.Models;
using VCCSharp.IoC;
using VCCSharp.Models;
using static System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* buffer, ushort length);
        unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate);
        int GetFreeBlockCount();
        AudioSpectrum Spectrum { get; set; }
        bool PauseAudio(bool pause);
        ushort CurrentRate { get; set; }
        void DirectSoundEnumerateSoundCards();
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;

        private readonly IDxSound _sound;

        public AudioSpectrum Spectrum { get; set; }
        public ushort CurrentRate { get; set; }

        private bool _initialized;
        private bool _audioPause;

        private ushort _bitRate;
        private ushort _blockSize;

        private readonly ushort[] _rateList = { 0, 11025, 22050, 44100 };

        private int _hr;

        // ReSharper disable once NotAccessedField.Local
        private byte _auxBufferPointer;

        private uint _sndLength1;
        private uint _sndLength2;
        private uint _sndBuffLength;
        private uint _buffOffset;

        public IntPtr SndPointer1;
        public IntPtr SndPointer2;

        // ReSharper disable IdentifierTypo
        private DSBUFFERDESC _dsbd;     // direct sound description
        private WAVEFORMATEX _pcmwf;    // generic wave format structure
        // ReSharper restore IdentifierTypo

        private bool _mute;

        public Audio(IModules modules, IDxSound sound)
        {
            _modules = modules;
            _sound = sound;
        }

        public unsafe int SoundInit(IntPtr hWnd, _GUID* guid, ushort rate)
        {
            rate &= 3;

            if (rate != 0)
            {
                //TODO: Since there is only 44100 or mute, remove the other options and make this a boolean
                //Force 44100 or Mute
                rate = 3;
            }

            CurrentRate = rate;

            _sndLength1 = 0;
            _sndLength2 = 0;
            _buffOffset = 0;
            _auxBufferPointer = 0;
            _bitRate = _rateList[rate];
            _blockSize = (ushort)(_bitRate * 4 / Define.TARGETFRAMERATE);
            _sndBuffLength = (ushort)(_blockSize * Define.AUDIOBUFFERS);

            int result = 0;

            if (!_initialized)
            {
                if (!_sound.CreateDirectSound(guid)) return 1;

                // set cooperation level normal
                if (!_sound.SetCooperativeLevel(hWnd)) return 1;

                if (!CreateDirectSoundBuffer()) return 1;

                // Clear out sound buffers
                _hr = DirectSoundLock((ushort)_sndBuffLength);

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                ClearBuffer(SndPointer1, _sndBuffLength);

                _hr = DirectSoundUnlock();

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                _sound.Reset();

                if (!_mute)
                {
                    _sound.Play();
                }

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                _initialized = true;
            }

            _audioPause = false;
            _modules.CoCo.SetAudioRate(_rateList[rate]);

            _mute = (rate == 0);

            return result;
        }

        public unsafe bool CreateDirectSoundBuffer()
        {
            uint avgBytesPerSec = (uint)(_bitRate * Define.BLOCKALIGN);

            _pcmwf.wFormatTag = Define.WAVE_FORMAT_PCM;
            _pcmwf.nChannels = Define.CHANNELS;
            _pcmwf.nSamplesPerSec = _bitRate;
            _pcmwf.wBitsPerSample = Define.BITSPERSAMPLE;
            _pcmwf.nBlockAlign = Define.BLOCKALIGN;
            _pcmwf.nAvgBytesPerSec = avgBytesPerSec;
            _pcmwf.cbSize = 0;

            int flags = Define.DSBCAPS_GETCURRENTPOSITION2 | Define.DSBCAPS_LOCSOFTWARE | Define.DSBCAPS_STATIC | Define.DSBCAPS_GLOBALFOCUS;

            _dsbd.dwSize = (uint)sizeof(DSBUFFERDESC);
            _dsbd.dwFlags = (uint)flags;
            _dsbd.dwBufferBytes = _sndBuffLength;

            fixed (WAVEFORMATEX* p = &(_pcmwf))
            {
                _dsbd.lpwfxFormat = (IntPtr)p;
            }

            return _sound.CreateDirectSoundBuffer(_dsbd);
        }

        public short SoundDeInit()
        {
            if (_initialized)
            {
                _initialized = false;

                _sound.Stop();
            }

            return 0;
        }

        public void ResetAudio()
        {
            _modules.CoCo.SetAudioRate(_rateList[CurrentRate]);

            if (_initialized)
            {
                _sound.Reset();
            }

            _buffOffset = 0;
            _auxBufferPointer = 0;
        }

        private unsafe int DirectSoundLock(ushort length)
        {
            int Invoke(IntPtr* sp1, uint* sl1, IntPtr* sp2, uint* sl2)
            {
                return _sound.Lock(_buffOffset, length, sp1, sl1, sp2, sl2);
            }

            fixed (uint* sl1 = &_sndLength1)
            {
                fixed (uint* sl2 = &_sndLength2)
                {
                    IntPtr s1 = SndPointer1;
                    IntPtr s2 = SndPointer2;

                    var result = Invoke(&s1, sl1, &s2, sl2);

                    SndPointer1 = s1;
                    SndPointer2 = s2;

                    return result;
                }
            }
        }

        private unsafe int DirectSoundUnlock()
        {
            fixed (IntPtr* sp1 = &SndPointer1)
            {
                fixed (IntPtr* sp2 = &SndPointer2)
                {
                    return _sound.Unlock(sp1, _sndLength1, sp2, _sndLength2);
                }
            }
        }

        public unsafe void FlushAudioBuffer(uint* buffer, ushort length)
        {
            byte* byteBuffer = (byte*)buffer;

            ushort leftAverage = (ushort)(buffer[0] >> 16);
            ushort rightAverage = (ushort)(buffer[0] & 0xFFFF);

            _modules.Audio.Spectrum?.UpdateSoundBar(leftAverage, rightAverage);

            if (!_initialized || _audioPause || length == 0 || _mute)
            {
                return;
            }

            if (GetFreeBlockCount() <= 0)   //this should only kick in when frame skipping or un-throttled
            {
                HandleSlowAudio();

                return;
            }

            _hr = DirectSoundLock(length);

            if (_hr != Define.DS_OK)
            {
                return;
            }

            CopyBuffer(SndPointer1, byteBuffer, _sndLength1);

            if (SndPointer2 != Zero)
            {
                // copy last section of circular buffer if wrapped
                CopyBuffer(SndPointer2, &byteBuffer[_sndLength1], _sndLength1);
            }

            _hr = DirectSoundUnlock();// unlock the buffer

            _buffOffset = (_buffOffset + length) % _sndBuffLength; //Where to write next
        }

        public void HandleSlowAudio()
        {
            _auxBufferPointer++;	//and chase your own tail
            _auxBufferPointer %= 5;	//At this point we are so far behind we may as well drop the buffer
        }

        public int GetFreeBlockCount()
        {
            ulong playCursor = 0;
            long maxSize;

            if (!_initialized || _audioPause || _mute)
            {
                return Define.AUDIOBUFFERS;
            }

            if (!_mute)
            {
                _sound.ReadCursors();
                playCursor = _sound.PlayCursor;
            }

            if (_buffOffset <= playCursor)
            {
                maxSize = (long)(playCursor - _buffOffset);
            }
            else
            {
                maxSize = (long)(_sndBuffLength - _buffOffset + playCursor);
            }

            return (int)(maxSize / _blockSize);
        }

        public bool PauseAudio(bool pause)
        {
            _audioPause = pause;

            if (_initialized)
            {
                if (_audioPause)
                {
                    _sound.Stop();
                }
                else
                {
                    if (!_mute)
                    {
                        _sound.Play();
                    }
                }
            }

            return _audioPause;
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

        public void DirectSoundEnumerateSoundCards()
        {
            _sound.DirectSoundEnumerate(DirectSoundEnumerateCallback);
        }

        public int DirectSoundEnumerateCallback(IntPtr guid, IntPtr description, IntPtr module, IntPtr context)
        {
            unsafe
            {
                var text = Converter.ToString((byte*)description);
                var index = _modules.Config.NumberOfSoundCards;

                var cards = _modules.Config.SoundCards;

                fixed (SoundCardList* card = &(cards[index]))
                {
                    Converter.ToByteArray(text, (*card).CardName);
                    (*card).Guid =(_GUID*)guid;
                }

                _modules.Config.NumberOfSoundCards++;

                return _modules.Config.NumberOfSoundCards < Define.MAXCARDS ? Define.TRUE : Define.FALSE;
            }
        }
    }
}
