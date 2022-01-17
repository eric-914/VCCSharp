using System;
using VCCSharp.DX8;
using VCCSharp.DX8.Models;
using VCCSharp.IoC;
using VCCSharp.Models;
using HWND = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        void SoundInit(HWND hWnd, _GUID guid, ushort rate);
        short SoundDeInit();

        bool PauseAudio(bool pause);
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* buffer, ushort length);
        int GetFreeBlockCount();
        void EnumerateSoundCards();

        AudioSpectrum Spectrum { get; set; }
        ushort CurrentRate { get; set; }
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

        private uint _sndBuffLength;
        private uint _buffOffset;

        private bool _mute;

        public Audio(IModules modules, IDxSound sound)
        {
            _modules = modules;
            _sound = sound;
        }

        public void SoundInit(IntPtr hWnd, _GUID guid, ushort rate)
        {
            rate &= 3;

            if (rate != 0)
            {
                //TODO: Since there is only 44100 or mute, remove the other options and make this a boolean
                //Force 44100 or Mute
                rate = 3;
            }

            CurrentRate = rate;

            _buffOffset = 0;
            _bitRate = _rateList[rate];
            _blockSize = (ushort)(_bitRate * 4 / Define.TARGETFRAMERATE);
            _sndBuffLength = (ushort)(_blockSize * Define.AUDIOBUFFERS);

            if (!_initialized)
            {
                if (!_sound.CreateDirectSound(guid)) return;

                // set cooperation level normal
                if (!_sound.SetCooperativeLevel(hWnd)) return;

                if (!_sound.CreateDirectSoundBuffer(_bitRate, _sndBuffLength)) return;

                // Clear out sound buffers
                if (!_sound.Lock(_buffOffset, (ushort)_sndBuffLength)) return;

                _sound.ClearBuffer(_sndBuffLength);

                if (!_sound.Unlock()) return;

                _sound.Reset();

                if (!_mute)
                {
                    _sound.Play();
                }

                _initialized = true;
            }

            _audioPause = false;
            _modules.CoCo.SetAudioRate(_rateList[rate]);

            _mute = (rate == 0);
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
        }

        public unsafe void FlushAudioBuffer(uint* buffer, ushort length)
        {
            ushort leftAverage = (ushort)(buffer[0] >> 16);
            ushort rightAverage = (ushort)(buffer[0] & 0xFFFF);

            _modules.Audio.Spectrum?.UpdateSoundBar(leftAverage, rightAverage);

            if (!_initialized || _audioPause || length == 0 || _mute)
            {
                return;
            }

            if (GetFreeBlockCount() <= 0)   //this should only kick in when frame skipping or un-throttled
            {
                return;
            }

            if (!_sound.Lock(_buffOffset, length)) return;

            _sound.CopyBuffer(buffer);

            _sound.Unlock(); // unlock the buffer

            _buffOffset = (_buffOffset + length) % _sndBuffLength; //Where to write next
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
                playCursor = _sound.ReadPlayCursor();
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

        public void EnumerateSoundCards()
        {
            int Callback(IntPtr guid, IntPtr description, IntPtr module, IntPtr context)
            {
                unsafe
                {
                    var text = Converter.ToString((byte*)description);

                    var card = new SoundCard
                    {
                        CardName = text
                    };

                    if (guid != Zero)
                    {
                        card.Guid = *(_GUID*)guid;
                    }

                    _modules.Config.SoundCards.Add(card);

                    return _modules.Config.SoundCards.Count < Define.MAXCARDS ? Define.TRUE : Define.FALSE;
                }
            }

            _sound.DirectSoundEnumerate(Callback);
        }
    }
}
