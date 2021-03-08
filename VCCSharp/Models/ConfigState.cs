using HWND = System.IntPtr;

namespace VCCSharp.Models
{
    public struct ConfigState
    {
        public unsafe HWND* hWndConfig; //[Define.TABS]
        public HWND hDlgBar;
        public HWND hDlgTape;

        public ConfigModel Model;

        public char TextMode;  //--Add LF to CR
        public char PrintMonitorWindow;

        public byte NumberOfJoysticks;

        public unsafe fixed byte IniFilePath[Define.MAX_PATH];
        public unsafe fixed byte TapeFileName[Define.MAX_PATH];
        public unsafe fixed byte ExecDirectory[Define.MAX_PATH];
        public unsafe fixed byte SerialCaptureFile[Define.MAX_PATH];
        public unsafe fixed byte OutBuffer[Define.MAX_PATH];

        public uint TapeCounter;
        public byte TapeMode;

        public int NumberOfSoundCards;
        public unsafe SoundCardList* SoundCards; //[Define.MAXCARDS];
    }
}
