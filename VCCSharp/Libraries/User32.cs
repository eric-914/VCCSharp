using System.Runtime.InteropServices;
using System.Windows.Interop;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

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
        public static extern unsafe int GetMessageA(MSG *lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax);

        //BOOL TranslateMessage(
        //const MSG *lpMsg
        //    );
        [DllImport(USER32)]
        public static extern unsafe int TranslateMessage(MSG *lpMsg);

        //LRESULT DispatchMessageA(
        //const MSG *lpMsg
        //    );
        [DllImport(USER32)]
        public static extern unsafe LRESULT DispatchMessageA(MSG *lpMsg);
    }
}
