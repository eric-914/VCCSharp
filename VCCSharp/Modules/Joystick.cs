using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IJoystick
    {
        unsafe JoystickState* GetJoystickState();
        short EnumerateJoysticks();
        int InitJoyStick(byte stickNumber);
        void SetStickNumbers(byte leftStickNumber, byte rightStickNumber);
    }

    public class Joystick : IJoystick
    {
        public unsafe JoystickState* GetJoystickState()
        {
            return Library.Joystick.GetJoystickState();
        }

        public short EnumerateJoysticks()
        {
            return Library.Joystick.EnumerateJoysticks();
        }

        public int InitJoyStick(byte stickNumber)
        {
            return Library.Joystick.InitJoyStick(stickNumber);
        }

        public void SetStickNumbers(byte leftStickNumber, byte rightStickNumber)
        {
            Library.Joystick.SetStickNumbers(leftStickNumber, rightStickNumber);
        }
    }
}
