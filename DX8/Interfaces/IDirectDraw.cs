using System;
using System.Runtime.InteropServices;
using DX8.Models;

namespace DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectDraw)]
    public interface IDirectDraw
    {
        long Compact();
        unsafe long CreateClipper(uint flags, ref IntPtr pInstance, IntPtr pUnknown);
        long CreatePalette();
        unsafe long CreateSurface(DDSURFACEDESC* pSurfaceDescription, ref IntPtr pInstance, IntPtr pUnknown);
        long DuplicateSurface();
        long EnumDisplayModes();
        long EnumSurfaces();
        long FlipToGDISurface();
        long GetCaps();
        unsafe long GetDisplayMode(DDSURFACEDESC* pSurfaceDescription);
        long GetFourCCCodes();
        long GetGDISurface();
        long GetMonitorFrequency();
        long GetScanLine();
        long GetVerticalBlankStatus();
        long Initialize();
        long RestoreDisplayMode();
        long SetCooperativeLevel(IntPtr hWnd, uint value);
        long SetDisplayMode();
        long WaitForVerticalBlank();
    }
}
