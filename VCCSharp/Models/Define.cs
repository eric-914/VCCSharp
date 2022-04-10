namespace VCCSharp.Models;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
public static class Define
{
    public static readonly IntPtr INVALID_HANDLE_VALUE = IntPtr.Add(IntPtr.Zero, -1);

    public const int IGNORE = 0;

    public const ushort FRAMEINTERVAL = 120;
    public const int TARGETFRAMERATE = 60;
    public const int LINESPERFIELD = 262;

    public const double FRAMESPERSECORD = 59.923;   //The coco really runs at about 59.923 Frames per second
    public const double PICOSECOND = 1000000000;
    public const double COLORBURST = 3579545;

    public const int AUDIOBUFFERS = 12;

    public const int TAPEAUDIORATE = 44100;

    public const uint DDSCL_NORMAL = 0x00000008;

    public const int SW_SHOWDEFAULT = 10;

    // MC6809 Vector table
    public const ushort VTRAP = 0xFFF0;
    public const ushort VSWI3 = 0xFFF2;
    public const ushort VSWI2 = 0xFFF4;
    public const ushort VFIRQ = 0xFFF6;
    public const ushort VIRQ = 0xFFF8;
    public const ushort VSWI = 0xFFFA;
    public const ushort VNMI = 0xFFFC;
    public const ushort VRESET = 0xFFFE;

    public const int KBTABLE_ENTRY_COUNT = 100; //< key translation table maximum size, (arbitrary) most of the layouts are < 80 entries

    //Defines the start and end IDs for the dynamic menus
    public const int ID_DYNAMENU_START = 5000;
    public const int ID_DYNAMENU_END = 5100;

    public const int KEY_DOWN = 1;
    public const int KEY_UP = 0;

    public const byte EMU_RUNSTATE_RUNNING = 0;

    public const byte CAS = 1;
    public const byte WAV = 0;

    public const uint WRITEBUFFERSIZE = 0x1FFFF;

    public const uint GENERIC_READ = 0x80000000;
    public const uint GENERIC_WRITE = 0x40000000;

    public const uint FILE_BEGIN = 0;
    public const uint FILE_END = 2;

    public const int NOMODULE = 1;
    public const int NOTVCC = 2;

    public const int MENU_PARENT = 0;
    public const int MENU_CHILD = 1;
    public const int MENU_STANDALONE = 2;

    public const int PAK_MAX_MEM = 0x40000;

    public const int DEFAULT_WIDTH = 640;
    public const int DEFAULT_HEIGHT = 480;

    public const byte _192Lines = 0; //--Indexer
    public const byte _225Lines = 1;

    public const uint CREATE_ALWAYS = 2;
    public const uint OPEN_ALWAYS = 4;
}
