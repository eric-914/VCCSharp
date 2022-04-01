using DX8.Internal;
using DX8.Internal.Converters;
using DX8.Internal.Formats;
using DX8.Internal.Interfaces;
using DX8.Internal.Libraries;
using DX8.Internal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DX8.Models;

internal class DxInput : IDxInput
{
    private readonly IDInput _input;
    private readonly IDxFactoryInternal _factory;

    private IDirectInput _di;
    private readonly List<DxDevice> _devices = new();

    internal DxInput(IDInput input, IDxFactoryInternal factory)
    {
        _input = input;
        _factory = factory;
    }

    public DxInput() : this(new DInput(), new DxFactoryInternal()) { }

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

    public List<string> JoystickList()
    {
        return _devices.Select(x => x.InstanceName).ToList();
    }

    public void EnumerateDevices()
    {
        _devices.Clear();

        int Callback(ref DIDEVICEINSTANCE p, IntPtr v)
        {
            IDirectInputDevice joystick = CreateDevice(p.guidInstance);
            string instanceName = StringConverter.ToString(p.tszInstanceName);

            var device = new DxDevice(_devices.Count, instanceName, joystick);

            _devices.Add(device);

            SetJoystickProperties(device.Device);

            return _devices.Count < DxDefine.MAX_JOYSTICKS ? DxDefine.TRUE : DxDefine.FALSE;
        }

        long hr = _di.EnumDevices(DxDefine.DI8DEVCLASS_GAMECTRL, Callback, IntPtr.Zero, DxDefine.DIEDFL_ATTACHEDONLY);

        if (hr < 0)
        {
            throw new Exception("Failed to enumerate joysticks");
        }
    }

    private static void SetJoystickProperties(IDirectInputDevice device)
    {
        int Callback(ref DIDEVICEOBJECTINSTANCE p, IntPtr v)
        {
            //--Just seems to be a GUID* to address "4".
            //_GUID* DIPROP_RANGE = Library.Joystick.GetRangeGuid();
            //TODO: Define.DI8DEVCLASS_GAMECTRL ?
            long guidPropertyRange = 4L;

            var propertyHeader = new DIPROPHEADER
            {
                dwSize = (uint)DIPROPRANGE.Size,
                dwHeaderSize = (uint)DIPROPHEADER.Size,
                dwHow = DxDefine.DIPH_BYID,
                dwObj = p.dwType
            };

            var d = new DIPROPRANGE
            {
                lMin = 0,
                lMax = 0xFFFF,
                diph = propertyHeader //--TODO: Somehow the address of diph is not the same address as property header.
            };

            long hr = device.SetProperty(guidPropertyRange, ref d.diph);

            //--This will iterate a few times per joystick.
            return hr < 0 ? DxDefine.DIENUM_STOP : DxDefine.DIENUM_CONTINUE;
        }

        //--Manually recreate: c_dfDIJoystick2
        DIDATAFORMAT df = JoystickDataFormat.GetDataFormat();

        long hr = device.SetDataFormat(ref df);

        if (hr < 0)
        {
            throw new Exception("Failed to set data format on joystick");
        }

        device.Acquire();

        hr = device.EnumObjects(Callback, IntPtr.Zero, DxDefine.DIDFT_AXIS);

        if (hr < 0)
        {
            throw new Exception("Failed to set properties on joystick");
        }
    }

    public IDxJoystickState JoystickPoll(int index)
    {
        DIJOYSTATE2 state = DIJOYSTATE2.Create();

        var device = _devices.FirstOrDefault(x => x.Index == index);

        if (device == null) return new NullDxJoystickState();

        long hr = JoystickPoll(ref state, device.Device);

        if (hr != DxDefine.S_OK)
        {
            Debug.WriteLine($"Bad joystick poll: {hr}");
        }

        var xbox = new DxXboxControllerState(state);
        //Debug.WriteLine(xbox);

        return xbox;
    }

    private static long JoystickPoll(ref DIJOYSTATE2 state, IDirectInputDevice device)
    {
        if (device == null)
        {
            return DxDefine.S_OK;
        }

        long hr;

        hr = device.Poll();
        //try
        //{
        //    hr = device.Poll();
        //}
        //catch (InvalidCastException)
        //{
        //    //--TODO: Track down why this happens on first poll after list refresh.
        //    Debug.WriteLine("InvalidCastException on device.Poll()");
        //    return -1;
        //}

        if (hr < 0)
        {
            hr = device.Acquire();

            while (hr == DxDefine.DIERR_INPUTLOST)
            {
                hr = device.Acquire();
            }

            switch (hr)
            {
                case DxDefine.DIERR_INVALIDPARAM:
                    return DxDefine.E_FAIL;

                case DxDefine.DIERR_OTHERAPPHASPRIO:
                    return DxDefine.S_OK;
            }
        }

        hr = device.GetDeviceState((uint)DIJOYSTATE2.Size, ref state);

        //try
        //{
        //    hr = device.GetDeviceState((uint)DIJOYSTATE2.Size, ref state);
        //}
        //catch (InvalidCastException)
        //{
        //    //--TODO: Track down why this happens on first poll after list refresh.
        //    Debug.WriteLine("InvalidCastException on device.GetDeviceState()");
        //}

        return hr < 0 ? hr : DxDefine.S_OK;
    }
}
