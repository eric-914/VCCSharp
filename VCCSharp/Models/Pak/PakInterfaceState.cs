using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models.Pak
{
    public struct PakInterfaceState
    {
        public HINSTANCE hInstLib;

        // Storage for Pak ROMs
        public unsafe byte* ExternalRomBuffer;

        public uint BankedCartOffset;
    }
}
