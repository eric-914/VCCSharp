using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IGDI
    {
        unsafe void WriteTextOut(void* hdc, ushort x, ushort y, string message);
        unsafe void SetBkColor(void* hdc, uint color);
        unsafe void SetTextColor(void* hdc, uint color);
        unsafe void TextOut(void* hdc, int x, int y, string text, int textLength);
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
    }
}
