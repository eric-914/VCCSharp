using VCCSharp.IoC;
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
        private readonly IModules _modules;

        public Config(IModules modules)
        {
            _modules = modules;
        }

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
            ConfigState* configState = GetConfigState();

            string appTitle = _modules.Resource.ResourceAppTitle(emuState->Resources);
            string iniFile = GetIniFilePath(cmdLineArgs.IniFile);

            Converter.ToByteArray(appTitle, configState->Model->Release);   //--A kind of "versioning" I guess
            Converter.ToByteArray(iniFile, configState->IniFilePath);

            //--TODO: Silly way to get C# to look at the SoundCardList array correctly
            SoundCardList* soundCards = (SoundCardList*)(&configState->SoundCards);

            configState->NumberOfSoundCards = _modules.Audio.GetSoundCardList(soundCards);

            JoystickState* joystickState = _modules.Joystick.GetJoystickState();

            joystickState->Left = configState->Model->Left;
            joystickState->Right = configState->Model->Right;

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

        public unsafe string GetIniFilePath(string argIniFile)
        {
            fixed (byte* buffer = new byte[Define.MAX_PATH])
            {
                Library.Config.GetIniFilePath(buffer, argIniFile);

                return Converter.ToString(buffer);
            };
        }
    }
}
