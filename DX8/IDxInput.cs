using System;
using System.Collections.Generic;

namespace DX8;

public interface IDxInput
{
    void CreateDirectInput(IntPtr handle);

    void EnumerateDevices();

    List<string> JoystickList();

    IDxJoystickState JoystickPoll(int index);
}
