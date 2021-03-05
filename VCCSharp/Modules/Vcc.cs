using VCCSharp.Libraries;
using VCCSharp.Models;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public class Vcc
    {
        public unsafe VccState* GetVccState()
        {
            return Library.Vcc.GetVccState();
        }

        public void CheckScreenModeChange()
        {
            Library.Vcc.CheckScreenModeChange();
        }

        public HANDLE CreateEventHandle()
        {
            return Library.Vcc.CreateEventHandle();
        }

        public HANDLE CreateThreadHandle(HANDLE hEvent)
        {
            return Library.Vcc.CreateThreadHandle(hEvent);
        }

        public void CreatePrimaryWindow()
        {
            Library.Vcc.CreatePrimaryWindow();
        }

        public void SetAppTitle(HINSTANCE hResources, string binFileName)
        {
            Library.Vcc.SetAppTitle(hResources, binFileName);
        }
    }
}
