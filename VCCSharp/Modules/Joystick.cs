//using Microsoft.DirectX.DirectInput;
using VCCSharp.Libraries;
using VCCSharp.Models;
using JoystickState = VCCSharp.Models.JoystickState;

namespace VCCSharp.Modules
{
    public interface IJoystick
    {
        unsafe JoystickState* GetJoystickState();
        unsafe JoystickModel* GetLeftJoystick();
        unsafe JoystickModel* GetRightJoystick();

        unsafe void SetLeftJoystick(JoystickModel* model);
        unsafe void SetRightJoystick(JoystickModel* model);

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

        private unsafe JoystickModel* left = Library.Joystick.GetLeftJoystickModel();
        private unsafe JoystickModel* right = Library.Joystick.GetRightJoystickModel();

        public unsafe JoystickState* GetJoystickState()
        {
            return Library.Joystick.GetJoystickState();
        }

        public unsafe JoystickModel* GetLeftJoystick()
        {
            return left;
        }

        public unsafe JoystickModel* GetRightJoystick()
        {
            return right;
        }

        public unsafe void SetLeftJoystick(JoystickModel* model)
        {
            left = model;
        }

        public unsafe void SetRightJoystick(JoystickModel* model)
        {
            right = model;
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
                JoystickModel* left = GetLeftJoystick();
                JoystickModel* right = GetRightJoystick();

                if (left->UseMouse == 1)
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

                if (right->UseMouse == 1)
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
                JoystickModel* left = GetLeftJoystick();
                JoystickModel* right = GetRightJoystick();

                if (x > 63)
                {
                    x = 63;
                }

                if (y > 63)
                {
                    y = 63;
                }

                if (left->UseMouse == 1)
                {
                    instance->LeftStickX = x;
                    instance->LeftStickY = y;
                }

                if (right->UseMouse == 1)
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
                JoystickModel* left = GetLeftJoystick();
                JoystickModel* right = GetRightJoystick();

                switch (phase)
                {
                    case 0:
                        if (left->UseMouse == 0)
                        {
                            if (scanCode == left->Left)
                            {
                                instance->LeftStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == left->Right)
                            {
                                instance->LeftStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == left->Up)
                            {
                                instance->LeftStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == left->Down)
                            {
                                instance->LeftStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == left->Fire1)
                            {
                                instance->LeftButton1Status = 0;
                                retValue = 0;
                            }

                            if (scanCode == left->Fire2)
                            {
                                instance->LeftButton2Status = 0;
                                retValue = 0;
                            }
                        }

                        if (right->UseMouse == 0)
                        {
                            if (scanCode == right->Left)
                            {
                                instance->RightStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == right->Right)
                            {
                                instance->RightStickX = 32;
                                retValue = 0;
                            }

                            if (scanCode == right->Up)
                            {
                                instance->RightStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == right->Down)
                            {
                                instance->RightStickY = 32;
                                retValue = 0;
                            }

                            if (scanCode == right->Fire1)
                            {
                                instance->RightButton1Status = 0;
                                retValue = 0;
                            }

                            if (scanCode == right->Fire2)
                            {
                                instance->RightButton2Status = 0;
                                retValue = 0;
                            }
                        }
                        break;

                    case 1:
                        if (left->UseMouse == 0)
                        {
                            if (scanCode == left->Left)
                            {
                                instance->LeftStickX = 0;
                                retValue = 0;
                            }

                            if (scanCode == left->Right)
                            {
                                instance->LeftStickX = 63;
                                retValue = 0;
                            }

                            if (scanCode == left->Up)
                            {
                                instance->LeftStickY = 0;
                                retValue = 0;
                            }

                            if (scanCode == left->Down)
                            {
                                instance->LeftStickY = 63;
                                retValue = 0;
                            }

                            if (scanCode == left->Fire1)
                            {
                                instance->LeftButton1Status = 1;
                                retValue = 0;
                            }

                            if (scanCode == left->Fire2)
                            {
                                instance->LeftButton2Status = 1;
                                retValue = 0;
                            }
                        }

                        if (right->UseMouse == 0)
                        {
                            if (scanCode == right->Left)
                            {
                                retValue = 0;
                                instance->RightStickX = 0;
                            }

                            if (scanCode == right->Right)
                            {
                                instance->RightStickX = 63;
                                retValue = 0;
                            }

                            if (scanCode == right->Up)
                            {
                                instance->RightStickY = 0;
                                retValue = 0;
                            }

                            if (scanCode == right->Down)
                            {
                                instance->RightStickY = 63;
                                retValue = 0;
                            }

                            if (scanCode == right->Fire1)
                            {
                                instance->RightButton1Status = 1;
                                retValue = 0;
                            }

                            if (scanCode == right->Fire2)
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
