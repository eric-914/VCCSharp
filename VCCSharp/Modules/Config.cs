using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IConfig
    {
        unsafe ConfigState* GetConfigState();
        unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);
        unsafe void WriteIniFile(EmuState* emuState);
        unsafe void SynchSystemWithConfig(EmuState* emuState);
        int GetPaletteType();
    }

    public class Config : IConfig
    {
        public unsafe ConfigState* GetConfigState()
        {
            return Library.Config.GetConfigState();
        }

        public unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs)
        {
            Library.Config.InitConfig(emuState, ref cmdLineArgs);

            var tmp = GetConfigState();
        }

        public unsafe void WriteIniFile(EmuState* emuState)
        {
            Library.Config.WriteIniFile(emuState);
        }

        public unsafe void SynchSystemWithConfig(EmuState* emuState)
        {
            Library.Config.SynchSystemWithConfig(emuState);
        }

        public int GetPaletteType()
        {
            return Library.Config.GetPaletteType();
        }
    }
}
