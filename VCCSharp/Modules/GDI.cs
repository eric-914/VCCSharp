﻿using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IGDI
    {
        unsafe void GDIWriteTextOut(void* hdc, ushort x, ushort y, string message);
        unsafe void GDISetBkColor(void* hdc, uint color);
        unsafe void GDISetTextColor(void* hdc, uint color);
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
    }
}
