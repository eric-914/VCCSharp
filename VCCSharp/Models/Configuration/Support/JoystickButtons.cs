using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace VCCSharp.Models.Configuration.Support
{
    public class JoystickButtons
    {
        private readonly Dictionary<int, Func<KeySelect>> _get;

        public JoystickButtons()
        {
            _get = new Dictionary<int, Func<KeySelect>>
            {
                { 0, () => _1},
                { 1, () => _2},
            };
        }

        public KeySelect this[int index] => _get[index]();

        [JsonIgnore]
        public KeySelect _1 { get; } = new KeySelect { Value = Key.D0 };

        [JsonProperty("1")]
        public virtual string _1Text
        {
            get => _1.Selected;
            set => _1.Selected = value;
        }

        [JsonIgnore]
        public KeySelect _2 { get; } = new KeySelect { Value = Key.Decimal };

        [JsonProperty("2")]
        public virtual string _2Text
        {
            get => _2.Selected;
            set => _2.Selected = value;
        }
    }
}
