using DX8;
using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        List<string> FindSoundDevices();

        void SoundInit();
        void SoundInit(HWND hWnd, int index, AudioRates rate);
        short SoundDeInit();

        bool PauseAudio(bool pause);
        void ResetAudio();
        void FlushAudioBuffer(int[] buffer, int length);
        int GetFreeBlockCount();

        AudioSpectrum? Spectrum { get; set; }
        AudioRates CurrentRate { get; set; }
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;
        private readonly IConfigurationRoot _configuration;
        private readonly IDxSound _sound;

        public AudioSpectrum? Spectrum { get; set; }
        public AudioRates CurrentRate { get; set; }

        private bool _initialized;
        private bool _audioPause;

        private ushort _bitRate;
        private ushort _blockSize;

        private readonly ushort[] _kHzRate = { 0, 11025, 22050, 44100 };

        private int _sndBuffLength;
        private int _buffOffset;

        private bool _mute;

        public Audio(IModules modules, IConfigurationRoot configuration, IDxSound sound)
        {
            _modules = modules;
            _configuration = configuration;
            _sound = sound;
        }

        public void SoundInit()
        {
            int deviceIndex = FindSoundDevices().IndexOf(_configuration.Audio.Device);

            SoundInit(_modules.Emu.WindowHandle, deviceIndex, _configuration.Audio.Rate.Value);
        }

        public void SoundInit(HWND hWnd, int index, AudioRates rate)
        {
            if (rate != AudioRates.Disabled)
            {
                //TODO: Since there is only 44100 or mute, remove the other options and make this a boolean
                //Force 44100 or Disabled
                rate = AudioRates._44100Hz;
            }

            CurrentRate = rate;

            _buffOffset = 0;
            _bitRate = _kHzRate[(int)rate];
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

                _sound.CopyBuffer(new int[_sndBuffLength >> 2]);

                if (!_sound.Unlock()) return;

                _sound.Reset();

                if (!_mute)
                {
                    _sound.Play();
                }

                _initialized = true;
            }

            _audioPause = false;
            _modules.CoCo.SetAudioRate(_kHzRate[(int)rate]);

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
            _modules.CoCo.SetAudioRate(_kHzRate[(int)CurrentRate]);

            if (_initialized)
            {
                _sound.Reset();
            }

            _buffOffset = 0;
        }

        public void FlushAudioBuffer(int[] buffer, int length)
        {
            int leftAverage = buffer[0] >> 16;
            int rightAverage = buffer[0] & 0xFFFF;

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
            int playCursor = 0;

            if (!_initialized || _audioPause || _mute)
            {
                return Define.AUDIOBUFFERS;
            }

            if (!_mute)
            {
                playCursor = _sound.ReadPlayCursor();
            }

            long maxSize = _buffOffset <= playCursor ? playCursor - _buffOffset : _sndBuffLength - _buffOffset + playCursor;

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
