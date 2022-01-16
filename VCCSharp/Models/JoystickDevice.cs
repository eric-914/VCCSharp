using VCCSharp.DX8.Interfaces;

namespace VCCSharp.Models
{
    public class JoystickDevice
    {
        public string Name { get; set; }

        public IDirectInputDevice Device { get; set; }
    }
}
