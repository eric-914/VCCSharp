using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Models.Keyboard;
using VCCSharp.Shared.Configuration;

namespace VCCSharp.Models.Configuration.Support;

public class JoystickKeyMapping : ILeftJoystickKeyMapping, IRightJoystickKeyMapping
{
    //--Too many keys to list out, just go look at the source
    [JsonProperty("(Comment)")]
    public string Comment { get; } = "See System.Windows.Input.Key";

    //--Mainly just mapping 'default' to the number-pad.

    [JsonIgnore]
    public IKeySelect Left { get; } = new KeySelect { Value = Key.D4 };

    [JsonProperty("Left")]
    public virtual string LeftText
    {
        get => Left.Selected;
        set => Left.Selected = value;
    }

    [JsonIgnore]
    public IKeySelect Right { get; } = new KeySelect { Value = Key.D6 };

    [JsonProperty("Right")]
    public virtual string RightText
    {
        get => Right.Selected;
        set => Right.Selected = value;
    }

    [JsonIgnore]
    public IKeySelect Up { get; } = new KeySelect { Value = Key.D8 };

    [JsonProperty("Up")]
    public virtual string UpText
    {
        get => Up.Selected;
        set => Up.Selected = value;
    }

    [JsonIgnore]
    public IKeySelect Down { get; } = new KeySelect { Value = Key.D6 };

    [JsonProperty("Down")]
    public virtual string DownText
    {
        get => Down.Selected;
        set => Down.Selected = value;
    }

    public IJoystickButtons Buttons { get; } = new JoystickButtons();

    [JsonIgnore]
    public IEnumerable<string> KeyNames => KeyScanMapper.KeyText;
}
