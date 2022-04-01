// ReSharper disable InconsistentNaming
namespace VCCSharp.Models.Joystick;

/// <summary>
/// Defines states when a keyboard is used to represent a joystick.
/// One for when a key is down.  Another for when the key is up.
/// </summary>
public interface IKeyJoystickState
{
    /// <summary>
    /// X-Axis, Left direction
    /// </summary>
    int XL { get; }

    /// <summary>
    /// X-Axis, Right direction
    /// </summary>
    int XR { get; }

    /// <summary>
    /// Y-Axis, Up direction
    /// </summary>
    int YU { get; }

    /// <summary>
    /// Y-Axis, Down direction
    /// </summary>
    int YD { get; }

    /// <summary>
    /// Button #1 up/down
    /// </summary>
    bool Button1 { get; }

    /// <summary>
    /// Button #2 up/down
    /// </summary>
    bool Button2 { get; }
}

public class KeyJoystickStateUp : IKeyJoystickState
{
    //--CoCo Joystick Center is 32 for both X/Y axes.
    private const int CENTER = 32;

    public int XL => CENTER;
    public int XR => CENTER;
    public int YU => CENTER;
    public int YD => CENTER;
    public bool Button1 => false;
    public bool Button2 => false;
}

public class KeyJoystickStateDown : IKeyJoystickState
{
    //--CoCo joystick min/max is 0/63 for both X/Y axes.
    private const int MIN = 0;
    private const int MAX = 63;

    public int XL => MIN;
    public int XR => MAX;
    public int YU => MIN;
    public int YD => MAX;
    public bool Button1 => true;
    public bool Button2 => true;
}