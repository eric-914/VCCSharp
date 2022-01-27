using System.Drawing;
using System.Windows.Interop;
using VCCSharp.Libraries.Models;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IUser32
    {
        /// <summary>
        /// Retrieves a message from the calling thread's message queue. The function dispatches incoming sent messages until a posted message is available for retrieval.
        /// Unlike GetMessage, the PeekMessage function does not wait for a message to be posted before returning.
        /// </summary>
        int GetMessageA(ref MSG lpMsg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        /// <summary>
        /// Translates virtual-key messages into character messages. The character messages are posted to the calling thread's message queue, to be read the next time the thread calls the GetMessage or PeekMessage function.
        /// </summary>
        int TranslateMessage(ref MSG lpMsg);

        /// <summary>
        /// Dispatches a message to a window procedure. It is typically used to dispatch a message retrieved by the GetMessage function.
        /// </summary>
        LRESULT DispatchMessageA(ref MSG lpMsg);

        /// <summary>
        /// Sets the specified window's show state.
        /// </summary>
        int ShowWindow(HWND hWnd, int nCmdShow);

        /// <summary>
        /// Retrieves the window handle to the active window attached to the calling thread's message queue.
        /// </summary>
        HWND GetActiveWindow();

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        int SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, ushort uFlags);

        /// <summary>
        /// Retrieves the coordinates of a window's client area. The client coordinates specify the upper-left and lower-right corners of the client area. Because client coordinates are relative to the upper-left corner of a window's client area, the coordinates of the upper-left corner are (0,0).
        /// </summary>
        int GetClientRect(HWND hWnd, ref RECT lpRect);

        /// <summary>
        /// The ClientToScreen function converts the client-area coordinates of a specified point to screen coordinates.
        /// </summary>
        int ClientToScreen(HWND hWnd, ref Point lpPoint);

        /// <summary>
        /// The OffsetRect function moves the specified rectangle by the specified offsets.
        /// </summary>
        int OffsetRect(ref RECT lpRect, int dx, int dy);

        /// <summary>
        /// The SetRect function sets the coordinates of the specified rectangle. This is equivalent to assigning the left, top, right, and bottom arguments to the appropriate members of the RECT structure.
        /// </summary>
        int SetRect(ref RECT lpRect, short xLeft, short yTop, short xRight, short yBottom);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        int GetWindowRect(HWND hWnd, ref RECT lpRect);

        /// <summary>
        /// Changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
        /// </summary>
        int MoveWindow(HWND hWnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        /// <summary>
        /// The UpdateWindow function updates the client area of the specified window by sending a WM_PAINT message to the window if the window's update region is not empty. The function sends a WM_PAINT message directly to the window procedure of the specified window, bypassing the application queue. If the update region is empty, no message is sent.
        /// </summary>
        int UpdateWindow(HWND hWnd);
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
    }
}
