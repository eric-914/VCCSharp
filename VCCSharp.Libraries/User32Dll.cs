using System.Drawing;
using System.Runtime.InteropServices;
using VCCSharp.Libraries.Models;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class User32Dll
    {
        public const string Dll = "User32.dll";

        [DllImport(Dll)]
        public static extern int GetMessageA(ref MSG lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport(Dll)]
        public static extern int TranslateMessage(ref MSG lpMsg);

        [DllImport(Dll)]
        public static extern LRESULT DispatchMessageA(ref MSG lpMsg);

        [DllImport(Dll)]
        public static extern int ShowWindow(HWND hWnd, int nCmdShow);

        [DllImport(Dll)]
        public static extern HWND GetActiveWindow();

        [DllImport(Dll)]
        public static extern int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);

        [DllImport(Dll)]
        public static extern int GetClientRect(HWND hWnd, ref RECT lpRect);

        [DllImport(Dll)]
        public static extern int ClientToScreen(HWND hWnd, ref Point lpPoint);

        [DllImport(Dll)]
        public static extern int OffsetRect(ref RECT lpRect, int dx, int dy);

        [DllImport(Dll)]
        public static extern int SetRect(ref RECT lpRect, short xLeft, short yTop, short xRight, short yBottom);

        [DllImport(Dll)]
        public static extern int GetWindowRect(HWND hWnd, ref RECT lpRect);

        [DllImport(Dll)]
        public static extern int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        [DllImport(Dll)]
        public static extern int UpdateWindow(HWND hWnd);
    }
}
