// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using DX8.Internal.Models;
using System.Runtime.InteropServices;
using HANDLE = System.IntPtr;
using HDC = System.IntPtr;
using LPDDBLTFX = System.IntPtr;
using LPDIRECTDRAWSURFACE7 = System.IntPtr;
using LPRECT = System.IntPtr;

namespace DX8.Internal.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectDrawSurface)]
    internal interface IDirectDrawSurface
    {
        #region unused
        long AddAttachedSurface();
        long AddOverlayDirtyRect();
        #endregion

        /// <summary>
        /// Performs a bit block transfer (bitblt). This method does not support z-buffering or alpha blending during bitblt operations.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-blt"/>
        /// <param name="rcDest">A pointer to a RECT structure that defines the upper-left and lower-right points of the rectangle to bitblt to on the destination surface. If this parameter is NULL, the entire destination surface is used.</param>
        /// <param name="back">A pointer to the IDirectDrawSurface7 interface for the DirectDrawSurface object that is the source of the bitblt.</param>
        /// <param name="rcSrc">A pointer to a RECT structure that defines the upper-left and lower-right points of the rectangle to bitblt from on the source surface. If this parameter is NULL, the entire source surface is used.</param>
        /// <param name="flags">A combination of flags that determine the valid members of the associated DDBLTFX structure, specify color-key information, or request special behavior from the method. </param>
        /// <param name="pFlags">A pointer to the DDBLTFX structure for the bitblt.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_GENERIC | DDERR_INVALIDCLIPLIST | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_INVALIDRECT | DDERR_NOALPHAHW | DDERR_NOBLTHW | DDERR_NOCLIPLIST | DDERR_NODDROPSHW | DDERR_NOMIRRORHW | DDERR_NORASTEROPHW | DDERR_NOROTATIONHW | DDERR_NOSTRETCHHW | DDERR_NOZBUFFERHW | DDERR_SURFACEBUSY | DDERR_SURFACELOST | DDERR_UNSUPPORTED | DDERR_WASSTILLDRAWING
        /// </returns>
        /// <remarks>Blt can perform synchronous or asynchronous bitblts (the latter is the default behavior). These bitblts can occur from display memory to display memory, from display memory to system memory, from system memory to display memory, or from system memory to system memory. The bitblts can be performed by using source color keys and destination color keys. Arbitrary stretching or shrinking is performed if the source and destination rectangles are not the same size.</remarks>
        long Blt(ref DXRECT rcDest, IDirectDrawSurface back, ref DXRECT rcSrc, uint flags, LPDDBLTFX pFlags);

        #region unused
        long BltBatch();
        long BltFast();
        long DeleteAttachedSurface();
        long EnumAttachedSurfaces();
        long EnumOverlayZOrders();
        #endregion

        /// <summary>
        /// Makes the surface memory that is associated with the DDSCAPS_BACKBUFFER surface become associated with the front-buffer surface.
        /// </summary>
        /// <see hcref="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-flip"/>
        /// <param name="pUnknown">A pointer to the IDirectDrawSurface7 interface for an arbitrary surface in the flipping chain. The default for this parameter is NULL, in which case DirectDraw cycles through the buffers in the order that they are attached to each other. If this parameter is not NULL, DirectDraw flips to the specified surface, instead of the next surface in the flipping chain. Flip fails if the specified surface is not a member of the flipping chain.</param>
        /// <param name="ddFlip">A combination of flags that specify flip options.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_GENERIC | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_NOFLIPHW | DDERR_NOTFLIPPABLE | DDERR_SURFACEBUSY | DDERR_SURFACELOST | DDERR_UNSUPPORTED | DDERR_WASSTILLDRAWING
        /// </returns>
        long Flip(LPDIRECTDRAWSURFACE7 pUnknown, /*DWORD*/ uint ddFlip);

        #region unused
        long GetAttachedSurface();
        long GetBltStatus();
        long GetCaps();
        long GetClipper();
        long GetColorKey();
        #endregion

        /// <summary>
        /// Creates a GDI-compatible handle of a device context for this surface.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-getdc"/>
        /// <param name="pHdc">A pointer to a variable that receives the handle of the device context for this surface.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_DCALREADYCREATED | DDERR_GENERIC | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_INVALIDSURFACETYPE | DDERR_SURFACELOST | DDERR_UNSUPPORTED | DDERR_WASSTILLDRAWING
        /// </returns>
        /// <remarks>GetDC uses an internal version of the Lock method to lock the surface. The surface remains locked until the ReleaseDC method is called.</remarks>
        long GetDC(ref HDC pHdc);

        #region unused
        long GetFlipStatus();
        long GetOverlayPosition();
        long GetPalette();
        long GetPixelFormat();
        long GetSurfaceDesc();
        long Initialize();
        #endregion

        /// <summary>
        /// Determines whether the surface memory that is associated with a DirectDrawSurface object has been freed.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-islost"/>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK because the memory has not been freed.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_SURFACELOST
        /// </returns>
        /// <remarks>Surfaces can lose their memory when the mode of the graphics adapter is changed or when an application receives exclusive access to the graphics adapter and frees all surface memory that is currently allocated on the graphics adapter.</remarks>
        long IsLost();

        /// <summary>
        /// Obtains a pointer to the surface memory.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-lock"/>
        /// <param name="rect">A pointer to a RECT structure that identifies the region of the surface that is being locked. If this parameter is NULL, the entire surface is locked.</param>
        /// <param name="ddsd">A pointer to a DDSURFACEDESC2 structure that describes relevant details about the surface and that receives information about the surface.</param>
        /// <param name="flags">A combination of flags that determine how to lock the surface.</param>
        /// <param name="handle">Handle of the event. This parameter is not currently used and must be set to NULL.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_OUTOFMEMORY | DDERR_SURFACEBUSY | DDERR_SURFACELOST | DDERR_WASSTILLDRAWING
        /// </returns>
        /// <remarks>In IDirectDrawSurface7, the default behavior of Lock is to wait for the accelerator to finish. Therefore, under default conditions, Lock never returns DDERR_WASSTILLDRAWING. If you want to see the error codes and not wait until the bitblt operation succeeds, use the DDLOCK_DONOTWAIT flag.</remarks>
        long Lock(LPRECT rect, ref DDSURFACEDESC ddsd, /*DWORD*/ uint flags, HANDLE handle);

        /// <summary>
        /// Releases the handle of a device context that was previously obtained by using the GetDC method.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-releasedc"/>
        /// <param name="hdc">The handle of a device context that was previously obtained by GetDC.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_GENERIC | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_SURFACELOST | DDERR_UNSUPPORTED
        /// </returns>
        /// <remarks>ReleaseDC also unlocks the surface that was previously locked when the GetDC method was called.</remarks>
        long ReleaseDC(HDC hdc);

        /// <summary>
        /// Restores a surface that has been lost. This occurs when the surface memory that is associated with the DirectDrawSurface object has been freed.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-restore"/>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_GENERIC | DDERR_IMPLICITLYCREATED | DDERR_INCOMPATIBLEPRIMARY | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_NOEXCLUSIVEMODE | DDERR_OUTOFMEMORY | DDERR_UNSUPPORTED | DDERR_WRONGMODE
        /// </returns>
        /// <remarks>Restore restores the memory that was allocated for a surface, but does not reload any bitmaps that might have existed in the surface before it was lost.</remarks>
        long Restore();

        /// <summary>
        /// Attaches a clipper object to, or deletes one from, this surface.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-setclipper"/>
        /// <param name="clipper">A pointer to the IDirectDrawClipper interface for the DirectDrawClipper object to be attached to the DirectDrawSurface object. If you set this parameter to NULL, the current DirectDrawClipper object is detached.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_INVALIDSURFACETYPE | DDERR_NOCLIPPERATTACHED
        /// </returns>
        /// <remarks>When you set a clipper to a surface for the first time, SetClipper increments the clipper's reference count; subsequent calls do not affect the clipper's reference count. If you pass NULL as the lpDDClipper parameter, the clipper is removed from the surface, and the clipper's reference count is decremented. If you do not delete the clipper, the surface automatically releases its reference to the clipper when the surface itself is released. According to COM rules, your application must release any references that it holds to the clipper when the object is no longer needed.</remarks>
        long SetClipper(IDirectDrawClipper clipper);

        #region unused
        long SetColorKey();
        long SetOverlayPosition();
        long SetPalette();
        #endregion

        /// <summary>
        /// Notifies DirectDraw that the direct surface manipulations are complete.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-idirectdrawsurface7-unlock"/>
        /// <param name="surface">A pointer to a RECT structure that was used to lock the surface in the corresponding call to the Lock method. This parameter can be NULL only if the entire surface was locked by passing NULL in the lpDestRect parameter of the corresponding call to the Lock method.</param>
        /// <returns>
        /// If the method succeeds, the return value is DD_OK.
        /// If it fails, the method can return one of the following error values:
        /// DDERR_GENERIC | DDERR_INVALIDOBJECT | DDERR_INVALIDPARAMS | DDERR_INVALIDRECT | DDERR_NOTLOCKED | DDERR_SURFACELOST
        /// </returns>
        /// <remarks>Because you can call Lock multiple times for the same surface with different destination rectangles, the pointer in lpRect links the calls to the Lock and Unlock methods.</remarks>
        long Unlock(LPRECT surface);

        #region unused
        long UpdateOverlay();
        long UpdateOverlayDisplay();
        long UpdateOverlayZOrder();
        #endregion
    }
}
