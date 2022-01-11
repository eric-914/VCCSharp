using System;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models.DirectX
{
    public unsafe delegate int DIEnumDevicesCallback(DIDEVICEINSTANCE* lpddi, void* pvRef);

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectInput)]
    public interface IDirectInput
    {
        /*** IUnknown methods ***/
        //STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID * ppvObj) PURE;
        //STDMETHOD_(ULONG,AddRef)(THIS) PURE;
        //STDMETHOD_(ULONG,Release)(THIS) PURE;

        /*** IDirectInput8A methods ***/
        //STDMETHOD(CreateDevice)(THIS_ REFGUID,LPDIRECTINPUTDEVICE8A *,LPUNKNOWN) PURE;
        unsafe long CreateDevice(_GUID refGuid, ref IDirectInputDevice lpDirectInputDevice, IntPtr lpUnknown);

        //STDMETHOD(EnumDevices)(THIS_ DWORD,LPDIENUMDEVICESCALLBACKA,LPVOID,DWORD) PURE;
        unsafe long EnumDevices(uint dwDevType, [MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDevicesCallback callback, IntPtr pvRef, uint dwFlags);

        //STDMETHOD(GetDeviceStatus)(THIS_ REFGUID) PURE;
        unsafe long GetDeviceStatus(_GUID* rguidInstance);

        //STDMETHOD(RunControlPanel)(THIS_ HWND,DWORD) PURE;
        unsafe long RunControlPanel(HWND hwndOwner, uint dwFlags);

        //STDMETHOD(Initialize)(THIS_ HINSTANCE,DWORD) PURE;
        unsafe long Initialize(HINSTANCE hinst, uint dwVersion);

        //STDMETHOD(FindDevice)(THIS_ REFGUID,LPCSTR,LPGUID) PURE;
        unsafe long FindDevice(_GUID* rguidClass, IntPtr ptszName, IntPtr pguidInstance);

        //STDMETHOD(EnumDevicesBySemantics)(THIS_ LPCSTR,LPDIACTIONFORMATA,LPDIENUMDEVICESBYSEMANTICSCBA,LPVOID,DWORD) PURE;
        unsafe long EnumDevicesBySemantics(IntPtr ptszUserName, IntPtr lpdiActionFormat, IntPtr lpCallback, IntPtr pvRef, uint dwFlags);

        //STDMETHOD(ConfigureDevices)(THIS_ LPDICONFIGUREDEVICESCALLBACK,LPDICONFIGUREDEVICESPARAMSA,DWORD,LPVOID) PURE;
        unsafe long ConfigureDevices(IntPtr lpdiCallback, IntPtr lpdiCDParams, uint dwFlags, IntPtr pvRefData);
    }
}
