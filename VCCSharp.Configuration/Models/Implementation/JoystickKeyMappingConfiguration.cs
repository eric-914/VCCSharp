﻿using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Configuration.Support;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

internal class JoystickKeyMappingConfiguration : IJoystickKeyMappingConfiguration
{
    //--Too many keys to list out, just go look at the source
    [JsonProperty("(Comment)")]
    public string Comment { get; } = "See System.Windows.Input.Key";

    //--Mainly just mapping 'default' to the number-pad.

    [JsonIgnore]
    public IKeySelectConfiguration Left { get; } = new KeySelectConfiguration { Value = Key.D4 };

    [JsonProperty("Left")]
    public virtual string LeftText
    {
        get => Left.Selected;
        set => Left.Selected = value;
    }

    [JsonIgnore]
    public IKeySelectConfiguration Right { get; } = new KeySelectConfiguration { Value = Key.D6 };

    [JsonProperty("Right")]
    public virtual string RightText
    {
        get => Right.Selected;
        set => Right.Selected = value;
    }

    [JsonIgnore]
    public IKeySelectConfiguration Up { get; } = new KeySelectConfiguration { Value = Key.D8 };

    [JsonProperty("Up")]
    public virtual string UpText
    {
        get => Up.Selected;
        set => Up.Selected = value;
    }

    [JsonIgnore]
    public IKeySelectConfiguration Down { get; } = new KeySelectConfiguration { Value = Key.D6 };

    [JsonProperty("Down")]
    public virtual string DownText
    {
        get => Down.Selected;
        set => Down.Selected = value;
    }

    public IJoystickButtonsConfiguration Buttons { get; } = new JoystickButtonsConfiguration();
}
