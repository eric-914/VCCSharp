using VCCSharp.Enums;

namespace VCCSharp.Models
{
    public class JoystickModel
    {
        public JoystickDevices InputSource { get; set; }

        // Index of which Joystick is selected
        public byte DeviceIndex { get; set; }

        public byte Up { get; set; }
        public byte Down { get; set; }
        public byte Left { get; set; }
        public byte Right { get; set; }
        public byte Fire1 { get; set; }
        public byte Fire2 { get; set; }
    }
}
