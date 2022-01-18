using System;
using System.Runtime.InteropServices;
using DX8.Models;

namespace DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectDrawSurface)]
    public interface IDirectDrawSurface
    {
        long AddAttachedSurface();
        long AddOverlayDirtyRect();
        unsafe long Blt(DXRECT * rcDest, IDirectDrawSurface back, DXRECT* rcSrc, uint flags, IntPtr pFlags);
        long BltBatch();
        long BltFast();
        long DeleteAttachedSurface();
        long EnumAttachedSurfaces();
        long EnumOverlayZOrders();
        long Flip(IntPtr pUnknown, uint ddFlip);
        long GetAttachedSurface();
        long GetBltStatus();
        long GetCaps();
        long GetClipper();
        long GetColorKey();
        unsafe long GetDC(IntPtr* pHdc);
        long GetFlipStatus();
        long GetOverlayPosition();
        long GetPalette();
        long GetPixelFormat();
        long GetSurfaceDesc();
        long Initialize();
        long IsLost();
        unsafe long Lock(IntPtr rect, DDSURFACEDESC* ddsd, uint flags, IntPtr handle);
        long ReleaseDC(IntPtr hdc);
        long Restore();
        long SetClipper(IDirectDrawClipper clipper);
        long SetColorKey();
        long SetOverlayPosition();
        long SetPalette();
        long Unlock(IntPtr surface);
        long UpdateOverlay();
        long UpdateOverlayDisplay();
        long UpdateOverlayZOrder();
    }
}
