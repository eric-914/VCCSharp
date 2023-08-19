﻿using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public class Video : IVideoConfiguration
{
    public RangeSelect<MonitorTypes> Monitor { get; } = new();

    public RangeSelect<PaletteTypes> Palette { get; } = new() { Value = PaletteTypes.Updated };

    public bool ScanLines { get; set; } = false;
    public bool ForceAspect { get; set; } = true;
}
