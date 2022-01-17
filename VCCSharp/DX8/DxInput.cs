using System;
using VCCSharp.DX8.Interfaces;
using VCCSharp.DX8.Libraries;
using VCCSharp.DX8.Models;
using VCCSharp.Models;

namespace VCCSharp.DX8
{
    public delegate void EnumerateDevicesCallback(IDirectInputDevice joystick, string name);

    public interface IDxInput
    {
        void CreateDirectInput(IntPtr handle);

        void EnumerateDevices(EnumerateDevicesCallback callback);

        void SetJoystickProperties(IDirectInputDevice device);

        JoystickState JoystickPoll(IDirectInputDevice stick);
    }

    public class DxInput : IDxInput
    {
        private readonly IDInput _input;
        private readonly IDxFactory _factory;

        private IDirectInput _di;

        public DxInput(IDInput input, IDxFactory factory)
        {
            _input = input;
            _factory = factory;
        }

        public void CreateDirectInput(IntPtr handle)
        {
            const uint version = 0x0800;

            _GUID guid = CreateIDirectInput8AGuid();

            _di = _factory.CreateDirectInput(_input, handle, version, guid);
        }

        private static unsafe _GUID CreateIDirectInput8AGuid()
        {
            //return Converter.ToGuid("BF798030-483A-4DA2AA99-5D64ED369700");
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

        public IDirectInputDevice CreateDevice(_GUID guidInstance)
        {
            IDirectInputDevice joystick = null;

            long hr = _di.CreateDevice(guidInstance, ref joystick, IntPtr.Zero);

            if (hr < 0)
            {
                throw new Exception("Failed to create device");
            }

            return joystick;
        }

        public void EnumerateDevices(DIEnumDevicesCallback callback)
        {
            long hr = _di.EnumDevices(Define.DI8DEVCLASS_GAMECTRL, callback, IntPtr.Zero, Define.DIEDFL_ATTACHEDONLY);

            if (hr < 0)
            {
                throw new Exception("Failed to enumerate joysticks");
            }
        }

        public unsafe void EnumerateDevices(EnumerateDevicesCallback callback)
        {
            int count = 0;

            int EnumerateCallback(DIDEVICEINSTANCE* p, void* v)
            {
                IDirectInputDevice joystick = CreateDevice(p->guidInstance);
                string name = Converter.ToString(p->tszInstanceName);

                callback(joystick, name);

                return ++count < Define.MAX_JOYSTICKS ? Define.TRUE : Define.FALSE;
            }

            long hr = _di.EnumDevices(Define.DI8DEVCLASS_GAMECTRL, EnumerateCallback, IntPtr.Zero, Define.DIEDFL_ATTACHEDONLY);

            if (hr < 0)
            {
                throw new Exception("Failed to enumerate joysticks");
            }
        }

        public unsafe void SetJoystickProperties(IDirectInputDevice device)
        {
            int SetJoystickPropertiesCallback(DIDEVICEOBJECTINSTANCE* p, void* v)
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

            hr = device.EnumObjects(SetJoystickPropertiesCallback, IntPtr.Zero, Define.DIDFT_AXIS);

            if (hr < 0)
            {
                throw new Exception("Failed to set properties on joystick");
            }
        }

        public JoystickState JoystickPoll(IDirectInputDevice stick)
        {
            DIJOYSTATE2 state = new DIJOYSTATE2();

            JoystickPoll(state, stick);

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

        private unsafe long JoystickPoll(DIJOYSTATE2 state, IDirectInputDevice stick)
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
