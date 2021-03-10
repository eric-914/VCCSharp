namespace VCCSharp.Models
{
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
    }
}
