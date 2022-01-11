using VCCSharp.Models.DirectX;

namespace VCCSharp.Models
{
    public class JoystickDevice
    {
        public string Name { get; set; }

        public IDirectInputDevice Device { get; set; }
    }
}
