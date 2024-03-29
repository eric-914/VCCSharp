﻿using DX8;
using VCCSharp.Configuration;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Joystick;
using VCCSharp.Shared.Dx;

namespace VCCSharp.Modules;

public interface IJoysticks : IModule
{
    void FindJoysticks();

    List<DxJoystick> JoystickList { get; }

    void SetButtonStatus(MouseButtonStates state);
    void SetJoystick(System.Windows.Size clientSize, System.Windows.Point point);
    byte SetJoystickFromKeyboard(byte scanCode, bool keyDown);

    int GetPotValue(Pots pot);

    ushort StickValue { get; set; }

    JoystickState Left { get; }
    JoystickState Right { get; }

    void Configure(IJoysticksConfiguration configuration);
}

public class Joysticks : IJoysticks
{
    //--Goal is to bring the horizontal/vertical direction to within CoCo's 0-63 range.
    private const int Max = 63;

    private readonly IModules _modules;
    private readonly IDxManager _manager;
    private readonly IKeyboardAsJoystick _keyboardHandler;

    private IConfiguration Configuration => _modules.Configuration;

    public ushort StickValue { get; set; }

    public List<DxJoystick> JoystickList { get; private set; } = new();
    public JoystickState Left { get; private set; } = new();
    public JoystickState Right { get; private set; } = new();

    public Joysticks(IModules modules, IDxManager manager, IKeyboardAsJoystick keyboardHandler)
    {
        _modules = modules;
        _manager = manager;
        _keyboardHandler = keyboardHandler;
    }

    public void FindJoysticks()
    {
        JoystickList = _manager.Devices;
    }

    private IDxJoystickState JoystickPoll(int index)
    {
        return _manager.State(index);
    }

    public int GetPotValue(Pots pot)
    {
        var left = Configuration.Joysticks.Left;
        var right = Configuration.Joysticks.Right;

        if (left.InputSource.Value == JoystickDevices.Joystick)
        {
            Left = new JoystickState(JoystickPoll(left.DeviceIndex));
        }

        if (right.InputSource.Value == JoystickDevices.Joystick)
        {
            Right = new JoystickState(JoystickPoll(right.DeviceIndex));
        }

        return pot switch
        {
            Pots.RightX => Right.X,
            Pots.RightY => Right.Y,
            Pots.LeftX => Left.X,
            Pots.LeftY => Left.Y,
            _ => 0
        };
    }

    public void SetButtonStatus(MouseButtonStates state)
    {
        var map =
            new Dictionary<MouseButtonStates, Action>
            {
                { MouseButtonStates.LeftUp, () => SetButtonStatus(0, 0) },
                { MouseButtonStates.LeftDown, () => SetButtonStatus(0, 1) },
                { MouseButtonStates.RightUp, () => SetButtonStatus(1, 0) },
                { MouseButtonStates.RightDown, () => SetButtonStatus(1, 1) }
            };

        map[state]();
    }

    //0=left 1=right
    private void SetButtonStatus(byte side, byte state)
    {
        byte buttonStatus = (byte)((side << 1) | state);

        if (Configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Mouse)
        {
            switch (buttonStatus)
            {
                case 0:
                    Left.Button1 = false;
                    break;

                case 1:
                    Left.Button1 = true;
                    break;

                case 2:
                    Left.Button2 = false;
                    break;

                case 3:
                    Left.Button2 = true;
                    break;
            }
        }

        if (Configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Mouse)
        {
            switch (buttonStatus)
            {
                case 0:
                    Right.Button1 = false;
                    break;

                case 1:
                    Right.Button1 = true;
                    break;

                case 2:
                    Right.Button2 = false;
                    break;

                case 3:
                    Right.Button2 = true;
                    break;
            }
        }
    }

    public void SetJoystick(System.Windows.Size clientSize, System.Windows.Point point)
    {
        int dx = (int)clientSize.Width >> 6;
        int dy = (int)clientSize.Height >> 6;

        if (dx > 0) point.X /= dx;
        if (dy > 0) point.Y /= dy;

        ushort x = (ushort)point.X;
        ushort y = (ushort)point.Y;

        if (x > Max)
        {
            x = Max;
        }

        if (y > Max)
        {
            y = Max;
        }

        if (Configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Mouse)
        {
            Left.X = x;
            Left.Y = y;
        }

        if (Configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Mouse)
        {
            Right.X = x;
            Right.Y = y;
        }
    }

    public byte SetJoystickFromKeyboard(byte scanCode, bool keyDown)
    {
        return _keyboardHandler.SetJoystickFromKeyboard(scanCode, keyDown, Left, Right);
    }

    public void Configure(IJoysticksConfiguration configuration)
    {
        FindJoysticks();

        var count = JoystickList.Count;

        if (configuration.Left.DeviceIndex >= count)
        {
            configuration.Left.DeviceIndex = -1;
        }

        if (configuration.Right.DeviceIndex >= count)
        {
            configuration.Right.DeviceIndex = -1;
        }

        if (count == 0)	//Use Mouse input if no Joysticks present
        {
            if (configuration.Left.InputSource.Value == JoystickDevices.Joystick)
            {
                configuration.Left.InputSource.Value = JoystickDevices.Mouse;
            }

            if (configuration.Right.InputSource.Value == JoystickDevices.Joystick)
            {
                configuration.Left.InputSource.Value = JoystickDevices.Mouse;
            }
        }
    }

    public void ModuleReset()
    {
    }
}
