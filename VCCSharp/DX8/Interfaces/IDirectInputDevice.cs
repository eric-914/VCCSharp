using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectInputDevice)]
    public interface IDirectInputDevice
    {
        long GetCapabilities(/*LPDIDEVCAPS*/ System.IntPtr lpDIDevCaps);
        long EnumObjects([MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDeviceObjectsCallback lpCallback, System.IntPtr pvRef, uint dwFlags);
        long GetProperty(ref _GUID rguidProp, /*LPDIPROPHEADER*/ System.IntPtr pdiph);
        unsafe long SetProperty(long rguidProp, DIPROPHEADER* pdiph);
        long Acquire();
        long Unacquire();
        unsafe long GetDeviceState(uint cbData, void* lpvData);
        unsafe long GetDeviceData(uint cbObjectData, /*LPDIDEVICEOBJECTDATA*/ System.IntPtr rgdod, uint* pdwInOut, uint dwFlags);
        unsafe long SetDataFormat(DIDATAFORMAT* lpdf);
        long SetEventNotification(HANDLE hEvent);
        long SetCooperativeLevel(HWND hwnd, uint dwFlags);
        long GetObjectInfo(/*LPDIDEVICEOBJECTINSTANCEA*/ System.IntPtr pdidoi, uint dwObj, uint dwHow);
        long GetDeviceInfo(/*LPDIDEVICEINSTANCEA*/ System.IntPtr pdidi);
        long RunControlPanel(HWND hwndOwner, uint dwFlags);
        long Initialize(HINSTANCE hinst, uint dwVersion, ref _GUID rguid);
        long CreateEffect(ref _GUID rguid, /*LPCDIEFFECT*/ System.IntPtr lpeff, /*LPDIRECTINPUTEFFECT* */ System.IntPtr ppdeff, /*LPUNKNOWN*/ System.IntPtr punkOuter);
        unsafe long EnumEffects(/*LPDIENUMEFFECTSCALLBACKA*/ System.IntPtr lpCallback, void* pvRef, uint dwEffType);
        long GetEffectInfo(/*LPDIEFFECTINFOA*/ System.IntPtr pdei, ref _GUID rguid);
        unsafe long GetForceFeedbackState(uint* pdwOut);
        long SendForceFeedbackCommand(uint dwFlags);
        unsafe long EnumCreatedEffectObjects(/*LPDIENUMCREATEDEFFECTOBJECTSCALLBACK*/ System.IntPtr lpCallback, void* pvRef, uint fl);
        long Escape(/*LPDIEFFESCAPE*/ System.IntPtr pesc);
        long Poll();
        //unsafe long SendDeviceData(uint cbObjectData, LPCDIDEVICEOBJECTDATA rgdod, uint* pdwInOut, uint fl);
        //unsafe long EnumEffectsInFile(string lpszFileName, LPDIENUMEFFECTSINFILECALLBACK pec, void* pvRef, uint dwFlags);
        //unsafe long WriteEffectToFile(string lpszFileName, uint dwEntries, LPDIFILEEFFECT rgDiFileEft, uint dwFlags);
        //unsafe long BuildActionMap(LPDIACTIONFORMATA lpdiaf, string lpszUserName, uint dwFlags);
        //unsafe long SetActionMap(LPDIACTIONFORMATA lpdiActionFormat, string lptszUserName, uint dwFlags);
        //unsafe long GetImageInfo(LPDIDEVICEIMAGEINFOHEADERA lpdiDevImageInfoHeader);
    }
}
