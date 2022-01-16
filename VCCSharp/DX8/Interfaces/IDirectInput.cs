using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.DX8.Interfaces
{
    public unsafe delegate int DIEnumDevicesCallback(DIDEVICEINSTANCE* lpddi, void* pvRef);
    public unsafe delegate int DIEnumDeviceObjectsCallback(DIDEVICEOBJECTINSTANCE *lpddoi, void* pvRef);

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectInput)]
    public interface IDirectInput
    {
        long CreateDevice(_GUID refGuid, ref IDirectInputDevice lpDirectInputDevice, IntPtr lpUnknown);
        long EnumDevices(uint dwDevType, [MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDevicesCallback callback, IntPtr pvRef, uint dwFlags);
        unsafe long GetDeviceStatus(_GUID* rguidInstance);
        long RunControlPanel(HWND hwndOwner, uint dwFlags);
        long Initialize(HINSTANCE hinst, uint dwVersion);
        unsafe long FindDevice(_GUID* rguidClass, IntPtr ptszName, IntPtr pguidInstance);
        long EnumDevicesBySemantics(IntPtr ptszUserName, IntPtr lpdiActionFormat, IntPtr lpCallback, IntPtr pvRef, uint dwFlags);
        long ConfigureDevices(IntPtr lpdiCallback, IntPtr lpdiCDParams, uint dwFlags, IntPtr pvRefData);
    }
}
