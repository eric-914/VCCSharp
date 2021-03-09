using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct PakInterfaceState
    {
        public HINSTANCE hInstLib;

        // Storage for Pak ROMs
        public unsafe byte* ExternalRomBuffer;
        public int RomPackLoaded;

        public uint BankedCartOffset;
        public unsafe fixed byte DllPath[256];
        public ushort ModualParms;
        public int DialogOpen;

        public unsafe fixed byte Modname[Define.MAX_PATH];
    }
}
