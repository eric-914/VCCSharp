namespace DX8
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public static class DxDefine
    {
        public const byte TRUE = 1;     //--Need to be careful, as TRUE doesn't necessarily mean NOT FALSE
        public const byte FALSE = 0;

        public const long S_OK = 0;

        public const int MAX_LOADSTRING = 100;
        public const int MAXCARDS = 12;

        public const uint DDFLIP_NOVSYNC = 0x00000008;
        public const uint DDFLIP_DONOTWAIT = 0x00000020;

        public const uint DDBLT_WAIT = 0x01000000;

        public const long DDLOCK_SURFACEMEMORYPTR = 0x00000000L;     // default
        public const long DDLOCK_WAIT = 0x00000001L;

        public const uint DDSCAPS_PRIMARYSURFACE = 0x00000200;
        public const uint DDSCAPS_VIDEOMEMORY = 0x00004000;

        public const uint DDSD_CAPS = 0x00000001;
        public const uint DDSD_HEIGHT = 0x00000002;
        public const uint DDSD_WIDTH = 0x00000004;

        public const int DSBCAPS_STATIC = 0x00000002;
        public const int DSBCAPS_LOCSOFTWARE = 0x00000008;
        public const int DSBCAPS_GLOBALFOCUS = 0x00008000;
        public const int DSBCAPS_GETCURRENTPOSITION2 = 0x00010000;

        public const int DSSCL_NORMAL = 0x00000001;

        public const ushort WAVE_FORMAT_PCM = 1;

        public const uint DSBPLAY_LOOPING = 0x00000001;

        public const ushort CHANNELS = 2;
        public const ushort BITSPERSAMPLE = 16;
        public const ushort BLOCKALIGN = (BITSPERSAMPLE * CHANNELS) >> 3;
    }
}
