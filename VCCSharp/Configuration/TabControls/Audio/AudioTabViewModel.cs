using Ninject;
using VCCSharp.Enums;
using VCCSharp.Models.Audio;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules;

namespace VCCSharp.Configuration.TabControls.Audio;

public class AudioTabViewModel
{
    private readonly IAudioConfiguration _model = new Models.Configuration.Audio();

    public AudioSpectrum Spectrum { get; } = new();

    public List<string> SoundDevices { get; } = new();

    public string SoundDevice 
    {
        get => _model.Device;
        set => _model.Device = value;
    }

    public List<string> SoundRates { get; } = new() { "Disabled", "11025 Hz", "22050 Hz", "44100 Hz" };
    public AudioRates AudioRate
    {
        get => _model.Rate.Value;
        set => _model.Rate.Value = value;
    }

    public AudioTabViewModel() { }

    [Inject]
    public AudioTabViewModel(IAudioConfiguration model, IAudio audio)
    {
        _model = model;

        SoundDevices = audio.FindSoundDevices();
        Spectrum = audio.Spectrum;
    }
}
