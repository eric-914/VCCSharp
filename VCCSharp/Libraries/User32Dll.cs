using System.Runtime.InteropServices;
using System.Windows.Interop;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class User32Dll
    {
        public const string DLL = "User32.dll";

        [DllImport(DLL)]
        public static extern unsafe int GetMessageA(MSG *lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax);

        [DllImport(DLL)]
        public static extern unsafe int TranslateMessage(MSG *lpMsg);

        [DllImport(DLL)]
        public static extern unsafe LRESULT DispatchMessageA(MSG *lpMsg);

        [DllImport(DLL)]
        public static extern int ShowWindow(HWND hWnd, int nCmdShow);

        [DllImport(DLL)]
        public static extern HWND GetActiveWindow();

        [DllImport(DLL)]
        public static extern int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int  X, int  Y, int  cx, int  cy, ushort uFlags);
    }
}