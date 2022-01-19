// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
using DX8.Models;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using LPDIRECTSOUNDBUFFER = System.IntPtr;
using LPUNKNOWN = System.IntPtr;

namespace DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectSound)]
    public interface IDirectSound
    {
        /// <summary>
        /// The CreateSoundBuffer method creates a sound buffer object to manage audio samples.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee418039(v=vs.85)"/>
        /// <param name="pBufferDescription">Address of a DSBUFFERDESC structure that describes the sound buffer to create.</param>
        /// <param name="pInstance">Address of a variable that receives the IDirectSoundBuffer interface of the new buffer object. Use QueryInterface to obtain IDirectSoundBuffer. IDirectSoundBuffer is not available for the primary buffer.</param>
        /// <param name="pUnknown">Address of the controlling object's IUnknown interface for COM aggregation. Must be NULL.</param>
        /// <returns>If the method succeeds, the return value is DS_OK</returns>
        /// <remarks>DirectSound does not initialize the contents of the buffer, and the application cannot assume that it contains silence.</remarks>
        unsafe long CreateSoundBuffer(DSBUFFERDESC* pBufferDescription, ref LPDIRECTSOUNDBUFFER pInstance, LPUNKNOWN pUnknown);

        #region unused
        long GetCaps();
        long DuplicateSoundBuffer();
        #endregion

        /// <summary>
        /// The SetCooperativeLevel method sets the cooperative level of the application for this sound device.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee418049(v=vs.85)"/>
        /// <param name="hwnd">Handle to the application window.</param>
        /// <param name="dwLevel">Requested level.</param>
        /// <returns>If the method succeeds, the return value is DS_OK.</returns>
        /// <remarks>The application must set the cooperative level by calling this method before its buffers can be played. The recommended cooperative level is DSSCL_PRIORITY. Do not call this method if any buffers are locked.</remarks>
        long SetCooperativeLevel(HWND hwnd, /*DWORD*/ uint dwLevel);

        #region unused
        long Compact();
        long GetSpeakerConfig();
        long SetSpeakerConfig();
        long Initialize();
        #endregion
    }
}
