namespace VCCSharp.Models
{
    public class JoystickState
    {
        public int X { get; set; } = 32;
        public int Y { get; set; } = 32;
        public int Button1 { get; set; }
        public int Button2 { get; set; }
    }

    public class JoystickStates
    {
        public byte LeftStickNumber;
        public JoystickState Left = new JoystickState();

        public byte RightStickNumber;
        public JoystickState Right = new JoystickState();
    }
}
