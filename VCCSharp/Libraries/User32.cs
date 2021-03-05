using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    public static class User32
    {
        public const string USER32 = "User32.dll";

        //BOOL GetMessageA(
        //    LPMSG lpMsg,
        //    HWND  hWnd,
        //    UINT  wMsgFilterMin,
        //    UINT  wMsgFilterMax
        //);
        [DllImport(USER32)]
        public static extern int GetMessageA();
    }
}
