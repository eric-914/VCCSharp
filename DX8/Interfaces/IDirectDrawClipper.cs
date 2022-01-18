using System;
using System.Runtime.InteropServices;

namespace DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectDrawClipper)]
    public interface IDirectDrawClipper
    {
        long GetClipList();
        long GetHWnd();
        long Initialize();
        long IsClipListChanged();
        long SetClipList();
        long SetHWnd(uint zero, IntPtr hWnd);
    }
}
