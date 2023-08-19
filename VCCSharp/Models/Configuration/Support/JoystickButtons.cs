#pragma warning disable IDE1006 // Naming Styles

using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Configuration.Models;
using VCCSharp.Shared.Configuration;

namespace VCCSharp.Models.Configuration.Support;

public class JoystickButtons : IJoystickButtons
{
    private readonly Dictionary<int, Func<IKeySelect>> _get;

    public JoystickButtons()
    {
        _get = new Dictionary<int, Func<IKeySelect>>
        {
            { 0, () => _1},
            { 1, () => _2},
        };
    }

    public IKeySelect this[int index] => _get[index]();

    [JsonIgnore]
    public IKeySelect _1 { get; } = new KeySelect { Value = Key.D0 };

    [JsonProperty("1")]
    public virtual string _1Text
    {
        get => _1.Selected;
        set => _1.Selected = value;
    }

    [JsonIgnore]
    public IKeySelect _2 { get; } = new KeySelect { Value = Key.Decimal };

    [JsonProperty("2")]
    public virtual string _2Text
    {
        get => _2.Selected;
        set => _2.Selected = value;
    }
}
