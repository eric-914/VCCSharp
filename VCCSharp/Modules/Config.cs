using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public class Config
    {
        public unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs)
        {
            Library.Config.InitConfig(emuState, ref cmdLineArgs);
        }

        public unsafe void WriteIniFile(EmuState* emuState)
        {
            Library.Config.WriteIniFile(emuState);
        }
    }
}
