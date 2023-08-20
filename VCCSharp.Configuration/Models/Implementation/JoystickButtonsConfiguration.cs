using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Models.Implementation;

namespace VCCSharp.Models.Configuration.Support;

internal class JoystickButtonsConfiguration : IJoystickButtonsConfiguration
{
    private readonly Dictionary<int, Func<IKeySelectConfiguration>> _get;

    public JoystickButtonsConfiguration()
    {
        _get = new Dictionary<int, Func<IKeySelectConfiguration>>
        {
            { 0, () => _1},
            { 1, () => _2},
        };
    }

    public IKeySelectConfiguration this[int index] => _get[index]();

    [JsonIgnore]
    public IKeySelectConfiguration _1 { get; } = new KeySelectConfiguration { Value = Key.D0 };

    [JsonProperty("1")]
    public virtual string _1Text
    {
        get => _1.Selected;
        set => _1.Selected = value;
    }

    [JsonIgnore]
    public IKeySelectConfiguration _2 { get; } = new KeySelectConfiguration { Value = Key.Decimal };

    [JsonProperty("2")]
    public virtual string _2Text
    {
        get => _2.Selected;
        set => _2.Selected = value;
    }
}
