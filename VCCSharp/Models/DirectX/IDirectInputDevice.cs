using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models.DirectX
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectInputDevice)]
    public interface IDirectInputDevice
    {
        /*** IUnknown methods ***/
        //STDMETHOD(QueryInterface(REFIID riid, void* * ppvObj);
        //STDMETHOD_(ULONG,AddRef)(THIS);
        //STDMETHOD_(ULONG,Release)(THIS);

        /*** IDirectInputDevice8A methods ***/
        unsafe long GetCapabilities(/*LPDIDEVCAPS*/ System.IntPtr lpDIDevCaps);
        /**/ unsafe long EnumObjects(DIEnumDeviceObjectsCallback lpCallback, void* pvRef, uint dwFlags);
        unsafe long GetProperty(ref _GUID rguidProp, /*LPDIPROPHEADER*/ System.IntPtr pdiph);
        /**/ unsafe long SetProperty(_GUID* rguidProp, DIPROPHEADER* pdiph);
        unsafe long Acquire();
        unsafe long Unacquire();
        unsafe long GetDeviceState(uint cbData, void* lpvData);
        unsafe long GetDeviceData(uint cbObjectData, /*LPDIDEVICEOBJECTDATA*/ System.IntPtr rgdod, uint* pdwInOut, uint dwFlags);
        /**/ unsafe long SetDataFormat(DIDATAFORMAT* lpdf);
        unsafe long SetEventNotification(HANDLE hEvent);
        unsafe long SetCooperativeLevel(HWND hwnd, uint dwFlags);
        //unsafe long GetObjectInfo(LPDIDEVICEOBJECTINSTANCEA pdidoi, uint dwObj, uint dwHow);
        //unsafe long GetDeviceInfo(LPDIDEVICEINSTANCEA pdidi);
        //unsafe long RunControlPanel(HWND hwndOwner, uint dwFlags);
        //unsafe long Initialize(HINSTANCE hinst, uint dwVersion, ref _GUID rguid);
        //unsafe long CreateEffect(ref _GUID rguid, LPCDIEFFECT lpeff, LPDIRECTINPUTEFFECT* ppdeff, LPUNKNOWN punkOuter);
        //unsafe long EnumEffects(LPDIENUMEFFECTSCALLBACKA lpCallback, void* pvRef, uint dwEffType);
        //unsafe long GetEffectInfo(LPDIEFFECTINFOA pdei, ref _GUID rguid);
        //unsafe long GetForceFeedbackState(uint* pdwOut);
        //unsafe long SendForceFeedbackCommand(uint dwFlags);
        //unsafe long EnumCreatedEffectObjects(LPDIENUMCREATEDEFFECTOBJECTSCALLBACK lpCallback, void* pvRef, uint fl);
        //unsafe long Escape(LPDIEFFESCAPE pesc);
        //unsafe long Poll();
        //unsafe long SendDeviceData(uint cbObjectData, LPCDIDEVICEOBJECTDATA rgdod, uint* pdwInOut, uint fl);
        //unsafe long EnumEffectsInFile(string lpszFileName, LPDIENUMEFFECTSINFILECALLBACK pec, void* pvRef, uint dwFlags);
        //unsafe long WriteEffectToFile(string lpszFileName, uint dwEntries, LPDIFILEEFFECT rgDiFileEft, uint dwFlags);
        //unsafe long BuildActionMap(LPDIACTIONFORMATA lpdiaf, string lpszUserName, uint dwFlags);
        //unsafe long SetActionMap(LPDIACTIONFORMATA lpdiActionFormat, string lptszUserName, uint dwFlags);
        //unsafe long GetImageInfo(LPDIDEVICEIMAGEINFOHEADERA lpdiDevImageInfoHeader);
    }
}
