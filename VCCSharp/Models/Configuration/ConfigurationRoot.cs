﻿namespace VCCSharp.Models.Configuration;

public class ConfigurationRoot : IConfiguration
{
    public Version Version { get; } = new();
    public Window Window { get; } = new();

    public CPU CPU { get; } = new();
    public IAudioConfiguration Audio { get; } = new Audio();
    public Video Video { get; } = new();
    public Memory Memory { get; } = new();
    public Keyboard Keyboard { get; } = new();
    public Joysticks Joysticks { get; } = new();

    public FilePaths FilePaths { get; } = new();
    public Startup Startup { get; } = new();

    public Accessories Accessories { get; } = new();
    public SerialPort SerialPort { get; } = new();
    public CassetteRecorder CassetteRecorder { get; } = new();
}
