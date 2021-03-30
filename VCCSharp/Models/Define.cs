namespace VCCSharp.Models
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public static class Define
    {
        public const byte TRUE = 1;     //--Need to be careful, as TRUE doesn't necessarily mean NOT FALSE
        public const byte FALSE = 0;

        public const int IGNORE = 0;

        public const uint INFINITE = 0xFFFFFFFF;
        public const short THREAD_PRIORITY_NORMAL = 0;

        public const int MAX_LOADSTRING = 100;

        public const int MAX_PATH = 260;

        public const int TABS = 8;

        public const long S_OK = 0;
        public const long DS_OK = S_OK;

        public const int MAXCARDS = 12;

        public const ushort FRAMEINTERVAL = 120;
        public const int TARGETFRAMERATE = 60;

        public const int AUDIOBUFFERS = 12;

        public const int TAPEAUDIORATE = 44100;

        public const uint FILE_BEGIN = 0;

        //--This seems to be use the default window tiling of new windows feature.
        public const int CW_USEDEFAULT = -1 ^ 0x7FFFFFFF; //0x80000000

        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_CAPTION = 0x00C00000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_POPUP = 0x80000000;

        public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;

        public const uint SBARS_SIZEGRIP = 0x0100;

        public const uint DDSCL_NORMAL = 0x00000008;

        public const uint DDSCAPS_PRIMARYSURFACE = 0x00000200;
        public const uint DDSCAPS_VIDEOMEMORY = 0x00004000;
        public const uint DDSCAPS_SYSTEMMEMORY = 0x00000800;

        public const uint DDSD_CAPS = 0x00000001;
        public const uint DDSD_HEIGHT = 0x00000002;
        public const uint DDSD_WIDTH = 0x00000004;
        public const uint DDSD_BACKBUFFERCOUNT = 0x00000020;
        public const uint DDSCAPS_COMPLEX = 0x00000008;
        public const uint DDSCAPS_FLIP = 0x00000010;
        public const uint DDSCAPS_BACKBUFFER = 0x00000004;

        public const uint DDSCL_EXCLUSIVE = 0x00000010;
        public const uint DDSCL_FULLSCREEN = 0x00000001;
        public const uint DDSCL_NOWINDOWCHANGES = 0x00000004;

        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWDEFAULT = 10;

        public const uint WM_SIZE = 0x0005;
        public const uint WM_SETTEXT = 0x000C;

        public const string STATUSCLASSNAME = "msctls_statusbar32";
    }
}
