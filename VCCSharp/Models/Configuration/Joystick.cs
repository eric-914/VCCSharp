﻿using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Shared.Configuration;

namespace VCCSharp.Models.Configuration;

public class Joystick : IJoystickConfiguration
{
    public int DeviceIndex { get; set; } = -1;

    public IRangeSelect<JoystickDevices> InputSource { get; } = new RangeSelect<JoystickDevices> { Value = JoystickDevices.Joystick };

    public IRangeSelect<JoystickEmulations> Type { get; } = new RangeSelect<JoystickEmulations> { Value = JoystickEmulations.Standard };

    public IJoystickKeyMapping KeyMap { get; } = new JoystickKeyMapping();
}
