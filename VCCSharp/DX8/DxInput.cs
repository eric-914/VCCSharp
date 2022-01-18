using System;
using System.Collections.Generic;
using System.Diagnostics;
using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using DX8.Converters;
using DX8.Formats;
using VCCSharp.Models;

namespace VCCSharp.DX8
{
    public delegate void EnumerateDevicesCallback(string name);

    public interface IDxInput
    {
        void CreateDirectInput(IntPtr handle);

        int EnumerateDevices(EnumerateDevicesCallback callback);

        JoystickState JoystickPoll(int id);
    }

    public class DxInput : IDxInput
    {
        private readonly IDInput _input;
        private readonly IDxFactory _factory;

        private IDirectInput _di;
        private readonly List<IDirectInputDevice> _devices = new List<IDirectInputDevice>();

        public DxInput(IDInput input, IDxFactory factory)
        {
            _input = input;
            _factory = factory;
        }

        public void CreateDirectInput(IntPtr handle)
        {
            const uint version = 0x0800;

            _GUID guid = GuidConverter.ToGuid(DxGuid.DirectInput);

            _di = _factory.CreateDirectInput(_input, handle, version, guid);
        }

        private IDirectInputDevice CreateDevice(_GUID guidInstance)
        {
            IDirectInputDevice joystick = null;

            long hr = _di.CreateDevice(guidInstance, ref joystick, IntPtr.Zero);

            if (hr < 0)
            {
                throw new Exception("Failed to create device");
            }

            return joystick;
        }

        public unsafe int EnumerateDevices(EnumerateDevicesCallback callback)
        {
            int count = 0;

            int EnumerateCallback(DIDEVICEINSTANCE* p, void* v)
            {
                IDirectInputDevice joystick = CreateDevice(p->guidInstance);
                string name = Converter.ToString(p->tszInstanceName);

                _devices.Add(joystick);

                callback(name);

                SetJoystickProperties(joystick);

                return ++count < Define.MAX_JOYSTICKS ? Define.TRUE : Define.FALSE;
            }

            long hr = _di.EnumDevices(Define.DI8DEVCLASS_GAMECTRL, EnumerateCallback, IntPtr.Zero, Define.DIEDFL_ATTACHEDONLY);

            if (hr < 0)
            {
                throw new Exception("Failed to enumerate joysticks");
            }

            return count;
        }

        private static unsafe void SetJoystickProperties(IDirectInputDevice device)
        {
            int Callback(DIDEVICEOBJECTINSTANCE* p, void* v)
            {
                //--Just seems to be a GUID* to address "4".
                //_GUID* DIPROP_RANGE = Library.Joystick.GetRangeGuid();
                long guidPropertyRange = 4L;

                var propertyHeader = new DIPROPHEADER
                {
                    dwSize = (uint)sizeof(DIPROPRANGE),
                    dwHeaderSize = (uint)sizeof(DIPROPHEADER),
                    dwHow = Define.DIPH_BYID,
                    dwObj = p->dwType
                };

                var d = new DIPROPRANGE
                {
                    lMin = 0,
                    lMax = 0xFFFF,
                    diph = propertyHeader //--TODO: Somehow the address of diph is not the same address as property header.
                };

                long hr = device.SetProperty(guidPropertyRange, &d.diph);

                //--This will iterate a few times per joystick.
                return hr < 0 ? Define.DIENUM_STOP : Define.DIENUM_CONTINUE;
            }

            //--Manually recreate: c_dfDIJoystick2
            DIDATAFORMAT df = JoystickDataFormat.GetDataFormat();

            long hr = device.SetDataFormat(&df);

            if (hr < 0)
            {
                throw new Exception("Failed to set data format on joystick");
            }

            hr = device.EnumObjects(Callback, IntPtr.Zero, Define.DIDFT_AXIS);

            if (hr < 0)
            {
                throw new Exception("Failed to set properties on joystick");
            }
        }

        public JoystickState JoystickPoll(int id)
        {
            DIJOYSTATE2 state = new DIJOYSTATE2();

            long hr = JoystickPoll(state, _devices[id]);

            if (hr != Define.S_OK)
            {
                Debug.WriteLine($"Bad joystick poll: {hr}");
            }

            unsafe
            {
                return new JoystickState
                {
                    X = state.lX >> 10,
                    Y = state.lY >> 10,
                    Button1 = state.rgbButtons[0] >> 7,
                    Button2 = state.rgbButtons[1] >> 7,

                };
            }
        }

        private static unsafe long JoystickPoll(DIJOYSTATE2 state, IDirectInputDevice stick)
        {
            if (stick == null)
            {
                return Define.S_OK;
            }

            long hr = stick.Poll();

            if (hr < 0)
            {
                hr = stick.Acquire();

                while (hr == Define.DIERR_INPUTLOST)
                {
                    hr = stick.Acquire();
                }

                switch (hr)
                {
                    case Define.DIERR_INVALIDPARAM:
                        return Define.E_FAIL;

                    case Define.DIERR_OTHERAPPHASPRIO:
                        return Define.S_OK;
                }
            }

            hr = stick.GetDeviceState((uint)sizeof(DIJOYSTATE2), &state);

            return hr < 0 ? hr : Define.S_OK;
        }
    }
}
