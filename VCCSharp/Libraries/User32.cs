using System.Drawing;
using System.Windows.Interop;
using VCCSharp.Libraries.Models;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IUser32
    {
        int GetMessageA(ref MSG lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        int TranslateMessage(ref MSG lpMsg);
        LRESULT DispatchMessageA(ref MSG lpMsg);
        int ShowWindow(HWND hWnd, int nCmdShow);
        HWND GetActiveWindow();
        int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);
        int GetClientRect(HWND hWnd, ref RECT lpRect);
        int ClientToScreen(HWND hWnd, ref Point lpPoint);
        int OffsetRect(ref RECT lpRect, int dx, int dy);
        int SetRect(ref RECT lpRect, short xLeft, short yTop, short xRight, short yBottom);
        int GetWindowRect(HWND hWnd, ref RECT lpRect);
        int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint);
        int UpdateWindow(HWND hWnd);
        short GetKeyState(int nVirtualKey);
    }

    public class User32 : IUser32
    {
        public int GetMessageA(ref MSG lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax)
            => User32Dll.GetMessageA(ref lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);

        public int TranslateMessage(ref MSG lpMsg)
            => User32Dll.TranslateMessage(ref lpMsg);

        public LRESULT DispatchMessageA(ref MSG lpMsg)
            => User32Dll.DispatchMessageA(ref lpMsg);

        public int ShowWindow(HWND hWnd, int nCmdShow)
            => User32Dll.ShowWindow(hWnd, nCmdShow);

        public HWND GetActiveWindow()
            => User32Dll.GetActiveWindow();

        public int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags)
            => User32Dll.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);

        public int GetClientRect(HWND hWnd, ref RECT lpRect)
            => User32Dll.GetClientRect(hWnd, ref lpRect);

        public int ClientToScreen(HWND hWnd, ref Point lpPoint)
            => User32Dll.ClientToScreen(hWnd, ref lpPoint);

        public int OffsetRect(ref RECT lpRect, int dx, int dy)
            => User32Dll.OffsetRect(ref lpRect, dx, dy);

        public int SetRect(ref RECT lpRect, short xLeft, short yTop, short xRight, short yBottom)
            => User32Dll.SetRect(ref lpRect, xLeft, yTop, xRight, yBottom);

        public int GetWindowRect(HWND hWnd, ref RECT lpRect)
            => User32Dll.GetWindowRect(hWnd, ref lpRect);

        public int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint)
            => User32Dll.MoveWindow(hWnd, x, y, nWidth, nHeight, bRepaint);

        public int UpdateWindow(HWND hWnd)
            => User32Dll.UpdateWindow(hWnd);

        public short GetKeyState(int nVirtualKey)
            => User32Dll.GetKeyState(nVirtualKey);
    }
}
