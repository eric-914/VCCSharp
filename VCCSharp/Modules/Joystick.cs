using DX8;
using System;
using System.Collections.Generic;
using System.Drawing;
using VCCSharp.Libraries;
using VCCSharp.Libraries.Models;
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

        void SetStickNumbers(byte leftStickNumber, byte rightStickNumber);
        void SetButtonStatus(byte side, byte state);
        void SetJoystick(RECT clientSize, Point point);
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

        public JoystickState Left { get; private set; } = new JoystickState();
        public JoystickState Right { get; private set; } = new JoystickState();

        private JoystickModel _left = new JoystickModel();
        private JoystickModel _right = new JoystickModel();

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

        public void SetStickNumbers(byte leftStickNumber, byte rightStickNumber)
        {
            _leftId = leftStickNumber;
            _rightId = rightStickNumber;
        }

        public int get_pot_value(byte pot)
        {
            bool useLeft = _left.UseMouse == 3;
            bool useRight = _right.UseMouse == 3;

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

        //0=left 1=right
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

            if (right.UseMouse == 1)
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

        public void SetJoystick(RECT clientSize, Point point)
        {
            int dx = (clientSize.right - clientSize.left) >> 6;
            int dy = (clientSize.bottom - clientSize.top - 20) >> 6;

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

            if (left.UseMouse == 1)
            {
                Left.X = x;
                Left.Y = y;
            }

            if (right.UseMouse == 1)
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
                    if (left.UseMouse == 0)
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

                    if (right.UseMouse == 0)
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
                    if (left.UseMouse == 0)
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

                    if (right.UseMouse == 0)
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
