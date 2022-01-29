using System;
using Newtonsoft.Json;
using VCCSharp.Enums;

namespace VCCSharp.Models.Configuration
{
    public class Keyboard
    {
        [JsonProperty("#Layout")]
        public string KeyboardLayoutComment { get; } = $"{{{KeyboardLayouts.CoCo},{KeyboardLayouts.Compact},{KeyboardLayouts.Count},{KeyboardLayouts.Custom},{KeyboardLayouts.Natural}}}";

        [JsonIgnore]
        public KeyboardLayouts KeyboardLayout { get; set; } = KeyboardLayouts.CoCo;

        public string Layout
        {
            get => KeyboardLayout.ToString();
            set => KeyboardLayout = Enum.Parse<KeyboardLayouts>(value);
        }
    }
}
