namespace VCCSharp.Models
{
    public class JoystickState
    {
        public byte LeftStickNumber;
        public byte LeftButton1Status;
        public byte LeftButton2Status;
        public ushort LeftStickX = 32;
        public ushort LeftStickY = 32;

        public byte RightStickNumber;
        public byte RightButton1Status;
        public byte RightButton2Status;
        public ushort RightStickX = 32;
        public ushort RightStickY = 32;
    }
}
