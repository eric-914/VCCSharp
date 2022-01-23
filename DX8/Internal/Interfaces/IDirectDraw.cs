// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using DX8.Internal.Models;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using IUnknown = System.IntPtr;
using LPDIRECTDRAWCLIPPER = System.IntPtr;
using LPDIRECTDRAWSURFACE7 = System.IntPtr;

namespace DX8.Internal.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectDraw)]
    internal interface IDirectDraw
    {
        #region unused
        long Compact();
        #endregion

        /// <summary>
        /// Creates a DirectDrawClipper object.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdraw7-createclipper"/>
        /// <param name="flags">Currently not used and must be set to 0.</param>
        /// <param name="pInstance">Address of a variable to be set to a valid IDirectDrawClipper interface pointer if the call succeeds.</param>
        /// <param name="pUnknown">Allows for future compatibility with COM aggregation features. Currently this method returns an error if this parameter is not NULL.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_NOCOOPERATIVELEVELSET |DDERR_OUTOFMEMORY
        /// </returns>
        /// <remarks>The DirectDrawClipper object can be attached to a DirectDrawSurface and used during Blt, BltBatch, and UpdateOverlay operations.</remarks>
        long CreateClipper(/*DWORD*/ uint flags, ref LPDIRECTDRAWCLIPPER pInstance, IUnknown pUnknown);

        #region unused
        long CreatePalette();
        #endregion

        /// <summary>
        /// Creates a DirectDrawSurface object for this DirectDraw object.
        /// </summary>
        /// <see hcref="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdraw7-createsurface"/>
        /// <param name="pSurfaceDescription">Address of a DDSURFACEDESC2 structure that describes the requested surface. Set any unused members of the DDSURFACEDESC2 structure to 0 before calling this method. A DDSCAPS2 structure is a member of DDSURFACEDESC2.</param>
        /// <param name="pInstance">Address of a variable to be set to a valid IDirectDrawSurface7 interface pointer if the call succeeds.</param>
        /// <param name="pUnknown">Allows for future compatibility with COM aggregation features. Currently, this method returns an error if this parameter is not NULL.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INCOMPATIBLEPRIMARY | DDERR_INVALIDCAPS | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_INVALIDPIXELFORMAT | DDERR_NOALPHAHW | DDERR_NOCOOPERATIVELEVELSET | DDERR_NODIRECTDRAWHW | DDERR_NOEMULATION | DDERR_NOEXCLUSIVEMODE | DDERR_NOFLIPHW | DDERR_NOMIPMAPHW | DDERR_NOOVERLAYHW | DDERR_NOZBUFFERHW | DDERR_OUTOFMEMORY | DDERR_OUTOFVIDEOMEMORY | DDERR_PRIMARYSURFACEALREADYEXISTS | DDERR_UNSUPPORTEDMODE
        /// </returns>
        long CreateSurface(ref DDSURFACEDESC pSurfaceDescription, ref LPDIRECTDRAWSURFACE7 pInstance, IUnknown pUnknown);

        #region unused
        long DuplicateSurface();
        long EnumDisplayModes();
        long EnumSurfaces();
        long FlipToGDISurface();
        long GetCaps();
        long GetDisplayMode(ref DDSURFACEDESC pSurfaceDescription);
        long GetFourCCCodes();
        long GetGDISurface();
        long GetMonitorFrequency();
        long GetScanLine();
        long GetVerticalBlankStatus();
        long Initialize();
        long RestoreDisplayMode();
        #endregion

        /// <summary>
        /// Determines the top-level behavior of the application.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdraw7-setcooperativelevel"/>
        /// <param name="hWnd">Window handle used for the application. Set to the calling application's top-level window handle (not a handle for any child windows created by the top-level window). This parameter can be NULL when the DDSCL_NORMAL flag is specified in the dwFlags parameter.</param>
        /// <param name="value"><see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdraw7-setcooperativelevel#ddscl_allowmodex"/></param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_EXCLUSIVEMODEALREADYSET | DDERR_HWNDALREADYSET | DDERR_HWNDSUBCLASSED | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS |DDERR_OUTOFMEMORY
        /// </returns>
        long SetCooperativeLevel(HWND hWnd, uint value);

        #region unused
        long SetDisplayMode();
        long WaitForVerticalBlank();
        #endregion
    }
}
