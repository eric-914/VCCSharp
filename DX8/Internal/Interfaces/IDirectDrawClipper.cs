// ReSharper disable CommentTypo

using System.Runtime.InteropServices;
using HWND = System.IntPtr;

namespace DX8.Internal.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectDrawClipper)]
    internal interface IDirectDrawClipper
    {
        #region unused
        long GetClipList();
        long GetHWnd();
        long Initialize();
        long IsClipListChanged();
        long SetClipList();
        #endregion

        /// <summary>
        /// Sets the window handle that the clipper object uses to obtain clipping information.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawclipper-sethwnd"/>
        /// <param name="zero">Currently not used and must be set to 0.</param>
        /// <param name="hWnd">Window handle that obtains the clipping information.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INVALIDCLIPLIST | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS |DDERR_OUTOFMEMORY
        /// </returns>
        long SetHWnd(/*DWORD*/ uint zero, HWND hWnd);
    }
}
