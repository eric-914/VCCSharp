using DX8.Internal.Models;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
namespace DX8.Models;

/// <summary>
/// Works on translating DIJOYSTATE2 state into something that makes more sense when viewed as an Xbox controller.
/// </summary>
internal class DxXboxControllerState : IDxJoystickState 
{
    /// <summary>
    /// The D-Pad value can be any of the following
    /// </summary>
    private const int N = 0;
    private const int NE = 4500;
    private const int E = 9000;
    private const int SE = 13500;
    private const int S = 18000;
    private const int SW = 22500;
    private const int W = 27000;
    private const int NW = 31500;
    private const uint UNPRESSED = 0xFFFFFFFF;

    //--Goal is to bring the horizontal/vertical direction to within CoCo's 0-63 range.
    private const int MIN = 0;
    private const int MAX = 63;
    private const int CENTER = 32;

    public int Horizontal { get; }
    public int Vertical { get; }

    public bool X { get; }
    public bool Y { get; }
    public bool A { get; }
    public bool B { get; }

    public bool LB { get; }
    public bool RB { get; }

    public bool Back { get; }
    public bool Start { get; }


    private static readonly Dictionary<uint, int> _horizontal = new()
    {
        {N, CENTER},
        {NE, MAX},
        {E, MAX},
        {SE, MAX},
        {S, CENTER},
        {SW, MIN},
        {W, MIN},
        {NW, MIN}
    };

    private static readonly Dictionary<uint, int> _vertical = new()
    {
        {N, MIN},
        {NE, MIN},
        {E, CENTER},
        {SE, MAX},
        {S, MAX},
        {SW, MAX},
        {W, CENTER},
        {NW, MIN}
    };

    public DxXboxControllerState(DIJOYSTATE2 state)
    {
        var dPad = state.rgdwPOV.UI0;

        //--If the D-Pad isn't being pressed, use the stick values
        if (dPad == UNPRESSED)
        {
            Horizontal = state.lX >> 10;
            Vertical = state.lY >> 10;
        }
        else
        {
            Horizontal = _horizontal[state.rgdwPOV.UI0];
            Vertical = _vertical[state.rgdwPOV.UI0];
        }

        A = state.rgbButtons[0] != 0;
        B = state.rgbButtons[1] != 0;
        X = state.rgbButtons[2] != 0;
        Y = state.rgbButtons[3] != 0;

        LB = state.rgbButtons[4] != 0;
        RB = state.rgbButtons[5] != 0;
        Back = state.rgbButtons[6] != 0;
        Start = state.rgbButtons[7] != 0;
    }

    public override string ToString()
    {
        return $"({Horizontal},{Vertical}) X={X} Y={Y} A={A} B={B} LB={LB} RB={RB} Back={Back} Start={Start}";
    }
}

public class NullDxJoystickState : IDxJoystickState
{
    public int Horizontal => 0;
    public int Vertical => 0;
    public bool X => false;
    public bool Y => false;
    public bool A => false;
    public bool B => false;
    public bool LB => false;
    public bool RB => false;
    public bool Back => false;
    public bool Start => false;
}
