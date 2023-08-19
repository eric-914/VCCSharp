﻿using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.Configuration;

public interface IJoystickConfiguration : ILeft<IJoystickConfiguration>, IRight<IJoystickConfiguration>
{
    int DeviceIndex { get; set; }
    IRangeSelect<JoystickDevices> InputSource { get; }
    IRangeSelect<JoystickEmulations> Type { get; }
    IJoystickKeyMapping KeyMap { get; }
}
