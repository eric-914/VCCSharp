using DX8.Models;
using System;
using System.Collections.Generic;

namespace DX8;

public interface IDxInput
{
    void CreateDirectInput(IntPtr handle);

    void EnumerateDevices();

    IEnumerable<IDxDevice> JoystickList();

    IDxJoystickState JoystickPoll(IDxDevice index);
}
