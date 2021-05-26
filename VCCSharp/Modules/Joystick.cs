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
        byte SetMouseStatus(byte scanCode, byte phase);

        ushort StickValue { get; set; }
    }

    public class Joystick : IJoystick
    {
        public ushort StickValue { get; set; }

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
            Library.Joystick.get_pot_value();

            unsafe
            {
                JoystickState* instance = GetJoystickState();

                switch (pot)
                {
                    case 0:
                        return instance->RightStickX;

                    case 1:
                        return instance->RightStickY;

                    case 2:
                        return instance->LeftStickX;

                    case 3:
                        return instance->LeftStickY;
                }

                return 0;
            }
        }

        //0 = Left 1=right
        public void SetButtonStatus(byte side, byte state)
        {
            byte buttonStatus = (byte)((side << 1) | state);

            unsafe
            {
                JoystickState* instance = GetJoystickState();

                if (instance->Left->UseMouse == 1)
                {
                    switch (buttonStatus)
                    {
                        case 0:
                            instance->LeftButton1Status = 0;
                            break;

                        case 1:
                            instance->LeftButton1Status = 1;
                            break;

                        case 2:
                            instance->LeftButton2Status = 0;
                            break;

                        case 3:
                            instance->LeftButton2Status = 1;
                            break;
                    }
                }

                if (instance->Right->UseMouse == 1)
                {
                    switch (buttonStatus)
                    {
                        case 0:
                            instance->RightButton1Status = 0;
                            break;

                        case 1:
                            instance->RightButton1Status = 1;
                            break;

                        case 2:
                            instance->RightButton2Status = 0;
                            break;

                        case 3:
                            instance->RightButton2Status = 1;
                            break;
                    }
                }
            }
        }

        public void SetJoystick(ushort x, ushort y)
        {
            unsafe
            {
                JoystickState* instance = GetJoystickState();

                if (x > 63)
                {
                    x = 63;
                }

                if (y > 63)
                {
                    y = 63;
                }

                if (instance->Left->UseMouse == 1)
                {
                    instance->LeftStickX = x;
                    instance->LeftStickY = y;
                }

                if (instance->Right->UseMouse == 1)
                {
                    instance->RightStickX = x;
                    instance->RightStickY = y;
                }
            }
        }

        public byte SetMouseStatus(byte scanCode, byte phase)
        {
            byte retValue = scanCode;

            unsafe
            {
                JoystickState* instance = GetJoystickState();

                switch (phase)
                {
                    case 0:
                        if (instance->Left->UseMouse == 0)
                        {
                            if (scanCode == instance->Left->Left)
                            {
                                instance->LeftStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Right)
                            {
                                instance->LeftStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Up)
                            {
                                instance->LeftStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Down)
                            {
                                instance->LeftStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Fire1)
                            {
                                instance->LeftButton1Status = 0;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Fire2)
                            {
                                instance->LeftButton2Status = 0;
                                retValue = 0;
                            }
                        }

                        if (instance->Right->UseMouse == 0)
                        {
                            if (scanCode == instance->Right->Left)
                            {
                                instance->RightStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Right)
                            {
                                instance->RightStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Up)
                            {
                                instance->RightStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Down)
                            {
                                instance->RightStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Fire1)
                            {
                                instance->RightButton1Status = 0;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Fire2)
                            {
                                instance->RightButton2Status = 0;
                                retValue = 0;
                            }
                        }
                        break;

                    case 1:
                        if (instance->Left->UseMouse == 0)
                        {
                            if (scanCode == instance->Left->Left)
                            {
                                instance->LeftStickX = 0;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Right)
                            {
                                instance->LeftStickX = 63;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Up)
                            {
                                instance->LeftStickY = 0;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Down)
                            {
                                instance->LeftStickY = 63;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Fire1)
                            {
                                instance->LeftButton1Status = 1;
                                retValue = 0;
                            }

                            if (scanCode == instance->Left->Fire2)
                            {
                                instance->LeftButton2Status = 1;
                                retValue = 0;
                            }
                        }

                        if (instance->Right->UseMouse == 0)
                        {
                            if (scanCode == instance->Right->Left)
                            {
                                retValue = 0;
                                instance->RightStickX = 0;
                            }

                            if (scanCode == instance->Right->Right)
                            {
                                instance->RightStickX = 63;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Up)
                            {
                                instance->RightStickY = 0;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Down)
                            {
                                instance->RightStickY = 63;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Fire1)
                            {
                                instance->RightButton1Status = 1;
                                retValue = 0;
                            }

                            if (scanCode == instance->Right->Fire2)
                            {
                                instance->RightButton2Status = 1;
                                retValue = 0;
                            }
                        }
                        break;
                }
            }

            return retValue;
        }
    }
}
