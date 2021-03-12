using System.Windows.Interop;
using HWND = System.IntPtr;
using LRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IUser32
    {
        unsafe int GetMessageA(MSG *lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax);
        unsafe int TranslateMessage(MSG *lpMsg);
        unsafe LRESULT DispatchMessageA(MSG *lpMsg);
        int ShowWindow(HWND hWnd, int nCmdShow);
    }

    public class User32 : IUser32
    {
        public unsafe int GetMessageA(MSG *lpMsg, HWND hWnd, ushort wMsgFilterMin, ushort wMsgFilterMax)
            => User32Dll.GetMessageA(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);

        public unsafe int TranslateMessage(MSG *lpMsg)
            => User32Dll.TranslateMessage(lpMsg);

        public unsafe LRESULT DispatchMessageA(MSG *lpMsg)
            => User32Dll.DispatchMessageA(lpMsg);

        public int ShowWindow(HWND hWnd, int nCmdShow)
            => User32Dll.ShowWindow(hWnd, nCmdShow);
    }
}
