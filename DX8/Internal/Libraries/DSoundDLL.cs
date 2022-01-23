// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using DX8.Internal.Models;
using System.Runtime.InteropServices;
using LPDIRECTSOUND = System.IntPtr;
using LPDSENUMCALLBACK = System.IntPtr;
using LPUNKNOWN = System.IntPtr;
using LPVOID = System.IntPtr;

namespace DX8.Internal.Libraries
{
    internal static class DSoundDLL
    {
        private const string Dll = "dsound.dll";

        /// <summary>
        /// This function creates and initializes an IDirectSound interface.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708921(v=vs.85)"/>
        /// <param name="lpGuid">Address of the GUID that identifies the sound device. The value of this parameter must be one of the GUIDs returned by DirectSoundEnumerate, or NULL for the default device.</param>
        /// <param name="ppDS">Address of a pointer to a DirectSound object created in response to this function.</param>
        /// <param name="pUnkOuter">Controlling unknown of the aggregate. Its value must be NULL.</param>
        /// <returns>
        /// If the function succeeds, the return value is DS_OK.
        /// If the function fails, the return value may be one of the following error values:
        /// DSERR_ALLOCATED | DSERR_INVALIDPARAM | DSERR_NOAGGREGATION | DSERR_NODRIVER |DSERR_OUTOFMEMORY
        /// </returns>
        /// <remarks>The application must call the IDirectSound::SetCooperativeLevel method immediately after creating a DirectSound object.</remarks>
        [DllImport(Dll)]
        public static extern long DirectSoundCreate(/*LPGUID*/ ref _GUID lpGuid, ref LPDIRECTSOUND ppDS, LPUNKNOWN pUnkOuter);

        /// <summary>
        /// The DirectSoundEnumerate function enumerates the DirectSound drivers installed in the system.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416763(v=vs.85)"/>
        /// <param name="lpDSEnumCallback">Address of the DSEnumCallback function that will be called for each device installed in the system.</param>
        /// <param name="lpContext">Address of the user-defined context passed to the enumeration callback function every time that function is called.</param>
        /// <returns>If the function succeeds, it returns DS_OK. If it fails, the return value may be DSERR_INVALIDPARAM.</returns>
        [DllImport(Dll)]
        public static extern long DirectSoundEnumerate(LPDSENUMCALLBACK lpDSEnumCallback, LPVOID lpContext);
    }
}
