using VCCSharp.Libraries;
using VCCSharp.Models;
using HCURSOR = System.IntPtr;
using HICON = System.IntPtr;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IGDI
    {
        unsafe void WriteTextOut(void* hdc, ushort x, ushort y, string message);
        unsafe void SetBkColor(void* hdc, uint color);
        unsafe void SetTextColor(void* hdc, uint color);
        unsafe void TextOut(void* hdc, int x, int y, string text, int textLength);
        HICON GetIcon(HINSTANCE resources);
        HCURSOR GetCursor(bool fullscreen);
        unsafe void* GetBrush();
        unsafe void GetClientRect(HWND hWnd, RECT* clientSize);
    }

    // ReSharper disable once InconsistentNaming
    public class GDI : IGDI
    {
        public unsafe void WriteTextOut(void* hdc, ushort x, ushort y, string message)
        {
            Library.GDI.GDIWriteTextOut(hdc, x, y, message);
        }

        public unsafe void SetBkColor(void* hdc, uint color)
        {
            Library.GDI.GDISetBkColor(hdc, color);
        }

        public unsafe void SetTextColor(void* hdc, uint color)
        {
            Library.GDI.GDISetTextColor(hdc, color);
        }

        public unsafe void TextOut(void* hdc, int x, int y, string text, int textLength)
        {
            Library.GDI.GDITextOut(hdc, x, y, text, textLength);
        }

        public HICON GetIcon(HINSTANCE resources)
        {
            return Library.GDI.GDIGetIcon(resources);
        }

        public HCURSOR GetCursor(bool fullscreen)
        {
            return Library.GDI.GDIGetCursor(fullscreen ? Define.TRUE : Define.FALSE);
        }

        public unsafe void* GetBrush()
        {
            return Library.GDI.GDIGetBrush();
        }

        public unsafe void GetClientRect(HWND hWnd, RECT* clientSize)
        {
            Library.GDI.GDIGetClientRect(hWnd, clientSize);
        }
    }
}
