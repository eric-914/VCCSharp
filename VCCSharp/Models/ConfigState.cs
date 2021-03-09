using System.Runtime.InteropServices;
using HWND = System.IntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConfigState
    {
        public unsafe ConfigModel* Model;

        public byte TextMode;  //--Add LF to CR
        public byte PrintMonitorWindow;

        public byte NumberOfJoysticks;

        public unsafe fixed byte IniFilePath[Define.MAX_PATH];
        public unsafe fixed byte TapeFileName[Define.MAX_PATH];
        public unsafe fixed byte ExecDirectory[Define.MAX_PATH];
        public unsafe fixed byte SerialCaptureFile[Define.MAX_PATH];
        public unsafe fixed byte OutBuffer[Define.MAX_PATH];

        public uint TapeCounter;
        public byte TapeMode;

        public int NumberOfSoundCards;

        //TODO: SoundCardList* is really a pointer to an array of SoundCardList items.  Haven't figured how to define it as such yet.
        public unsafe SoundCardList* SoundCards; //[Define.MAXCARDS];

        //TODO: HWND* is really a pointer to an array of HWND items.  Haven't figured how to define it as such yet.
        public unsafe HWND* hWndConfig; //[Define.TABS]
        public HWND hDlgBar;
        public HWND hDlgTape;
    }
}
