using System;
using VCCSharp.DX8.Interfaces;
using VCCSharp.DX8.Libraries;
using VCCSharp.DX8.Models;
using VCCSharp.Models;

namespace VCCSharp.DX8
{
    public interface IDxInput
    {
        void CreateDirectInput(IntPtr handle);

        IDirectInputDevice CreateDevice(_GUID guidInstance);
        void EnumerateDevices(DIEnumDevicesCallback callback);
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
    }
}
