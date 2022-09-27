using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    public static class Gdi32Dll
    {
        public const string Dll = "Gdi32.dll";

        [DllImport(Dll)]
        public static extern int TextOutA(IntPtr hdc, int x, int y, string lpString, int c);

        [DllImport(Dll)]
        public static extern uint SetTextColor(IntPtr hdc, uint color);

        [DllImport(Dll)]
        public static extern uint SetBkColor(IntPtr hdc, uint color);
    }
}
