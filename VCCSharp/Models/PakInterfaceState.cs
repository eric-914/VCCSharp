using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct PakInterfaceState
    {
        HINSTANCE hInstLib;

        // Storage for Pak ROMs
        unsafe byte* ExternalRomBuffer;
        bool RomPackLoaded;

        uint BankedCartOffset;
        public unsafe fixed byte DllPath[256];
        ushort ModualParms;
        bool DialogOpen;

        public unsafe fixed byte Modname[Define.MAX_PATH];
    }
}
