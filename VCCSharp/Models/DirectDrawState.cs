namespace VCCSharp.Models
{
    public struct DirectDrawState
    {
        public unsafe fixed byte AppNameText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte TitleBarText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte StatusText[255];
    }
}
