using System.Windows.Input;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Models.Joystick;

public interface IKeyboardAsJoystick
{
    byte SetJoystickFromKeyboard(byte scanCode, bool keyDown, JoystickState left, JoystickState right);
}

/// <summary>
/// Handles using the keyboard as the joystick source.
/// </summary>
public class KeyboardAsJoystick : IKeyboardAsJoystick
{
    private static readonly IKeyJoystickState KeyStateUp = new KeyJoystickStateUp();
    private static readonly IKeyJoystickState KeyStateDown = new KeyJoystickStateDown();

    private readonly IConfiguration _configuration;

    public KeyboardAsJoystick(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public byte SetJoystickFromKeyboard(byte scanCode, bool keyDown, JoystickState left, JoystickState right)
    {
        return keyDown
            ? SetJoystickFromKey(scanCode, KeyStateDown, left, right)
            : SetJoystickFromKey(scanCode, KeyStateUp, left, right);
    }

    private byte SetJoystickFromKey(byte scanCode, IKeyJoystickState keyState, JoystickState left, JoystickState right)
    {
        byte retValue = scanCode;

        if (_configuration.Joysticks.Left.InputSource.Value == JoystickDevices.Keyboard)
        {
            retValue = SetJoystickFromKey(retValue, _configuration.Joysticks.Left.KeyMap, left, keyState);
        }

        if (_configuration.Joysticks.Right.InputSource.Value == JoystickDevices.Keyboard)
        {
            retValue = SetJoystickFromKey(retValue, _configuration.Joysticks.Right.KeyMap, right, keyState);
        }

        return retValue;
    }

    private static byte SetJoystickFromKey(byte scanCode, JoystickKeyMapping keyMap, JoystickState state, IKeyJoystickState keyState)
    {
        byte retValue = scanCode;

        if (Compare(scanCode, keyMap.Left.Value))
        {
            state.X = keyState.XL;
            retValue = 0;
        }

        if (Compare(scanCode, keyMap.Right.Value))
        {
            state.X = keyState.XR;
            retValue = 0;
        }

        if (Compare(scanCode, keyMap.Up.Value))
        {
            state.Y = keyState.YU;
            retValue = 0;
        }

        if (Compare(scanCode, keyMap.Down.Value))
        {
            state.Y = keyState.YD;
            retValue = 0;
        }

        if (Compare(scanCode, keyMap.Buttons._1.Value))
        {
            state.Button1 = keyState.Button1;
            retValue = 0;
        }

        if (Compare(scanCode, keyMap.Buttons._2.Value))
        {
            state.Button2 = keyState.Button2;
            retValue = 0;
        }

        return retValue;
    }

    private static bool Compare(byte scanCode, Key key)
    {
        //Fix this if it ever gets invoked, I guess.
        throw new NotImplementedException();
    }
}
