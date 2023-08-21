using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.TabControls.Keyboard;

public interface IKeyboardTabViewModel
{
    KeyboardLayouts KeyboardLayout { get; set; }
    List<string> KeyboardLayouts { get; }
}

public abstract class KeyboardTabViewModelBase : IKeyboardTabViewModel
{
    private readonly IKeyboardConfiguration _model;

    public List<string> KeyboardLayouts { get; } = new()
    {
        "Color Computer 1/2",
        "Color Computer 3",
        "PC"
    };

    protected KeyboardTabViewModelBase(IKeyboardConfiguration model)
    {
        _model = model;
    }

    public KeyboardLayouts KeyboardLayout
    {
        get => _model.Layout.Value;
        set => _model.Layout.Value = value;
    }
}

public class KeyboardTabViewModelStub : KeyboardTabViewModelBase
{
    public KeyboardTabViewModelStub() : base(ConfigurationFactory.KeyboardConfiguration()) { }
}

public class KeyboardTabViewModel : KeyboardTabViewModelBase
{
    public KeyboardTabViewModel(IKeyboardConfiguration model) : base(model) { }
}