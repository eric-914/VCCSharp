using DX8;
using DX8.Models;

namespace VCCSharp.Shared.Dx;

public class DxJoystick
{
    public IDxDevice Device { get; }

    public IDxJoystickState State { get; set; } = new NullDxJoystickState();

    internal DxJoystick(IDxDevice device)
    {
        Device = device;
    }
}
