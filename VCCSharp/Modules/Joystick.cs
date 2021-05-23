//using Microsoft.DirectX.DirectInput;
using VCCSharp.Libraries;
using JoystickState = VCCSharp.Models.JoystickState;

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
            //DeviceList devices = Manager.GetDevices(
            //    DeviceClass.GameControl,
            //    EnumDevicesFlags.AttachedOnly);

            return Library.Joystick.EnumerateJoysticks();
        }

        public int InitJoyStick(byte stickNumber)
        {
            return Library.Joystick.InitJoyStick(stickNumber);
        }

        public void SetStickNumbers(byte leftStickNumber, byte rightStickNumber)
        {
            unsafe
            {
                JoystickState* instance = GetJoystickState();

                instance->LeftStickNumber = leftStickNumber;
                instance->RightStickNumber = rightStickNumber;
            }
        }
    }
}
