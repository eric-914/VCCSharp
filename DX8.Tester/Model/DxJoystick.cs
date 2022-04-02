using DX8.Models;

namespace DX8.Tester.Model;

public class DxJoystick
{
    public IDxDevice Device { get; }

    public IDxJoystickState State { get; set; } = new NullDxJoystickState();

    public DxJoystick(IDxDevice device)
    {
        Device = device;
    }
}
