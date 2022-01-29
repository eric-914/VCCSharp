using System;
using Newtonsoft.Json;
using VCCSharp.Enums;

namespace VCCSharp.Models.Configuration
{
    public class CPU
    {
        [JsonProperty("#Type")]
        public string CpuTypeComment { get; } = $"{{{CPUTypes.MC6809},{CPUTypes.HD6309}}}";

        [JsonIgnore]
        public CPUTypes CpuType { get; set; } = CPUTypes.MC6809;

        public string Type
        {
            get => CpuType.ToString();
            set => CpuType = Enum.Parse<CPUTypes>(value);
        }

        public bool ThrottleSpeed { get; set; } = true;
        public int CpuMultiplier { get; set; } = 227;
        public int FrameSkip { get; set; } = 1;
        public int MaxOverclock { get; set; } = 227;
    }
}
