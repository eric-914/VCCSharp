using System;
using System.Collections.Generic;
using VCCSharp.DX8;
using VCCSharp.DX8.Interfaces;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IJoystick
    {
        int FindJoysticks();

        JoystickModel GetLeftJoystick();
        JoystickModel GetRightJoystick();

        void SetLeftJoystick(JoystickModel model);
        void SetRightJoystick(JoystickModel model);

        void SetStickNumbers(byte leftStickNumber, byte rightStickNumber);
        void SetButtonStatus(byte side, byte state);
        void SetJoystick(ushort x, ushort y);
        byte SetMouseStatus(byte scanCode, byte phase);

        int get_pot_value(byte pot);

        ushort StickValue { get; set; }
        JoystickStates States { get; set; }
    }

    public class Joystick : IJoystick
    {
        private readonly IDxInput _input;

        public ushort StickValue { get; set; }

        public JoystickDevice[] Joysticks { get; private set; }
        public JoystickStates States { get; set; } = new JoystickStates();

        private JoystickModel _left = new JoystickModel();
        private JoystickModel _right = new JoystickModel();

        public Joystick(IDxInput input)
        {
            _input = input;
        }

        public int FindJoysticks()
        {
            _input.CreateDirectInput(KernelDll.GetModuleHandleA(IntPtr.Zero));

            var joysticks = new List<JoystickDevice>();

            void Callback(IDirectInputDevice joystick, string name)
            {
                var device = new JoystickDevice
                {
                    Device = joystick,
                    Name = name
                };

                joysticks.Add(device);
            }

            _input.EnumerateDevices(Callback);

            Joysticks = joysticks.ToArray();

            for (byte index = 0; index < Joysticks.Length; index++)
            {
                IDirectInputDevice device = Joysticks[index].Device;

                _input.SetJoystickProperties(device);
            }

            return Joysticks.Length;
        }

        public JoystickModel GetLeftJoystick()
        {
            return _left;
        }

        public JoystickModel GetRightJoystick()
        {
            return _right;
        }

        public void SetLeftJoystick(JoystickModel model)
        {
            _left = model;
        }

        public void SetRightJoystick(JoystickModel model)
        {
            _right = model;
        }

        public void SetStickNumbers(byte leftStickNumber, byte rightStickNumber)
        {
            States.LeftStickNumber = leftStickNumber;
            States.RightStickNumber = rightStickNumber;
        }

        public int get_pot_value(byte pot)
        {
            bool useLeft = _left.UseMouse == 3;
            bool useRight = _right.UseMouse == 3;

            if (useLeft)
            {
                States.Left = _input.JoystickPoll(Joysticks[States.LeftStickNumber].Device);
            }

            if (useRight)
            {
                States.Right = _input.JoystickPoll(Joysticks[States.RightStickNumber].Device);
            }

            switch (pot)
            {
                case 0:
                    return States.Right.X;

                case 1:
                    return States.Right.Y;

                case 2:
                    return States.Left.X;

                case 3:
                    return States.Left.Y;
            }

            return 0;
        }

        //0 = Left 1=right
        public void SetButtonStatus(byte side, byte state)
        {
            byte buttonStatus = (byte)((side << 1) | state);

            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            if (left.UseMouse == 1)
            {
                switch (buttonStatus)
                {
                    case 0:
                        States.Left.Button1 = 0;
                        break;

                    case 1:
                        States.Left.Button1 = 1;
                        break;

                    case 2:
                        States.Left.Button2 = 0;
                        break;

                    case 3:
                        States.Left.Button2 = 1;
                        break;
                }
            }

            if (right.UseMouse == 1)
            {
                switch (buttonStatus)
                {
                    case 0:
                        States.Right.Button1 = 0;
                        break;

                    case 1:
                        States.Right.Button1 = 1;
                        break;

                    case 2:
                        States.Right.Button2 = 0;
                        break;

                    case 3:
                        States.Right.Button2 = 1;
                        break;
                }
            }
        }

        public void SetJoystick(ushort x, ushort y)
        {
            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            if (x > 63)
            {
                x = 63;
            }

            if (y > 63)
            {
                y = 63;
            }

            if (left.UseMouse == 1)
            {
                States.Left.X = x;
                States.Left.Y = y;
            }

            if (right.UseMouse == 1)
            {
                States.Right.X = x;
                States.Right.Y = y;
            }
        }

        public byte SetMouseStatus(byte scanCode, byte phase)
        {
            byte retValue = scanCode;

            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            switch (phase)
            {
                case 0:
                    if (left.UseMouse == 0)
                    {
                        if (scanCode == left.Left)
                        {
                            States.Left.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            States.Left.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            States.Left.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            States.Left.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            States.Left.Button1 = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            States.Left.Button2 = 0;
                            retValue = 0;
                        }
                    }

                    if (right.UseMouse == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            States.Right.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            States.Right.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            States.Right.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            States.Right.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            States.Right.Button1 = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            States.Right.Button2 = 0;
                            retValue = 0;
                        }
                    }
                    break;

                case 1:
                    if (left.UseMouse == 0)
                    {
                        if (scanCode == left.Left)
                        {
                            States.Left.X = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            States.Left.X = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            States.Left.Y = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            States.Left.Y = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            States.Left.Button1 = 1;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            States.Left.Button2 = 1;
                            retValue = 0;
                        }
                    }

                    if (right.UseMouse == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            retValue = 0;
                            States.Right.X = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            States.Right.X = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            States.Right.Y = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            States.Right.Y = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            States.Right.Button1 = 1;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            States.Right.Button2 = 1;
                            retValue = 0;
                        }
                    }
                    break;
            }

            return retValue;
        }
    }
}
