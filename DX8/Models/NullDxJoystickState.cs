namespace DX8.Models;

/// <summary>
/// Use this to set the state when there isn't really a state.
/// </summary>
public class NullDxJoystickState : IDxJoystickState
{
    public JoystickStateErrorCodes ErrorCode => JoystickStateErrorCodes.Ok;
    public int Horizontal => 0;
    public int Vertical => 0;
    public bool X => false;
    public bool Y => false;
    public bool A => false;
    public bool B => false;
    public bool LB => false;
    public bool RB => false;
    public bool Back => false;
    public bool Start => false;
}
