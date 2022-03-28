﻿using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.Models.Audio;
using VCCSharp.Modules;

namespace VCCSharp.Configuration.TabControls.Audio;

public class AudioTabViewModel
{
    private readonly Models.Configuration.Audio _model = new();

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

    public AudioTabViewModel(Models.Configuration.Audio model, IAudio audio)
    {
        _model = model;

        SoundDevices = audio.FindSoundDevices();
        Spectrum = audio.Spectrum;
    }
}