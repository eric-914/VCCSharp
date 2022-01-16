using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectDraw)]
    public interface IDirectDraw
    {
        long Compact();
        unsafe long CreateClipper(uint flags, IntPtr* clipper, IntPtr pUnknown);
        long CreatePalette();
        unsafe long CreateSurface(DDSURFACEDESC* ddsd, IntPtr* surface, IntPtr pUnknown);
        long DuplicateSurface();
        long EnumDisplayModes();
        long EnumSurfaces();
        long FlipToGDISurface();
        long GetCaps();
        unsafe long GetDisplayMode(DDSURFACEDESC* ddsd);
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
