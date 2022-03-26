using DX8;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Joystick;

namespace VCCSharp.Modules;

public interface IJoysticks : IModule
{
    List<string> FindJoysticks();

    Joystick GetLeftJoystick();
    Joystick GetRightJoystick();

    void SetLeftJoystick();
    void SetRightJoystick();

    void SetStickNumbers(int leftStickNumber, int rightStickNumber);
    void SetButtonStatus(MouseButtonStates state);
    void SetJoystick(System.Windows.Size clientSize, System.Windows.Point point);
    byte SetMouseStatus(byte scanCode, byte phase);

    int GetPotValue(byte pot);

    ushort StickValue { get; set; }

    JoystickState Left { get; }
    JoystickState Right { get; }
}

public class Joysticks : IJoysticks
{
    //--Goal is to bring the horizontal/vertical direction to within CoCo's 0-63 range.
    private const int MIN = 0;
    private const int MAX = 63;
    private const int CENTER = 32;

    private readonly IConfiguration _configuration;
    private readonly IDxInput _input;

    public ushort StickValue { get; set; }

    public JoystickState Left { get; private set; } = new();
    public JoystickState Right { get; private set; } = new();

    private Joystick _left = new();
    private Joystick _right = new();

    private int _leftId = -1;
    private int _rightId = -1;

    public Joysticks(IConfiguration configuration, IDxInput input)
    {
        _configuration = configuration;
        _input = input;
    }

    public List<string> FindJoysticks()
    {
        var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

        _input.CreateDirectInput(handle);

        return _input.EnumerateDevices();
    }

    public Joystick GetLeftJoystick()
    {
        return _left;
    }

    public Joystick GetRightJoystick()
    {
        return _right;
    }

    public void SetLeftJoystick()
    {
        _left = _configuration.Joysticks.Left;
    }

    public void SetRightJoystick()
    {
        _right = _configuration.Joysticks.Right;
    }

    public void SetStickNumbers(int leftStickNumber, int rightStickNumber)
    {
        _leftId = leftStickNumber;
        _rightId = rightStickNumber;
    }

    public int GetPotValue(byte pot)
    {
        bool useLeft = _left.InputSource.Value == JoystickDevices.Joystick;
        bool useRight = _right.InputSource.Value == JoystickDevices.Joystick;

        if (useLeft && _leftId != -1)
        {
            Left = new JoystickState(_input.JoystickPoll(_leftId));
        }

        if (useRight && _rightId != -1)
        {
            Right = new JoystickState(_input.JoystickPoll(_rightId));
        }

        return pot switch
        {
            0 => Right.X,
            1 => Right.Y,
            2 => Left.X,
            3 => Left.Y,
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

        if (_left.InputSource.Value == JoystickDevices.Mouse)
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

        if (_right.InputSource.Value == JoystickDevices.Mouse)
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

        if (_left.InputSource.Value == JoystickDevices.Mouse)
        {
            Left.X = x;
            Left.Y = y;
        }

        if (_right.InputSource.Value == JoystickDevices.Mouse)
        {
            Right.X = x;
            Right.Y = y;
        }
    }

    public byte SetMouseStatus(byte scanCode, byte phase)
    {
        byte retValue = scanCode;

        switch (phase)
        {
            case 0:
                if (_left.InputSource.Value == JoystickDevices.Keyboard)
                {
                    if (Compare(scanCode, _left.KeyMap.Left.Value))
                    {
                        Left.X = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Right.Value))
                    {
                        Left.X = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Up.Value))
                    {
                        Left.Y = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Down.Value))
                    {
                        Left.Y = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Buttons._1.Value))
                    {
                        Left.Button1 = false;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Buttons._2.Value))
                    {
                        Left.Button2 = false;
                        retValue = 0;
                    }
                }

                if (_right.InputSource.Value == JoystickDevices.Keyboard)
                {
                    if (Compare(scanCode, _right.KeyMap.Left.Value))
                    {
                        Right.X = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Right.Value))
                    {
                        Right.X = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Up.Value))
                    {
                        Right.Y = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Down.Value))
                    {
                        Right.Y = CENTER;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Buttons._1.Value))
                    {
                        Right.Button1 = false;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Buttons._2.Value))
                    {
                        Right.Button2 = false;
                        retValue = 0;
                    }
                }
                break;

            case 1:
                if (_left.InputSource.Value == JoystickDevices.Keyboard)
                {
                    if (Compare(scanCode, _left.KeyMap.Left.Value))
                    {
                        Left.X = MIN;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Right.Value))
                    {
                        Left.X = MAX;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Up.Value))
                    {
                        Left.Y = MIN;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Down.Value))
                    {
                        Left.Y = MAX;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Buttons._1.Value))
                    {
                        Left.Button1 = true;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _left.KeyMap.Buttons._2.Value))
                    {
                        Left.Button2 = true;
                        retValue = 0;
                    }
                }

                if (_right.InputSource.Value == JoystickDevices.Keyboard)
                {
                    if (Compare(scanCode, _right.KeyMap.Left.Value))
                    {
                        Right.X = MIN;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Right.Value))
                    {
                        Right.X = MAX;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Up.Value))
                    {
                        Right.Y = MIN;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Down.Value))
                    {
                        Right.Y = MAX;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Buttons._1.Value))
                    {
                        Right.Button1 = true;
                        retValue = 0;
                    }

                    if (Compare(scanCode, _right.KeyMap.Buttons._2.Value))
                    {
                        Right.Button2 = true;
                        retValue = 0;
                    }
                }
                break;
        }

        return retValue;
    }

    private bool Compare(byte scanCode, Key key)
    {
        //Fix this if it ever gets invoked, I guess.
        throw new NotImplementedException();
    }

    public void Reset()
    {
        _leftId = -1;
        _rightId = -1;

        SetLeftJoystick();
        SetRightJoystick();
    }
}
