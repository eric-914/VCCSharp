using System;

namespace VCCSharp.Libraries
{
    public interface IGdi32
    {
        int TextOut(IntPtr hdc, int x, int y, string lpString, int c);
        uint SetTextColor(IntPtr hdc, uint color);
        uint SetBkColor(IntPtr hdc, uint color);
    }

    public class Gdi32 : IGdi32
    {
        public int TextOut(IntPtr hdc, int x, int y, string lpString, int c)
        {
            return Gdi32Dll.TextOutA(hdc, x, y, lpString, c);
        }

        public uint SetTextColor(IntPtr hdc, uint color)
        {
            return Gdi32Dll.SetTextColor(hdc, color);
        }

        public uint SetBkColor(IntPtr hdc, uint color)
        {
            return Gdi32Dll.SetBkColor(hdc, color);
        }
    }
}
