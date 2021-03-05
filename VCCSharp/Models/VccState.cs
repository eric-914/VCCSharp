using System.Runtime.InteropServices;
using HANDLE = System.IntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VccState
    {
        public HANDLE hEventThread;
        public HANDLE hEmuThread;  // Message handlers

        public byte AutoStart;
        public byte BinaryRunning;
        public byte DialogOpen;
        public byte FlagEmuStop;
        public byte Throttle;

        //--------------------------------------------------------------------------
        // When the main window is about to lose keyboard focus there are one
        // or two keys down in the emulation that must be raised.  These routines
        // track the last two key down events so they can be raised when needed.
        //--------------------------------------------------------------------------
        public byte SC_save1;
        public byte SC_save2;
        public byte KB_save1;
        public byte KB_save2;

        public int KeySaveToggle;

        public unsafe fixed byte CpuName[20];
        public unsafe fixed byte AppName[100];
    }
}
