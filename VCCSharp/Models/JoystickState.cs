namespace VCCSharp.Models
{
    public struct JoystickState
    {
        public byte LeftStickNumber;
        public byte LeftButton1Status;
        public byte LeftButton2Status;
        public ushort LeftStickX;
        public ushort LeftStickY;

        public byte RightStickNumber;
        public byte RightButton1Status;
        public byte RightButton2Status;
        public ushort RightStickX;
        public ushort RightStickY;

        public unsafe JoystickModel* Left;
        public unsafe JoystickModel* Right;
    }
}
