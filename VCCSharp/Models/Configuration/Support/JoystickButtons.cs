using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VCCSharp.Models.Configuration.Support
{
    public class JoystickButtons
    {
        private readonly Dictionary<int, Func<string>> _get;
        private readonly Dictionary<int, Action<string>> _set;

        public JoystickButtons()
        {
            _get = new Dictionary<int, Func<string>>
            {
                { 0, () => _1 },
                { 1, () => _2 },
            };
            _set = new Dictionary<int, Action<string>>
            {
                { 0, v => _1 = v },
                { 1, v => _2 = v },
            };
        }

        public string this[int index]
        {
            get => _get[index]();
            set => _set[index](value);
        }

        [JsonProperty("1")]
        public string _1 { get; set; } = "0";

        [JsonProperty("2")]
        public string _2 { get; set; } = ".";
    }
}
