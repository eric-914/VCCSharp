using DX8;
using System.Diagnostics;
using VCCSharp.Configuration;
using VCCSharp.Configuration.Options;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Audio;
using HWND = System.IntPtr;

namespace VCCSharp.Modules;

public interface IAudio : IModule
{
    List<string> FindSoundDevices();

    void Shutdown();

    void PauseAudio(bool pause);
    void ResetAudio();
    void FlushAudioBuffer(int[] buffer, int length);
    int GetFreeBlockCount();

    AudioSpectrum Spectrum { get; }
    AudioRates CurrentRate { get; }
}

public class Audio : IAudio
{
    private readonly IModules _modules;
    private readonly IDxSound _sound;

    public AudioSpectrum Spectrum { get; } = new();
    public AudioRates CurrentRate { get; private set; }

    private bool _initialized;
    private bool _audioPause;

    private ushort _bitRate;
    private ushort _blockSize;

    private readonly ushort[] _kHzRate = { 0, 11025, 22050, 44100 };

    private int _sndBuffLength;
    private int _buffOffset;

    private bool _mute;

    public List<string> FindSoundDevices() => _sound.EnumerateSoundCards();

    public Audio(IModules modules, IDxSound sound, IConfigurationManager configurationManager)
    {
        _modules = modules;
        _sound = sound;

        configurationManager.OnConfigurationSynch += OnConfigurationSynch;
    }

    public void Shutdown()
    {
        if (_initialized)
        {
            _initialized = false;

            _sound.Stop();
        }
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

        _modules.Audio.Spectrum.UpdateSoundBar(leftAverage, rightAverage);

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

    public void PauseAudio(bool pause)
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
    }

    public void ModuleReset()
    {
    }

    private void OnConfigurationSynch(SynchDirection direction, IConfiguration model)
    {
        Debug.WriteLine($"Audio:OnConfigurationSynch({direction})");

        if (direction == SynchDirection.SaveConfiguration) return;

        Shutdown(); //--Shutdown any previous audio configuration.  Initialized = false

        HWND hWnd = _modules.Emu.WindowHandle;
        AudioRates rate = model.Audio.Rate.Value;

        int deviceIndex = FindSoundDevices().IndexOf(model.Audio.Device);
        
        if (rate != AudioRates.Disabled)
        {
            //TODO: Since there is only 44100 or mute, remove the other options and make this a boolean
            //Force 44100 or Disabled
            rate = AudioRates._44100Hz;
        }

        CurrentRate = rate;

        _bitRate = _kHzRate[(int)rate];
        _blockSize = (ushort)(_bitRate * 4 / Define.TARGETFRAMERATE);
        _sndBuffLength = (ushort)(_blockSize * Define.AUDIOBUFFERS);

        if (!_sound.CreateDirectSound(deviceIndex)) return;

        // set cooperation level normal
        if (!_sound.SetCooperativeLevel(hWnd)) return;

        if (!_sound.CreateDirectSoundBuffer(_bitRate, _sndBuffLength)) return;

        // Clear out sound buffers
        if (!_sound.Lock(_buffOffset, (ushort)_sndBuffLength)) return;

        _sound.CopyBuffer(new int[_sndBuffLength >> 2]);

        if (!_sound.Unlock()) return;

        _initialized = true;

        ResetAudio();

        if (!_mute)
        {
            _sound.Play();
        }

        _audioPause = false;

        _mute = rate == AudioRates.Disabled;
    }
}
