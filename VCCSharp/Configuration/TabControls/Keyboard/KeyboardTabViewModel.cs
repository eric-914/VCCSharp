using Ninject;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.TabControls.Keyboard;

public class KeyboardTabViewModel
{
    private readonly IKeyboardConfiguration _model = ConfigurationFactory.KeyboardConfiguration();

    public List<string> KeyboardLayouts { get; } = new()
    {
        "Color Computer 1/2",
        "Color Computer 3",
        "PC"
    };

    public KeyboardTabViewModel() { }

    [Inject]
    public KeyboardTabViewModel(IKeyboardConfiguration model)
    {
        _model = model;
    }

    public KeyboardLayouts KeyboardLayout
    {
        get => _model.Layout.Value;
        set => _model.Layout.Value = value;
    }
}
