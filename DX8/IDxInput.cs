using System;
using System.Collections.Generic;

namespace DX8;

public interface IDxInput
{
    void CreateDirectInput(IntPtr handle);

    List<string> EnumerateDevices();

    IDxJoystickState JoystickPoll(int index);
}
