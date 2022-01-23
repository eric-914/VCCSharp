namespace DX8
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public static class DxDefine
    {
        public const long S_OK = 0;

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
    }
}
