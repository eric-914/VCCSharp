// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using DX8.Internal.Models;
using System.Runtime.InteropServices;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;
using IntPtr = System.IntPtr;
using LPUNKNOWN = System.IntPtr;
using LPVOID = System.IntPtr;

namespace DX8.Internal.Interfaces
{
    internal delegate int DIEnumDevicesCallback(ref DIDEVICEINSTANCE lpddi, IntPtr pvRef);

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectInput)]
    internal interface IDirectInput
    {
        /// <summary>
        /// Creates and initializes an instance of a device based on a given globally unique identifier (GUID), and obtains an IDirectInputDevice8 Interface interface.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417803(v=vs.85)"/>
        /// <param name="refGuid">Reference to the GUID for the desired input device (see Remarks). The GUID is retrieved through the IDirectInput8::EnumDevices method, or it can be one of the predefined GUIDs listed below. For the following GUID values to be valid, your application must define INITGUID before all other preprocessor directives at the beginning of the source file, or link to Dxguid.lib.</param>
        /// <param name="lpDirectInputDevice">Address of a variable to receive the IDirectInputDevice8 Interface interface pointer if successful.</param>
        /// <param name="lpUnknown">Address of the controlling object's IUnknown interface for COM aggregation, or NULL if the interface is not aggregated. Most calling applications pass NULL.</param>
        /// <returns>
        /// If the method succeeds, the return value is DI_OK.
        /// If the method fails, the return value can be one of the following:
        /// DIERR_DEVICENOTREG | DIERR_INVALIDPARAM | DIERR_NOINTERFACE | DIERR_NOTINITIALIZED | DIERR_OUTOFMEMORY.
        /// </returns>
        /// <remarks>Calling this method with pUnkOuter = NULL is equivalent to creating the object by CoCreateInstance (CLSID_DirectInputDevice, NULL, CLSCTX_INPROC_SERVER, riid, lplpDirectInputDevice) and then initializing it with Initialize.</remarks>
        long CreateDevice(_GUID refGuid, ref IDirectInputDevice? lpDirectInputDevice, LPUNKNOWN lpUnknown);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee417804(v=vs.85)"/>
        /// <param name="dwDevType">Device type filter.</param>
        /// <param name="callback">Address of a callback function to be called once for each device enumerated. See DIEnumDevicesCallback.</param>
        /// <param name="pvRef">Application-defined 32-bit value to be passed to the enumeration callback each time it is called.</param>
        /// <param name="dwFlags">Flag value that specifies the scope of the enumeration.</param>
        /// <returns></returns>
        long EnumDevices(/*DWORD*/ uint dwDevType, [MarshalAs(UnmanagedType.FunctionPtr)] DIEnumDevicesCallback callback, LPVOID pvRef, /*DWORD*/ uint dwFlags);

        #region unused
        long GetDeviceStatus(ref _GUID rguidInstance);
        long RunControlPanel(HWND hwndOwner, uint dwFlags);
        long Initialize(HINSTANCE hinst, uint dwVersion);
        long FindDevice(ref _GUID rguidClass, IntPtr ptszName, IntPtr pguidInstance);
        long EnumDevicesBySemantics(IntPtr ptszUserName, IntPtr lpdiActionFormat, IntPtr lpCallback, IntPtr pvRef, uint dwFlags);
        long ConfigureDevices(IntPtr lpdiCallback, IntPtr lpdiCDParams, uint dwFlags, IntPtr pvRefData);
        #endregion
    }
}
