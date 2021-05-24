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
        ushort get_pot_value(byte pot);
        void SetButtonStatus(byte side, byte state);
        void SetJoystick(ushort x, ushort y);
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

        public ushort get_pot_value(byte pot)
        {
            return Library.Joystick.get_pot_value(pot);
        }

        public void SetButtonStatus(byte side, byte state)
        {
            Library.Joystick.SetButtonStatus(side, state);
        }

        public void SetJoystick(ushort x, ushort y)
        {
            Library.Joystick.SetJoystick(x, y);
        }
    }
}
