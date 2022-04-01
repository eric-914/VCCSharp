using DX8;
using System;
using System.Collections.Generic;
using System.Linq;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Joystick;

namespace VCCSharp.Modules;

public interface IJoysticks : IModule
{
    void FindJoysticks(bool refresh);

    List<string> JoystickList { get; }

    void SetButtonStatus(MouseButtonStates state);
    void SetJoystick(System.Windows.Size clientSize, System.Windows.Point point);
    byte SetJoystickFromKeyboard(byte scanCode, bool keyDown);

    IDxJoystickState JoystickPoll(int index);
    int GetPotValue(Pots pot);

    ushort StickValue { get; set; }

    JoystickState Left { get; }
    JoystickState Right { get; }
}

public class Joysticks : IJoysticks
{
    //--Goal is to bring the horizontal/vertical direction to within CoCo's 0-63 range.
    //private const int MIN = 0;
    private const int MAX = 63;
    //private const int CENTER = 32;

    private readonly IConfiguration _configuration;
    private readonly IDxInput _input;
    private readonly IKeyboardAsJoystick _keyboardHandler;

    public ushort StickValue { get; set; }

    public List<string> JoystickList { get; private set; } = new();
    public JoystickState Left { get; private set; } = new();
    public JoystickState Right { get; private set; } = new();

    public Joysticks(IConfiguration configuration, IDxInput input, IKeyboardAsJoystick keyboardHandler)
    {
        _configuration = configuration;
        _input = input;
        _keyboardHandler = keyboardHandler;
    }

    public void FindJoysticks(bool refresh)
    {
        if (!refresh && JoystickList.Any()) return;

        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        _input.EnumerateDevices();

        JoystickList = _input.JoystickList();
    }

    public IDxJoystickState JoystickPoll(int index) => _input.JoystickPoll(index);

    public int GetPotValue(Pots pot)
    {
        int leftId = _configuration.Joysticks.Left.DeviceIndex;
        int rightId = _configuration.Joysticks.Right.DeviceIndex;
        bool useLeft = _configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Joystick;
        bool useRight = _configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Joystick;

        if (useLeft && leftId != -1)
        {
            Left = new JoystickState(JoystickPoll(leftId));
        }

        if (useRight && rightId != -1)
        {
            Right = new JoystickState(JoystickPoll(rightId));
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

        if (_configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Mouse)
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

        if (_configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Mouse)
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

        if (x > MAX)
        {
            x = MAX;
        }

        if (y > MAX)
        {
            y = MAX;
        }

        if (_configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Mouse)
        {
            Left.X = x;
            Left.Y = y;
        }

        if (_configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Mouse)
        {
            Right.X = x;
            Right.Y = y;
        }
    }

    public byte SetJoystickFromKeyboard(byte scanCode, bool keyDown)
    {
        return _keyboardHandler.SetJoystickFromKeyboard(scanCode, keyDown, Left, Right);
    }

    public void Reset()
    {
    }
}
