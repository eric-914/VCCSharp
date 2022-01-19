// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
using DX8.Models;
using System.Runtime.InteropServices;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;
using IntPtr = System.IntPtr;
using LPVOID = System.IntPtr;

namespace DX8.Interfaces
{
    public delegate int DIEnumDeviceObjectsCallback(ref DIDEVICEOBJECTINSTANCE lpddoi, IntPtr pvRef);

    /// <summary>
    /// Applications use the methods of the IDirectInputDevice interface to gain and release access to Microsoft DirectInput devices, manage device properties and information, set behavior, perform initialization, create and play force-feedback effects, and invoke a device's control panel.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectInputDevice)]
    public interface IDirectInputDevice
    {
        #region unused
        long GetCapabilities(/*LPDIDEVCAPS*/ IntPtr lpDIDevCaps);
        #endregion

        /// <summary>
        /// Enumerates the input and output objects available on a device.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417889(v=vs.85)"/>
        /// <param name="lpCallback">Address of a callback function that receives DirectInputDevice objects. DirectInput provides a prototype of this function as DIEnumDeviceObjectsCallback.</param>
        /// <param name="pvRef">Reference data (context) for callback.</param>
        /// <param name="dwFlags">Flags that specify the types of object to be enumerated.</param>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_INVALIDPARAM | DIERR_NOTINITIALIZED
        /// </returns>
        /// <remarks>The DIDFT_FFACTUATOR and DIDFT_FFEFFECTTRIGGER flags in the dwFlags parameter restrict enumeration to objects that meet all the criteria defined by the included flags. For all the other flags, an object is enumerated if it meets the criterion defined by any included flag in this category.</remarks>
        long EnumObjects([MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDeviceObjectsCallback lpCallback, LPVOID pvRef, /*DWORD*/ uint dwFlags);
        
        #region unused
        long GetProperty(ref _GUID rguidProp, /*LPDIPROPHEADER*/ IntPtr pdiph);
        #endregion

        /// <summary>
        /// Sets properties that define the device behavior. These properties include input buffer size and axis mode.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417929(v=vs.85)"/>
        /// <param name="rguidProp">Reference to (C++) or address of (C) the globally unique identifier (GUID) identifying the property to be set. This can be one of the predefined values, or a pointer to a GUID that identifies the property.</param>
        /// <param name="pdiph">Address of the DIPROPHEADER structure contained within the type-specific property structure.</param>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK or DI_PROPNOEFFECT.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_INVALIDPARAM | DIERR_NOTINITIALIZED | DIERR_OBJECTNOTFOUND | DIERR_UNSUPPORTED.
        /// </returns>
        /// <remarks>The buffer size determines the amount of data that the buffer can hold between calls to the GetDeviceData method before data is lost. This value may be set to 0 to indicate that the application does not read buffered data from the device. If the buffer size in the dwData member of the DIPROPDWORD structure is too large for the device to support it, then the largest possible buffer size is set.</remarks>
        long SetProperty(/*REFGUID*/ long rguidProp, ref DIPROPHEADER pdiph);

        /// <summary>
        /// Obtains access to the input device.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417818(v=vs.85)"/>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK, or S_FALSE if the device was already acquired.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_INVALIDPARAM | DIERR_NOTINITIALIZED | DIERR_OTHERAPPHASPRIO.
        /// </returns>
        /// <remarks>Before a device can be acquired, a data format must be set by using the SetDataFormat method or SetActionMap method. If the data format has not been set, IDirectInputDevice8 Interface returns DIERR_INVALIDPARAM.</remarks>
        long Acquire();
        
        #region unused
        long Unacquire();
        #endregion

        /// <summary>
        /// Retrieves immediate data from the device.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417897(v=vs.85)"/>
        /// <param name="cbData">Size of the buffer in the lpvData parameter, in bytes.</param>
        /// <param name="lpvData">Address of a structure that receives the current state of the device. The format of the data is established by a prior call to the SetDataFormat method.</param>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_INPUTLOST | DIERR_INVALIDPARAM | DIERR_NOTACQUIRED | DIERR_NOTINITIALIZED | E_PENDING.
        /// </returns>
        /// <remarks>Before device data can be obtained, set the cooperative level by using the SetCooperativeLevel method, then set the data format by using SetDataFormat, and acquire the device by using the IDirectInputDevice8 Interface method.</remarks>
        long GetDeviceState(/*DWORD*/ uint cbData, /*LPVOID*/ ref DIJOYSTATE2 lpvData);
        //long GetDeviceState(/*DWORD*/ uint cbData, /*LPVOID*/ ref LPVOID lpvData); //--TODO: Hold this until change is confirmed.
        
        #region unused
        long GetDeviceData(uint cbObjectData, /*LPDIDEVICEOBJECTDATA*/ IntPtr rgdod, ref uint pdwInOut, uint dwFlags);
        #endregion

        /// <summary>
        /// Sets the data format for the DirectInput device.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417925(v=vs.85)"/>
        /// <param name="lpdf">Address of a structure that describes the format of the data that the DirectInputDevice should return. </param>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_ACQUIRED | DIERR_INVALIDPARAM | DIERR_NOTINITIALIZED.
        /// </returns>
        long SetDataFormat(ref DIDATAFORMAT lpdf);
        
        #region unused
        long SetEventNotification(HANDLE hEvent);
        long SetCooperativeLevel(HWND hwnd, uint dwFlags);
        long GetObjectInfo(/*LPDIDEVICEOBJECTINSTANCEA*/ IntPtr pdidoi, uint dwObj, uint dwHow);
        long GetDeviceInfo(/*LPDIDEVICEINSTANCEA*/ IntPtr pdidi);
        long RunControlPanel(HWND hwndOwner, uint dwFlags);
        long Initialize(HINSTANCE hinst, uint dwVersion, ref _GUID rguid);
        long CreateEffect(ref _GUID rguid, /*LPCDIEFFECT*/ IntPtr lpeff, /*LPDIRECTINPUTEFFECT* */ IntPtr ppdeff, /*LPUNKNOWN*/ IntPtr punkOuter);
        long EnumEffects(/*LPDIENUMEFFECTSCALLBACKA*/ IntPtr lpCallback, ref HANDLE pvRef, uint dwEffType);
        long GetEffectInfo(/*LPDIEFFECTINFOA*/ IntPtr pdei, ref _GUID rguid);
        long GetForceFeedbackState(IntPtr pdwOut);
        long SendForceFeedbackCommand(uint dwFlags);
        long EnumCreatedEffectObjects(/*LPDIENUMCREATEDEFFECTOBJECTSCALLBACK*/ IntPtr lpCallback, ref IntPtr pvRef, uint fl);
        long Escape(/*LPDIEFFESCAPE*/ IntPtr pesc);
        #endregion

        /// <summary>
        /// Retrieves data from polled objects on a DirectInput device. If the device does not require polling, calling this method has no effect. If a device that requires polling is not polled periodically, no new data is received from the device. Calling this method causes DirectInput to update the device state, generate input events (if buffered data is enabled), and set notification events (if notification is enabled).
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417913(v=vs.85)"/>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK, or DI_NOEFFECT if the device does not require polling.
        /// If the method fails, the return value can be one of the following error values:
        /// DIERR_INPUTLOST | DIERR_NOTACQUIRED | DIERR_NOTINITIALIZED.
        /// </returns>
        /// <remarks>Before a device data can be polled, the data format must be set by using the SetDataFormat or SetActionMap method, and the device must be acquired by using the IDirectInputDevice8 Interface method.</remarks>
        long Poll();

        #region unused
        //long SendDeviceData(uint cbObjectData, LPCDIDEVICEOBJECTDATA rgdod, ref uint pdwInOut, uint fl);
        //long EnumEffectsInFile(string lpszFileName, LPDIENUMEFFECTSINFILECALLBACK pec, IntPtr pvRef, uint dwFlags);
        //long WriteEffectToFile(string lpszFileName, uint dwEntries, LPDIFILEEFFECT rgDiFileEft, uint dwFlags);
        //long BuildActionMap(LPDIACTIONFORMATA lpdiaf, string lpszUserName, uint dwFlags);
        //long SetActionMap(LPDIACTIONFORMATA lpdiActionFormat, string lptszUserName, uint dwFlags);
        //long GetImageInfo(LPDIDEVICEIMAGEINFOHEADERA lpdiDevImageInfoHeader);
        #endregion
    }
}
