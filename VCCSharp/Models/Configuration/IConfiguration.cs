﻿namespace VCCSharp.Models.Configuration;

public interface IConfiguration
{
    Version Version { get; }
    IWindowConfiguration Window { get; }
    ICPUConfiguration CPU { get; }
    IAudioConfiguration Audio { get; }
    IVideoConfiguration Video { get; }
    IMemoryConfiguration Memory { get; }
    IKeyboardConfiguration Keyboard { get; }
    Joysticks Joysticks { get; }
    FilePaths FilePaths { get; }
    Startup Startup { get; }
    Accessories Accessories { get; }
    CassetteRecorder CassetteRecorder { get; }
    SerialPort SerialPort { get; }
}
