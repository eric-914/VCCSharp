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
        public const string Dll = "User32.dll";

        [DllImport(Dll)]
        public static extern unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport(Dll)]
        public static extern unsafe int TranslateMessage(MSG* lpMsg);

        [DllImport(Dll)]
        public static extern unsafe LRESULT DispatchMessageA(MSG* lpMsg);

        [DllImport(Dll)]
        public static extern int ShowWindow(HWND hWnd, int nCmdShow);

        [DllImport(Dll)]
        public static extern HWND GetActiveWindow();

        [DllImport(Dll)]
        public static extern int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);

        [DllImport(Dll)]
        public static extern unsafe int GetClientRect(HWND hWnd, RECT* lpRect);

        [DllImport(Dll)]
        public static extern unsafe int ClientToScreen(HWND hWnd, Point* lpPoint);

        [DllImport(Dll)]
        public static extern unsafe int OffsetRect(RECT* lpRect, int dx, int dy);

        [DllImport(Dll)]
        public static extern unsafe int SetRect(RECT* lpRect, short xLeft, short yTop, short xRight, short yBottom);

        [DllImport(Dll)]
        public static extern unsafe int GetWindowRect(HWND hWnd, RECT* lpRect);

        [DllImport(Dll)]
        public static extern int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        [DllImport(Dll)]
        public static extern int UpdateWindow(HWND hWnd);

        [DllImport(Dll)]
        public static extern short GetKeyState(int nVirtualKey);
    }
}