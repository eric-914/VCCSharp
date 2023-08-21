using VCCSharp.Configuration.Models;

namespace VCCSharp.Configuration.TabControls.Miscellaneous;

public interface IMiscellaneousTabViewModel
{
    bool AutoStart { get; set; }
    bool CartAutoStart { get; set; }
}

public abstract class MiscellaneousTabViewModelBase : IMiscellaneousTabViewModel
{
    private readonly IStartupConfiguration _model;

    protected MiscellaneousTabViewModelBase(IStartupConfiguration model)
    {
        _model = model;
    }

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
}

public class MiscellaneousTabViewModelStub : MiscellaneousTabViewModelBase
{
    public MiscellaneousTabViewModelStub() : base(ConfigurationFactory.StartupConfiguration()) { }
}

public class MiscellaneousTabViewModel : MiscellaneousTabViewModelBase
{
    public MiscellaneousTabViewModel(IStartupConfiguration model) : base(model) { }
}