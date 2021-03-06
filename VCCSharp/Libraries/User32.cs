﻿using System.Drawing;
using System.Windows.Interop;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IUser32
    {
        unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        unsafe int TranslateMessage(MSG* lpMsg);
        unsafe LRESULT DispatchMessageA(MSG* lpMsg);
        int ShowWindow(HWND hWnd, int nCmdShow);
        HWND GetActiveWindow();
        int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);
        unsafe int GetClientRect(HWND hWnd, RECT* lpRect);
        unsafe int ClientToScreen(HWND hWnd, Point* lpPoint);
        unsafe int OffsetRect(RECT* lpRect, int dx, int dy);
        unsafe int SetRect(RECT* lpRect, short xLeft, short yTop, short xRight, short yBottom);
        unsafe int GetWindowRect(HWND hWnd, RECT* lpRect);
        int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint);
        int UpdateWindow(HWND hWnd);
        short GetKeyState(int nVirtualKey);
    }

    public class User32 : IUser32
    {
        public unsafe int GetMessageA(MSG* lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax)
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

        public unsafe int OffsetRect(RECT* lpRect, int dx, int dy)
            => User32Dll.OffsetRect(lpRect, dx, dy);

        public unsafe int SetRect(RECT* lpRect, short xLeft, short yTop, short xRight, short yBottom)
            => User32Dll.SetRect(lpRect, xLeft, yTop, xRight, yBottom);

        public unsafe int GetWindowRect(HWND hWnd, RECT* lpRect)
            => User32Dll.GetWindowRect(hWnd, lpRect);

        public int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint)
            => User32Dll.MoveWindow(hWnd, x, y, nWidth, nHeight, bRepaint);

        public int UpdateWindow(HWND hWnd)
            => User32Dll.UpdateWindow(hWnd);

        public short GetKeyState(int nVirtualKey)
            => User32Dll.GetKeyState(nVirtualKey);
    }
}
