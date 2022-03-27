using System.Collections.Generic;
using VCCSharp.Enums;

namespace VCCSharp.Configuration.TabControls.Keyboard;

public class KeyboardTabViewModel
{
    private readonly Models.Configuration.Keyboard _model = new();

    public List<string> KeyboardLayouts { get; } = new()
    {
        "Color Computer 1/2",
        "Color Computer 3",
        "PC"
    };

    public KeyboardTabViewModel() { }

    public KeyboardTabViewModel(Models.Configuration.Keyboard model)
    {
        _model = model;
    }

    public KeyboardLayouts KeyboardLayout
    {
        get => _model.Layout.Value;
        set => _model.Layout.Value = value;
    }

}
