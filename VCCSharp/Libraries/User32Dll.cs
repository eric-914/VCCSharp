using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using VCCSharp.Models;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class User32Dll
    {
        public const string DLL = "User32.dll";

        [DllImport(DLL)]
        public static extern unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax);

        [DllImport(DLL)]
        public static extern unsafe int TranslateMessage(MSG* lpMsg);

        [DllImport(DLL)]
        public static extern unsafe LRESULT DispatchMessageA(MSG* lpMsg);

        [DllImport(DLL)]
        public static extern int ShowWindow(HWND hWnd, int nCmdShow);

        [DllImport(DLL)]
        public static extern HWND GetActiveWindow();

        [DllImport(DLL)]
        public static extern int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int X, int Y, int cx, int cy, ushort uFlags);

        [DllImport(DLL)]
        public static extern unsafe int GetClientRect(HWND hWnd, RECT* lpRect);

        [DllImport(DLL)]
        public static extern unsafe int ClientToScreen(HWND hWnd, Point* lpPoint);

        [DllImport(DLL)]
        public static extern unsafe int OffsetRect(RECT* lprc, int dx, int dy);

        [DllImport(DLL)]
        public static extern unsafe int SetRect(RECT* lprc, short xLeft, short yTop, short xRight, short yBottom);

        [DllImport(DLL)]
        public static extern unsafe int GetWindowRect(HWND hWnd, RECT* lpRect);

        [DllImport(DLL)]
        public static extern int MoveWindow(HWND hWnd, int  X, int  Y, int  nWidth, int  nHeight, int bRepaint);
    }
}