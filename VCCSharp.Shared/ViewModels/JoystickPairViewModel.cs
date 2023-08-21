using VCCSharp.Configuration.Support;

namespace VCCSharp.Shared.ViewModels;

public interface IJoystickPairViewModel
{
    JoystickConfigurationViewModel Left { get; }
    JoystickConfigurationViewModel Right { get; }
}

public abstract class JoystickPairViewModelBase : NotifyViewModel, IJoystickPairViewModel
{
    //[LeftJoyStick]
    public JoystickConfigurationViewModel Left { get; } = new();

    //[RightJoyStick]
    public JoystickConfigurationViewModel Right { get; } = new();

    protected JoystickPairViewModelBase(ILeft<JoystickConfigurationViewModel> left, IRight<JoystickConfigurationViewModel> right)
    {
        Left = (JoystickConfigurationViewModel)left;
        Right = (JoystickConfigurationViewModel)right;
    }
}

public class JoystickPairViewModelStub : JoystickPairViewModelBase
{
    public JoystickPairViewModelStub() : base(new JoystickConfigurationViewModel(), new JoystickConfigurationViewModel()) { }
}

public class JoystickPairViewModel : JoystickPairViewModelBase
{
    public JoystickPairViewModel(ILeft<JoystickConfigurationViewModel> left, IRight<JoystickConfigurationViewModel> right) : base(left, right) { }
}