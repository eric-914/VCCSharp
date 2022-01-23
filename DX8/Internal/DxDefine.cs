namespace DX8.Internal
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    internal static class DxDefine
    {
        public const byte TRUE = 1;     //--Need to be careful, as TRUE doesn't necessarily mean NOT FALSE
        public const byte FALSE = 0;

        public const long S_OK = 0;

        public const int MAX_LOADSTRING = 100;
        public const int MAXCARDS = 12;
        public const uint MAX_JOYSTICKS = 8;

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

        //public const uint DI8DEVCLASS_ALL = 0;
        //public const uint DI8DEVCLASS_DEVICE = 1;
        //public const uint DI8DEVCLASS_POINTER = 2;
        //public const uint DI8DEVCLASS_KEYBOARD = 3;
        public const uint DI8DEVCLASS_GAMECTRL = 4;

        //public const uint DIEDFL_ALLDEVICES = 0x00000000;
        public const uint DIEDFL_ATTACHEDONLY = 0x00000001;

        public const int DIENUM_STOP = 0;
        public const int DIENUM_CONTINUE = 1;

        //public const uint DIPH_DEVICE = 0;
        //public const uint DIPH_BYOFFSET = 1;
        public const uint DIPH_BYID = 2;
        //public const uint DIPH_BYUSAGE = ?;

        //public const uint DIDFT_RELAXIS = 0x00000001;
        //public const uint DIDFT_ABSAXIS = 0x00000002;
        public const uint DIDFT_AXIS = 0x00000003;

        //public const long SEVERITY_SUCCESS = 0;
        public const long SEVERITY_ERROR = 1;

        //public const long FACILITY_NULL = 0;
        //public const long FACILITY_RPC = 1;
        //public const long FACILITY_DISPATCH = 2;
        //public const long FACILITY_STORAGE = 3;
        //public const long FACILITY_ITF = 4;
        public const long FACILITY_WIN32 = 7;
        //public const long FACILITY_WINDOWS = 8;

        /// <summary>
        /// The system cannot read from the specified device.
        /// </summary>
        public const long ERROR_READ_FAULT = 30L;

        /// <summary>
        /// One or more arguments are invalid
        /// </summary>
        public const long E_INVALIDARG = 0x80070057L;

        /// <summary>
        /// General access denied error
        /// </summary>
        public const long E_ACCESSDENIED = 0x80070005L;

        /// <summary>
        /// Unspecified error
        /// </summary>
        public const long E_FAIL = 0x80004005L;

        public const long DIERR_INPUTLOST = (SEVERITY_ERROR << 31) | (FACILITY_WIN32 << 16) | ERROR_READ_FAULT;
        public const long DIERR_INVALIDPARAM = E_INVALIDARG;

        /// <summary>
        /// Another app has a higher priority level, preventing this call from succeeding.
        /// </summary>
        public const long DIERR_OTHERAPPHASPRIO = E_ACCESSDENIED;
    }
}
