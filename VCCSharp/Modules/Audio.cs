using System.Collections.Generic;
using VCCSharp.DX8;
using VCCSharp.IoC;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        List<string> FindSoundDevices();

        void SoundInit(HWND hWnd, int index, ushort rate);
        short SoundDeInit();

        bool PauseAudio(bool pause);
        void ResetAudio();
        void FlushAudioBuffer(uint[] buffer, ushort length);
        int GetFreeBlockCount();

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

        public void SoundInit(HWND hWnd, int index, ushort rate)
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
                if (!_sound.CreateDirectSound(index)) return;

                // set cooperation level normal
                if (!_sound.SetCooperativeLevel(hWnd)) return;

                if (!_sound.CreateDirectSoundBuffer(_bitRate, _sndBuffLength)) return;

                // Clear out sound buffers
                if (!_sound.Lock(_buffOffset, (ushort)_sndBuffLength)) return;

                _sound.CopyBuffer(new uint[_sndBuffLength >> 2]);

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

        public void FlushAudioBuffer(uint[] buffer, ushort length)
        {
            uint leftAverage = buffer[0] >> 16;
            uint rightAverage = buffer[0] & 0xFFFF;

            _modules.Audio.Spectrum?.UpdateSoundBar((int)leftAverage, (int)rightAverage);

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

            if (!_initialized || _audioPause || _mute)
            {
                return Define.AUDIOBUFFERS;
            }

            if (!_mute)
            {
                playCursor = _sound.ReadPlayCursor();
            }

            long maxSize = (long)(_buffOffset <= playCursor ? playCursor - _buffOffset : _sndBuffLength - _buffOffset + playCursor);

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

        public List<string> FindSoundDevices() => _sound.EnumerateSoundCards();
    }
}
