using DX8;

namespace VCCSharp.Models
{
    public class JoystickState
    {
        public int X { get; set; } = 32;
        public int Y { get; set; } = 32;
        public int Button1 { get; set; }
        public int Button2 { get; set; }

        public JoystickState() { }

        public JoystickState(IDxJoystickState state)
        {
            X = state.X >> 10;
            Y = state.Y >> 10;
            Button1 = state.Buttons[0] >> 7;
            Button2 = state.Buttons[1] >> 7;
        }
    }

    public class JoystickStates
    {
        public byte LeftStickNumber { get; set; }
        public JoystickState Left = new();

        public byte RightStickNumber { get; set; }
        public JoystickState Right = new();
    }
}
