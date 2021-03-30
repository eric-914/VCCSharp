using System.Drawing;
using System.Windows.Interop;
using VCCSharp.Models;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IUser32
    {
        unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax);
        unsafe int TranslateMessage(MSG* lpMsg);
        unsafe LRESULT DispatchMessageA(MSG* lpMsg);
        int ShowWindow(HWND hWnd, int nCmdShow);
        HWND GetActiveWindow();
        int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);
        unsafe int GetClientRect(HWND hWnd, RECT* lpRect);
        unsafe int ClientToScreen(HWND hWnd, Point* lpPoint);
        unsafe int OffsetRect(RECT* lprc, int dx, int dy);
        unsafe int SetRect(RECT* lprc, short xLeft, short yTop, short xRight, short yBottom);
        unsafe int GetWindowRect(HWND hWnd, RECT* lpRect);
        int MoveWindow(HWND hWnd, int X, int Y, int nWidth, int nHeight, int bRepaint);
        int DestroyWindow(HWND hWnd);
        unsafe int AdjustWindowRect(RECT* lpRect, uint dwStyle, int bMenu);
        unsafe HWND CreateWindowExA(uint dwExStyle, byte* lpClassName, byte* lpWindowName, uint dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, void* hMenu, HINSTANCE hInstance, void* lpParam);
        unsafe HWND CreateWindowExA(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, void* hMenu, HINSTANCE hInstance, void* lpParam);
        int UpdateWindow(HWND hWnd);
        unsafe LRESULT SendMessageA(HWND hWnd, uint Msg, uint* wParam, ulong* lParam);
    }

    public class User32 : IUser32
    {
        public unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax)
            => User32Dll.GetMessageA(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);

        public unsafe int TranslateMessage(MSG* lpMsg)
            => User32Dll.TranslateMessage(lpMsg);

        public unsafe LRESULT DispatchMessageA(MSG* lpMsg)
            => User32Dll.DispatchMessageA(lpMsg);

        public int ShowWindow(HWND hWnd, int nCmdShow)
            => User32Dll.ShowWindow(hWnd, nCmdShow);

        public HWND GetActiveWindow()
            => User32Dll.GetActiveWindow();

        public int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags)
            => User32Dll.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);

        public unsafe int GetClientRect(HWND hWnd, RECT* lpRect)
            => User32Dll.GetClientRect(hWnd, lpRect);

        public unsafe int ClientToScreen(HWND hWnd, Point* lpPoint)
            => User32Dll.ClientToScreen(hWnd, lpPoint);

        public unsafe int OffsetRect(RECT* lprc, int dx, int dy)
            => User32Dll.OffsetRect(lprc, dx, dy);

        public unsafe int SetRect(RECT* lprc, short xLeft, short yTop, short xRight, short yBottom)
            => User32Dll.SetRect(lprc, xLeft, yTop, xRight, yBottom);

        public unsafe int GetWindowRect(HWND hWnd, RECT* lpRect)
            => User32Dll.GetWindowRect(hWnd, lpRect);

        public int MoveWindow(HWND hWnd, int X, int Y, int nWidth, int nHeight, int bRepaint)
            => User32Dll.MoveWindow(hWnd, X, Y, nWidth, nHeight, bRepaint);

        public int DestroyWindow(HWND hWnd)
            => User32Dll.DestroyWindow(hWnd);

        public unsafe int AdjustWindowRect(RECT* lpRect, uint dwStyle, int bMenu)
            => User32Dll.AdjustWindowRect(lpRect, dwStyle, bMenu);

        public unsafe HWND CreateWindowExA(uint dwExStyle, byte* lpClassName, byte* lpWindowName, uint dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, void* hMenu, HINSTANCE hInstance, void* lpParam)
            => User32Dll.CreateWindowExA(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

        public unsafe HWND CreateWindowExA(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int X, int Y, int nWidth, int nHeight, HWND hWndParent, void* hMenu, HINSTANCE hInstance, void* lpParam)
            => User32Dll.CreateWindowExA(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

        public int UpdateWindow(HWND hWnd)
            => User32Dll.UpdateWindow(hWnd);

        public unsafe LRESULT SendMessageA(HWND hWnd, uint Msg, uint* wParam, ulong* lParam)
            => User32Dll.SendMessageA(hWnd, Msg, wParam, lParam);
    }
}
