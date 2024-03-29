﻿using Newtonsoft.Json;
using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration.Support;

internal class MultiSlotsConfiguration : IMultiSlotsConfiguration
{
    private readonly Dictionary<int, Func<string>> _get;
    private readonly Dictionary<int, Action<string>> _set;

    public MultiSlotsConfiguration()
    {
        _get = new Dictionary<int, Func<string>>
        {
            { 0, () => _1 },
            { 1, () => _2 },
            { 2, () => _3 },
            { 3, () => _4 },
        };
        _set = new Dictionary<int, Action<string>>
        {
            { 0, v => _1 = v },
            { 1, v => _2 = v },
            { 2, v => _3 = v },
            { 3, v => _4 = v },
        };
    }

    public string this[int index]
    {
        get => _get[index]();
        set => _set[index](value);
    }

    [JsonProperty("1")]
    public string _1 { get; set; } = "";

    [JsonProperty("2")]
    public string _2 { get; set; } = "";

    [JsonProperty("3")]
    public string _3 { get; set; } = "";

    [JsonProperty("4")]
    public string _4 { get; set; } = ""; //="C:\CoCo\Mega-Bug (1982) (26-3076) (Tandy).ccc"
}
