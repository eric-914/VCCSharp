﻿using System;
using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IGDI
    {
        unsafe void GDIWriteTextOut(void* hdc, ushort x, ushort y, string message);
        unsafe void GDISetBkColor(void* hdc, uint color);
        unsafe void GDISetTextColor(void* hdc, uint color);
        unsafe void GDITextOut(void* hdc, int x, int y, string text, int textLength);
        unsafe void* GetIcon(IntPtr resources);
        unsafe void* GetCursor(byte fullscreen);
        unsafe void* GetBrush();
    }

    public class GDI : IGDI
    {
        public unsafe void GDIWriteTextOut(void* hdc, ushort x, ushort y, string message)
        {
            Library.GDI.GDIWriteTextOut(hdc, x, y, message);
        }

        public unsafe void GDISetBkColor(void* hdc, uint color)
        {
            Library.GDI.GDISetBkColor(hdc, color);
        }

        public unsafe void GDISetTextColor(void* hdc, uint color)
        {
            Library.GDI.GDISetTextColor(hdc, color);
        }

        public unsafe void GDITextOut(void* hdc, int x, int y, string text, int textLength)
        {
            Library.GDI.GDITextOut(hdc, x, y, text, textLength);
        }

        public unsafe void* GetIcon(IntPtr resources)
        {
            return Library.GDI.GDIGetIcon(resources);
        }

        public unsafe void* GetCursor(byte fullscreen)
        {
            return Library.GDI.GDIGetCursor(fullscreen);
        }

        public unsafe void* GetBrush()
        {
            return Library.GDI.GDIGetBrush();
        }
    }
}
