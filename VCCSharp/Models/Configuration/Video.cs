using System;
using Newtonsoft.Json;
using VCCSharp.Enums;

namespace VCCSharp.Models.Configuration
{
    public class Video
    {
        [JsonProperty("#Monitor")]
        public string MonitorTypeComment { get; } = $"{{{MonitorTypes.Composite},{MonitorTypes.RGB}}}";

        [JsonIgnore]
        public MonitorTypes MonitorType { get; set; } = MonitorTypes.Composite;

        public string Monitor
        {
            get => MonitorType.ToString();
            set => MonitorType = Enum.Parse<MonitorTypes>(value);
        }

        public byte PaletteType { get; set; } = 1;

        public bool ScanLines { get; set; } = false;
        public bool ForceAspect { get; set; } = true;
    }
}