using DX8;

namespace VCCSharp.Models;

public class JoystickState
{
    public int X { get; set; } = 32;
    public int Y { get; set; } = 32;
    public bool Button1 { get; set; }
    public bool Button2 { get; set; }

    public JoystickState() { }

    public JoystickState(IDxJoystickState state)
    {
        X = state.Horizontal;
        Y = state.Vertical;

        Button1 = state.A;
        Button2 = state.B;
    }
}
