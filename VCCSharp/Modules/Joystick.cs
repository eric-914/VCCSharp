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
        //--Goal is to bring the horizontal/vertical direction to within CoCo's 0-63 range.
        private const int MIN = 0;
        private const int MAX = 63;
        private const int CENTER = 32;

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
            var handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

            _input.CreateDirectInput(handle);

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

            if (_left.InputSource == JoystickDevices.Mouse)
            {
                switch (buttonStatus)
                {
                    case 0:
                        Left.Button1 = false;
                        break;

                    case 1:
                        Left.Button1 = true;
                        break;

                    case 2:
                        Left.Button2 = false;
                        break;

                    case 3:
                        Left.Button2 = true;
                        break;
                }
            }

            if (_right.InputSource == JoystickDevices.Mouse)
            {
                switch (buttonStatus)
                {
                    case 0:
                        Right.Button1 = false;
                        break;

                    case 1:
                        Right.Button1 = true;
                        break;

                    case 2:
                        Right.Button2 = false;
                        break;

                    case 3:
                        Right.Button2 = true;
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

            if (x > MAX)
            {
                x = MAX;
            }

            if (y > MAX)
            {
                y = MAX;
            }

            if (_left.InputSource == JoystickDevices.Mouse)
            {
                Left.X = x;
                Left.Y = y;
            }

            if (_right.InputSource == JoystickDevices.Mouse)
            {
                Right.X = x;
                Right.Y = y;
            }
        }

        public byte SetMouseStatus(byte scanCode, byte phase)
        {
            byte retValue = scanCode;

            switch (phase)
            {
                case 0:
                    if (_left.InputSource == JoystickDevices.Keyboard)
                    {
                        if (scanCode == _left.Left)
                        {
                            Left.X = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _left.Right)
                        {
                            Left.X = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _left.Up)
                        {
                            Left.Y = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _left.Down)
                        {
                            Left.Y = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _left.Fire1)
                        {
                            Left.Button1 = false;
                            retValue = 0;
                        }

                        if (scanCode == _left.Fire2)
                        {
                            Left.Button2 = false;
                            retValue = 0;
                        }
                    }

                    if (_right.InputSource == JoystickDevices.Keyboard)
                    {
                        if (scanCode == _right.Left)
                        {
                            Right.X = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _right.Right)
                        {
                            Right.X = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _right.Up)
                        {
                            Right.Y = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _right.Down)
                        {
                            Right.Y = CENTER;
                            retValue = 0;
                        }

                        if (scanCode == _right.Fire1)
                        {
                            Right.Button1 = false;
                            retValue = 0;
                        }

                        if (scanCode == _right.Fire2)
                        {
                            Right.Button2 = false;
                            retValue = 0;
                        }
                    }
                    break;

                case 1:
                    if (_left.InputSource == JoystickDevices.Keyboard)
                    {
                        if (scanCode == _left.Left)
                        {
                            Left.X = MIN;
                            retValue = 0;
                        }

                        if (scanCode == _left.Right)
                        {
                            Left.X = MAX;
                            retValue = 0;
                        }

                        if (scanCode == _left.Up)
                        {
                            Left.Y = MIN;
                            retValue = 0;
                        }

                        if (scanCode == _left.Down)
                        {
                            Left.Y = MAX;
                            retValue = 0;
                        }

                        if (scanCode == _left.Fire1)
                        {
                            Left.Button1 = true;
                            retValue = 0;
                        }

                        if (scanCode == _left.Fire2)
                        {
                            Left.Button2 = true;
                            retValue = 0;
                        }
                    }

                    if (_right.InputSource == JoystickDevices.Keyboard)
                    {
                        if (scanCode == _right.Left)
                        {
                            Right.X = MIN;
                            retValue = 0;
                        }

                        if (scanCode == _right.Right)
                        {
                            Right.X = MAX;
                            retValue = 0;
                        }

                        if (scanCode == _right.Up)
                        {
                            Right.Y = MIN;
                            retValue = 0;
                        }

                        if (scanCode == _right.Down)
                        {
                            Right.Y = MAX;
                            retValue = 0;
                        }

                        if (scanCode == _right.Fire1)
                        {
                            Right.Button1 = true;
                            retValue = 0;
                        }

                        if (scanCode == _right.Fire2)
                        {
                            Right.Button2 = true;
                            retValue = 0;
                        }
                    }
                    break;
            }

            return retValue;
        }
    }
}
