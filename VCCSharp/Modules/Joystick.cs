using DX8;
using System;
using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IJoystick
    {
        List<string> FindJoysticks();

        JoystickModel GetLeftJoystick();
        JoystickModel GetRightJoystick();

        void SetLeftJoystick(JoystickModel model);
        void SetRightJoystick(JoystickModel model);

        void SetStickNumbers(int leftStickNumber, int rightStickNumber);
        void SetButtonStatus(MouseButtonStates state);
        void SetJoystick(System.Windows.Size clientSize, System.Windows.Point point);
        byte SetMouseStatus(byte scanCode, byte phase);

        int get_pot_value(byte pot);

        ushort StickValue { get; set; }

        JoystickState Left { get; }
        JoystickState Right { get; }
    }

    public class Joystick : IJoystick
    {
        private readonly IDxInput _input;

        public ushort StickValue { get; set; }

        public JoystickState Left { get; private set; } = new();
        public JoystickState Right { get; private set; } = new();

        private JoystickModel _left = new();
        private JoystickModel _right = new();

        private int _leftId;
        private int _rightId;

        public Joystick(IDxInput input)
        {
            _input = input;
        }

        public List<string> FindJoysticks()
        {
            _input.CreateDirectInput(KernelDll.GetModuleHandleA(IntPtr.Zero));

            return _input.EnumerateDevices();
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

        public void SetStickNumbers(int leftStickNumber, int rightStickNumber)
        {
            _leftId = leftStickNumber;
            _rightId = rightStickNumber;
        }

        public int get_pot_value(byte pot)
        {
            bool useLeft = _left.InputSource == JoystickDevices.Joystick;
            bool useRight = _right.InputSource == JoystickDevices.Joystick;

            if (useLeft)
            {
                Left = new JoystickState(_input.JoystickPoll(_leftId));
            }

            if (useRight)
            {
                Right = new JoystickState(_input.JoystickPoll(_rightId));
            }

            switch (pot)
            {
                case 0:
                    return Right.X;

                case 1:
                    return Right.Y;

                case 2:
                    return Left.X;

                case 3:
                    return Left.Y;
            }

            return 0;
        }

        public void SetButtonStatus(MouseButtonStates state)
        {
            var map =
                new Dictionary<MouseButtonStates, Action>
                {
                    { MouseButtonStates.LeftUp, () => SetButtonStatus(0, 0) },
                    { MouseButtonStates.LeftDown, () => SetButtonStatus(0, 1) },
                    { MouseButtonStates.RightUp, () => SetButtonStatus(1, 0) },
                    { MouseButtonStates.RightDown, () => SetButtonStatus(1, 1) },
                };

            map[state]();
        }

        //0=left 1=right
        private void SetButtonStatus(byte side, byte state)
        {
            byte buttonStatus = (byte)((side << 1) | state);

            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            if (left.InputSource == JoystickDevices.Mouse)
            {
                switch (buttonStatus)
                {
                    case 0:
                        Left.Button1 = 0;
                        break;

                    case 1:
                        Left.Button1 = 1;
                        break;

                    case 2:
                        Left.Button2 = 0;
                        break;

                    case 3:
                        Left.Button2 = 1;
                        break;
                }
            }

            if (right.InputSource == JoystickDevices.Mouse)
            {
                switch (buttonStatus)
                {
                    case 0:
                        Right.Button1 = 0;
                        break;

                    case 1:
                        Right.Button1 = 1;
                        break;

                    case 2:
                        Right.Button2 = 0;
                        break;

                    case 3:
                        Right.Button2 = 1;
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

            if (left.InputSource == JoystickDevices.Mouse)
            {
                Left.X = x;
                Left.Y = y;
            }

            if (right.InputSource == JoystickDevices.Mouse)
            {
                Right.X = x;
                Right.Y = y;
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
                    if (left.InputSource == 0)
                    {
                        if (scanCode == left.Left)
                        {
                            Left.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            Left.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            Left.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            Left.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            Left.Button1 = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            Left.Button2 = 0;
                            retValue = 0;
                        }
                    }

                    if (right.InputSource == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            Right.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            Right.X = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            Right.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            Right.Y = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            Right.Button1 = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            Right.Button2 = 0;
                            retValue = 0;
                        }
                    }
                    break;

                case 1:
                    if (left.InputSource == 0)
                    {
                        if (scanCode == left.Left)
                        {
                            Left.X = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            Left.X = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            Left.Y = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            Left.Y = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            Left.Button1 = 1;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            Left.Button2 = 1;
                            retValue = 0;
                        }
                    }

                    if (right.InputSource == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            retValue = 0;
                            Right.X = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            Right.X = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            Right.Y = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            Right.Y = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            Right.Button1 = 1;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            Right.Button2 = 1;
                            retValue = 0;
                        }
                    }
                    break;
            }

            return retValue;
        }
    }
}
