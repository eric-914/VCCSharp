using System;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;

namespace VCCSharp.Models.Configuration.Support
{
    public class RangeSelect<T> where T : struct
    {
        private readonly bool _usesUnderscore;
        private readonly T[] _range;

        public RangeSelect(bool usesUnderscore = false)
        {
            _usesUnderscore = usesUnderscore;
            _range = (T[])Enum.GetValues(typeof(T));
            Value = _range.First();
        }

        [JsonProperty("(Options)")]
        public virtual string Options => $"{{{string.Join(',', _range.Select(Clean))}}}";

        [JsonIgnore]
        public T Value { get; set; }

        public virtual string Selected
        {
            get => _usesUnderscore ? Clean(Value) : Value.ToString() ?? string.Empty;
            set => Value = Enum.Parse<T>(_usesUnderscore ? $"_{value}" : value);
        }

        private static string Clean(T value) => Clean(value.ToString() ?? string.Empty);
        private static string Clean(string value) => value.StartsWith("_") ? value[1..] : value;
    }

    public class RangeSelect 
    {
        private readonly int[] _range;

        public RangeSelect(params int[] range)
        {
            _range = range;
            Value = _range.First();
        }

        [JsonProperty("(Options)")]
        public string Comments => $"{{{_range.First()}-{_range.Last()}}}";

        [JsonIgnore]
        public int Value { get; set; }

        public virtual string Selected
        {
            get => Value.ToString();
            set => Value = int.Parse(value);
        }
    }

    public class KeySelect : RangeSelect<Key>
    {
        [JsonIgnore]
        public override string Options => "See System.Windows.Input.Key";
    }
}
