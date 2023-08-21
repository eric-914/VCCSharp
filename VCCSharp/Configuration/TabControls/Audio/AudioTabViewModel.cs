using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Models.Audio;
using VCCSharp.Modules;

namespace VCCSharp.Configuration.TabControls.Audio;

public interface IAudioTabViewModel
{
    AudioRates AudioRate { get; set; }
    string SoundDevice { get; set; }
    List<string> SoundDevices { get; }
    List<string> SoundRates { get; }
    AudioSpectrum Spectrum { get; }
}

public abstract class AudioTabViewModelBase : IAudioTabViewModel
{
    private readonly IAudioConfiguration _model;

    public AudioSpectrum Spectrum { get; }

    public List<string> SoundDevices { get; }

    public List<string> SoundRates { get; } = new() { "Disabled", "11025 Hz", "22050 Hz", "44100 Hz" };

    protected AudioTabViewModelBase(IAudioConfiguration model, AudioSpectrum spectrum, List<string> devices)
    {
        _model = model;

        SoundDevices = devices;
        Spectrum = spectrum;
    }

    public string SoundDevice
    {
        get => _model.Device;
        set => _model.Device = value;
    }

    public AudioRates AudioRate
    {
        get => _model.Rate.Value;
        set => _model.Rate.Value = value;
    }
}

public class AudioTabViewModelStub : AudioTabViewModelBase
{
    public AudioTabViewModelStub() : base(ConfigurationFactory.AudioConfiguration(), new AudioSpectrum(), new List<string>()) { }
}

public class AudioTabViewModel : AudioTabViewModelBase
{
    public AudioTabViewModel(IAudioConfiguration model, IAudio audio) : base(model, audio.Spectrum, audio.FindSoundDevices()) { }
}
