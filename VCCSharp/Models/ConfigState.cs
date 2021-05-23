using System.Runtime.InteropServices;
using HWND = System.IntPtr;

namespace VCCSharp.Models
{
    //TODO: Figure out how to convince C# to let me turn this into an array of MAXCARD items
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SoundCardListArray
    {
        //[Define.MAXCARDS] = 12;
        public SoundCardList _0;
        public SoundCardList _1;
        public SoundCardList _2;
        public SoundCardList _3;
        public SoundCardList _4;
        public SoundCardList _5;
        public SoundCardList _6;
        public SoundCardList _7;
        public SoundCardList _8;
        public SoundCardList _9;
        public SoundCardList _A;
        public SoundCardList _B;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConfigState
    {
        public byte TextMode;  //--Add LF to CR
        public byte PrintMonitorWindow;

        public byte NumberOfJoysticks;

        public ushort TapeCounter;
        public byte TapeMode;

        public short NumberOfSoundCards;
        
        //public unsafe fixed SoundCardList SoundCards[Define.MAXCARDS];
        //TODO: SoundCardList* is really a pointer to an array of SoundCardList items.  Haven't figured how to define it as such yet.
        public SoundCardListArray SoundCards;

        public unsafe fixed byte AppDataPath[Define.MAX_PATH];
        public unsafe fixed byte IniFilePath[Define.MAX_PATH];
        public unsafe fixed byte TapeFileName[Define.MAX_PATH];
        public unsafe fixed byte ExecDirectory[Define.MAX_PATH];
        public unsafe fixed byte SerialCaptureFile[Define.MAX_PATH];
        public unsafe fixed byte OutBuffer[Define.MAX_PATH];

        public HWND hDlgBar;
        public HWND hDlgTape;
    }

    public static class SoundCardListExtensions
    {
        //TODO: I want my arrays!
        public static SoundCardList[] ToArray(this SoundCardListArray source)
        {
            return new []
            {
                source._0, source._1, source._2, source._3,
                source._4, source._5, source._6, source._7,
                source._8, source._9, source._A, source._B
            };
        }
    }
}
