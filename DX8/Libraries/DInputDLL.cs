// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using DX8.Models;
using System.Runtime.InteropServices;
using HINSTANCE = System.IntPtr;
using LPUNKNOWN = System.IntPtr;
using LPVOID = System.IntPtr;

namespace DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DInputDLL
    {
        private const string Dll = "dinput8.dll";

        /// <summary>
        /// reates a DirectInput object and returns an IDirectInput8 Interface or later interface.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416756(v=vs.85)"/>
        /// <param name="hinst">Instance handle to the application or dynamic-link library (DLL) that is creating the DirectInput object. DirectInput uses this value to determine whether the application or DLL has been certified and to establish any special behaviors that might be necessary for backward compatibility. It is an error for a DLL to pass the handle to the parent application. For example, an Microsoft ActiveX control embedded in a Web page that uses DirectInput must pass its own instance handle, and not the handle to the browser. This ensures that DirectInput recognizes the control and can enable any special behaviors that might be necessary.</param>
        /// <param name="dwVersion">Version number of DirectInput for which the application is designed. This value is normally DIRECTINPUT_VERSION. If the application defines DIRECTINPUT_VERSION before including Dinput.h, the value must be greater than 0x0800. For earlier versions, use DirectInputCreateEx, which is in Dinput.lib.</param>
        /// <param name="riidltf">Unique identifier of the desired interface. This value is IID_IDirectInput8A or IID_IDirectInput8W. Passing the IID_IDirectInput8 define selects the ANSI or Unicode version of the interface, depending on whether UNICODE is defined during compilation.</param>
        /// <param name="ppvOut">Address of a pointer to a variable to receive the interface pointer if the call succeeds.</param>
        /// <param name="punkOuter">Pointer to the address of the controlling object's IUnknown interface for COM aggregation, or NULL if the interface is not aggregated. Most calling applications pass NULL. If aggregation is requested, the object returned in ppvOut is a pointer to IUnknown, as required by COM aggregation.</param>
        /// <returns>If the function succeeds, the return value is DI_OK. If the function fails, the return value can be one of the following error values: DIERR_BETADIRECTINPUTVERSION, DIERR_INVALIDPARAM, DIERR_OLDDIRECTINPUTVERSION, DIERR_OUTOFMEMORY.</returns>
        /// <remarks>The DirectInput object created by this function is implemented in Dinput8.dll. Versions of interfaces earlier than DirectX 8.0 cannot be obtained in this implementation.</remarks>
        [DllImport(Dll)]
        public static extern int DirectInput8Create(HINSTANCE hinst, /*DWORD*/ uint dwVersion, /*REFIID*/ _GUID riidltf, ref LPVOID ppvOut, LPUNKNOWN punkOuter);
    }
}
