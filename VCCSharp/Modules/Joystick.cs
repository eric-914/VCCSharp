﻿//using Microsoft.DirectX.DirectInput;

using System;
using System.Linq;
using System.Runtime.InteropServices;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.DirectX;
using HRESULT = System.IntPtr;
using JoystickState = VCCSharp.Models.JoystickState;
using LPVOID = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IJoystick
    {
        short FindJoysticks();

        JoystickModel GetLeftJoystick();
        JoystickModel GetRightJoystick();

        void SetLeftJoystick(JoystickModel model);
        void SetRightJoystick(JoystickModel model);

        void SetStickNumbers(byte leftStickNumber, byte rightStickNumber);
        void SetButtonStatus(byte side, byte state);
        void SetJoystick(ushort x, ushort y);
        byte SetMouseStatus(byte scanCode, byte phase);

        ushort get_pot_value(byte pot);

        ushort StickValue { get; set; }
        JoystickState State { get; set; }
    }

    public class Joystick : IJoystick
    {
        private readonly IDInput _dInput;

        public ushort StickValue { get; set; }

        public short NumberOfJoysticks { get; private set; }
        public JoystickDevice[] Joysticks { get; private set; }

        public JoystickState State { get; set; } = new JoystickState();
        private JoystickModel _left = new JoystickModel();
        private JoystickModel _right = new JoystickModel();

        private readonly unsafe void* _pollStick = GetPollStick();

        private IDirectInput _di;

        public Joystick(IDInput dInput)
        {
            _dInput = dInput;
        }

        public short FindJoysticks()
        {
            _di = CreateDirectInput();

            Joysticks = new JoystickDevice[Define.MAX_JOYSTICKS];

            unsafe
            {
                int EnumerateCallback(DIDEVICEINSTANCE* p, void* v)
                {
                    IDirectInputDevice joystick = null;

                    _GUID guidInstance = p->guidInstance;

                    long hr = _di.CreateDevice(guidInstance, ref joystick, IntPtr.Zero);

                    if (hr < 0)
                    {
                        throw new Exception($"Failed to create device #{NumberOfJoysticks}");
                    }

                    Joysticks[NumberOfJoysticks] = new JoystickDevice
                    {
                        Device = joystick,
                        Name = $"{NumberOfJoysticks + 1}. {Converter.ToString(p->tszInstanceName)}"
                    };

                    return ++NumberOfJoysticks < Define.MAX_JOYSTICKS ? Define.TRUE : Define.FALSE;
                }

                long hr = _di.EnumDevices(Define.DI8DEVCLASS_GAMECTRL, EnumerateCallback, IntPtr.Zero, Define.DIEDFL_ATTACHEDONLY);

                if (hr < 0)
                {
                    throw new Exception("Failed to enumerate joysticks");
                }
            }

            Joysticks = Joysticks.Take(NumberOfJoysticks).ToArray();

            DIDATAFORMAT df = Library.Joystick.GetDataFormat();

            unsafe
            {
                DIDATAFORMAT df2 = new DIDATAFORMAT
                {
                    dwDataSize = 272,
                    dwFlags = 1,
                    dwNumObjs = 164,
                    dwObjSize = 24,
                    dwSize = 32,
                    rgodf = new DIOBJECTDATAFORMAT
                    {
                        dwFlags = 0,
                        dwOfs = 0,
                        dwType = 4,
                        pguid = df.rgodf.pguid
                        //new _GUID
                        //{
                        //    Data1 = 0,
                        //    Data2 = 0xF000,
                        //    Data3 = 0xD4DF
                        //}
                    }
                };

                for (byte index = 0; index < NumberOfJoysticks; index++)
                {
                    _GUID* DIPROP_RANGE = Library.Joystick.GetRangeGuid();

                    var device = Joysticks[index].Device;

                    int SetJoystickPropertiesCallback(DIDEVICEOBJECTINSTANCE* p, void* v)
                    {
                        DIPROPRANGE d;
                        d.diph.dwSize = (uint)sizeof(DIPROPRANGE);
                        d.diph.dwHeaderSize = (uint)sizeof(DIPROPHEADER);
                        d.diph.dwHow = Define.DIPH_BYID;
                        d.diph.dwObj = p->dwType;
                        d.lMin = 0;
                        d.lMax = 0xFFFF;

                        long hr = device.SetProperty(DIPROP_RANGE, &d.diph);

                        //--This will iterate a few times per joystick.
                        return hr < 0 ? Define.DIENUM_STOP : Define.DIENUM_CONTINUE;
                    }

                    long hr = device.SetDataFormat(&df);

                    if (hr < 0)
                    {
                        throw new Exception($"Failed to set data format on joystick #{index}");
                    }

                    hr = device.EnumObjects(SetJoystickPropertiesCallback, IntPtr.Zero, Define.DIDFT_AXIS);

                    if (hr < 0)
                    {
                        throw new Exception($"Failed to set properties on joystick #{index}");
                    }
                }
            }

            return NumberOfJoysticks;
        }

        private IDirectInput CreateDirectInput()
        {
            const uint version = 0x0800;

            LPVOID di = IntPtr.Zero;

            IntPtr handle = KernelDll.GetModuleHandleA(IntPtr.Zero);

            _GUID guid = CreateIDirectInput8AGuid();

            long hr = _dInput.DirectInputCreate(handle, version, guid, ref di);

            return hr < 0 ? null : (IDirectInput)Marshal.GetObjectForIUnknown(di);
        }

        private static unsafe _GUID CreateIDirectInput8AGuid()
        {
            byte[] d4 = { 0xAA, 0x99, 0x5D, 0x64, 0xED, 0x36, 0x97, 0x00 };

            _GUID guid = new _GUID
            {
                Data1 = 0xBF798030,
                Data2 = 0x483A,
                Data3 = 0x4DA2
            };

            for (int i = 0; i < 8; i++)
            {
                guid.Data4[i] = d4[i];
            }

            return guid;
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
            State.LeftStickNumber = leftStickNumber;
            State.RightStickNumber = rightStickNumber;
        }

        public ushort get_pot_value(byte pot)
        {
            unsafe
            {
                bool useLeft = _left.UseMouse == 3;
                bool useRight = _right.UseMouse == 3;

                if (useLeft)
                {
                    JoystickPoll(_pollStick, State.LeftStickNumber);

                    State.LeftStickX = (ushort)(Library.Joystick.StickX(_pollStick) >> 10);
                    State.LeftStickY = (ushort)(Library.Joystick.StickY(_pollStick) >> 10);
                    State.LeftButton1Status = (byte)(Library.Joystick.Button(_pollStick, 0) >> 7);
                    State.LeftButton2Status = (byte)(Library.Joystick.Button(_pollStick, 1) >> 7);
                }

                if (useRight)
                {
                    JoystickPoll(_pollStick, State.RightStickNumber);

                    State.RightStickX = (ushort)(Library.Joystick.StickX(_pollStick) >> 10);
                    State.RightStickY = (ushort)(Library.Joystick.StickY(_pollStick) >> 10);
                    State.RightButton1Status = (byte)(Library.Joystick.Button(_pollStick, 0) >> 7);
                    State.RightButton2Status = (byte)(Library.Joystick.Button(_pollStick, 1) >> 7);
                }

                switch (pot)
                {
                    case 0:
                        return State.RightStickX;

                    case 1:
                        return State.RightStickY;

                    case 2:
                        return State.LeftStickX;

                    case 3:
                        return State.LeftStickY;
                }

                return 0;
            }
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
                        State.LeftButton1Status = 0;
                        break;

                    case 1:
                        State.LeftButton1Status = 1;
                        break;

                    case 2:
                        State.LeftButton2Status = 0;
                        break;

                    case 3:
                        State.LeftButton2Status = 1;
                        break;
                }
            }

            if (right.UseMouse == 1)
            {
                switch (buttonStatus)
                {
                    case 0:
                        State.RightButton1Status = 0;
                        break;

                    case 1:
                        State.RightButton1Status = 1;
                        break;

                    case 2:
                        State.RightButton2Status = 0;
                        break;

                    case 3:
                        State.RightButton2Status = 1;
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
                State.LeftStickX = x;
                State.LeftStickY = y;
            }

            if (right.UseMouse == 1)
            {
                State.RightStickX = x;
                State.RightStickY = y;
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
                            State.LeftStickX = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            State.LeftStickX = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            State.LeftStickY = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            State.LeftStickY = 32;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            State.LeftButton1Status = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            State.LeftButton2Status = 0;
                            retValue = 0;
                        }
                    }

                    if (right.UseMouse == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            State.RightStickX = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            State.RightStickX = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            State.RightStickY = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            State.RightStickY = 32;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            State.RightButton1Status = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            State.RightButton2Status = 0;
                            retValue = 0;
                        }
                    }
                    break;

                case 1:
                    if (left.UseMouse == 0)
                    {
                        if (scanCode == left.Left)
                        {
                            State.LeftStickX = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Right)
                        {
                            State.LeftStickX = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Up)
                        {
                            State.LeftStickY = 0;
                            retValue = 0;
                        }

                        if (scanCode == left.Down)
                        {
                            State.LeftStickY = 63;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire1)
                        {
                            State.LeftButton1Status = 1;
                            retValue = 0;
                        }

                        if (scanCode == left.Fire2)
                        {
                            State.LeftButton2Status = 1;
                            retValue = 0;
                        }
                    }

                    if (right.UseMouse == 0)
                    {
                        if (scanCode == right.Left)
                        {
                            retValue = 0;
                            State.RightStickX = 0;
                        }

                        if (scanCode == right.Right)
                        {
                            State.RightStickX = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Up)
                        {
                            State.RightStickY = 0;
                            retValue = 0;
                        }

                        if (scanCode == right.Down)
                        {
                            State.RightStickY = 63;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire1)
                        {
                            State.RightButton1Status = 1;
                            retValue = 0;
                        }

                        if (scanCode == right.Fire2)
                        {
                            State.RightButton2Status = 1;
                            retValue = 0;
                        }
                    }
                    break;
            }

            return retValue;
        }

        public static unsafe void* GetPollStick()
        {
            return Library.Joystick.GetPollStick();
        }

        public unsafe HRESULT JoystickPoll(void* js, byte stickNumber)
        {
            return Library.Joystick.JoystickPoll(js, stickNumber);
        }
    }
}
