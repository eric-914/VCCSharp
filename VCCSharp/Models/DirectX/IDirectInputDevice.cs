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
        /**/
        unsafe long EnumObjects([MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDeviceObjectsCallback lpCallback, System.IntPtr pvRef, uint dwFlags);
        unsafe long GetProperty(ref _GUID rguidProp, /*LPDIPROPHEADER*/ System.IntPtr pdiph);
        /**/
        //unsafe long SetProperty(_GUID* rguidProp, DIPROPHEADER* pdiph);
        unsafe long SetProperty(long rguidProp, DIPROPHEADER* pdiph);
        unsafe long Acquire();
        unsafe long Unacquire();
        unsafe long GetDeviceState(uint cbData, void* lpvData);
        unsafe long GetDeviceData(uint cbObjectData, /*LPDIDEVICEOBJECTDATA*/ System.IntPtr rgdod, uint* pdwInOut, uint dwFlags);
        /**/
        unsafe long SetDataFormat(DIDATAFORMAT* lpdf);
        unsafe long SetEventNotification(HANDLE hEvent);
        unsafe long SetCooperativeLevel(HWND hwnd, uint dwFlags);
        unsafe long GetObjectInfo(/*LPDIDEVICEOBJECTINSTANCEA*/ System.IntPtr pdidoi, uint dwObj, uint dwHow);
        unsafe long GetDeviceInfo(/*LPDIDEVICEINSTANCEA*/ System.IntPtr pdidi);
        unsafe long RunControlPanel(HWND hwndOwner, uint dwFlags);
        unsafe long Initialize(HINSTANCE hinst, uint dwVersion, ref _GUID rguid);
        unsafe long CreateEffect(ref _GUID rguid, /*LPCDIEFFECT*/ System.IntPtr lpeff, /*LPDIRECTINPUTEFFECT* */ System.IntPtr ppdeff, /*LPUNKNOWN*/ System.IntPtr punkOuter);
        unsafe long EnumEffects(/*LPDIENUMEFFECTSCALLBACKA*/ System.IntPtr lpCallback, void* pvRef, uint dwEffType);
        unsafe long GetEffectInfo(/*LPDIEFFECTINFOA*/ System.IntPtr pdei, ref _GUID rguid);
        unsafe long GetForceFeedbackState(uint* pdwOut);
        unsafe long SendForceFeedbackCommand(uint dwFlags);
        unsafe long EnumCreatedEffectObjects(/*LPDIENUMCREATEDEFFECTOBJECTSCALLBACK*/ System.IntPtr lpCallback, void* pvRef, uint fl);
        unsafe long Escape(/*LPDIEFFESCAPE*/ System.IntPtr pesc);
        unsafe long Poll();
        //unsafe long SendDeviceData(uint cbObjectData, LPCDIDEVICEOBJECTDATA rgdod, uint* pdwInOut, uint fl);
        //unsafe long EnumEffectsInFile(string lpszFileName, LPDIENUMEFFECTSINFILECALLBACK pec, void* pvRef, uint dwFlags);
        //unsafe long WriteEffectToFile(string lpszFileName, uint dwEntries, LPDIFILEEFFECT rgDiFileEft, uint dwFlags);
        //unsafe long BuildActionMap(LPDIACTIONFORMATA lpdiaf, string lpszUserName, uint dwFlags);
        //unsafe long SetActionMap(LPDIACTIONFORMATA lpdiActionFormat, string lptszUserName, uint dwFlags);
        //unsafe long GetImageInfo(LPDIDEVICEIMAGEINFOHEADERA lpdiDevImageInfoHeader);
    }
}
