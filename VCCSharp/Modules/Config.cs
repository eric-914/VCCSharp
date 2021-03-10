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
        string ExternalBasicImage();
        void UpdateSoundBar(ushort left, ushort right);
    }

    public class Config : IConfig
    {
        public unsafe ConfigState* GetConfigState()
        {
            return Library.Config.GetConfigState();
        }

        public unsafe ConfigModel* GetConfigModel()
        {
            return Library.Config.GetConfigModel();
        }
        
        public unsafe JoystickModel* GetLeftJoystick()
        {
            return Library.Config.GetLeftJoystick();
        }
        
        public unsafe JoystickModel* GetRightJoystick()
        {
            return Library.Config.GetRightJoystick();
        }
        
        public unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs)
        {
            Library.Config.InitConfig(emuState, ref cmdLineArgs);

            //var left = GetLeftJoystick();
            //var right = GetRightJoystick();
            //var model = GetConfigModel();
            //var state = GetConfigState();
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

        public string ExternalBasicImage()
        {
            unsafe
            {
                byte* data = Library.Config.ExternalBasicImage();

                return Converter.ToString(data);
            }
        }

        public void UpdateSoundBar(ushort left, ushort right)
        {
            Library.Config.UpdateSoundBar(left, right);
        }
    }
}
