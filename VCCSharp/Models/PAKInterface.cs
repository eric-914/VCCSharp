using VCCSharp.Libraries;

namespace VCCSharp.Models
{
    public class PAKInterface
    {
        public unsafe void UnloadDll(EmuState* emuState)
        {
            Library.PAKInterface.UnloadDll(emuState);
        }
    }
}
