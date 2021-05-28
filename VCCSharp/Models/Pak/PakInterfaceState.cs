using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models.Pak
{
    public struct PakInterfaceState
    {
        public HINSTANCE hInstLib;

        public byte CartInserted;

        // Storage for Pak ROMs
        public unsafe byte* ExternalRomBuffer;
        public int RomPackLoaded;

        public uint BankedCartOffset;
        public int DialogOpen;

        public unsafe fixed byte Modname[Define.MAX_PATH];
    }
}
