using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Audio
    {
        public string SoundDevice { get; set; } = "Primary Sound Driver";

        public RangeSelect<AudioRates> Rate { get; } = new RangeSelect<AudioRates>(true) { Value = AudioRates._44100Hz };
    }
}