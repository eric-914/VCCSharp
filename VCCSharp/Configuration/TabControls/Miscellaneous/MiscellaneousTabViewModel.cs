using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.TabControls.Miscellaneous;

public class MiscellaneousTabViewModel
{
    private readonly IStartupConfiguration _model = new Startup();

    public bool AutoStart
    {
        get => _model.AutoStart;
        set => _model.AutoStart = value;
    }

    public bool CartAutoStart
    {
        get => _model.CartridgeAutoStart;
        set => _model.CartridgeAutoStart = value;
    }

    public MiscellaneousTabViewModel() { }

    public MiscellaneousTabViewModel(IStartupConfiguration model)
    {
        _model = model;
    }
}
