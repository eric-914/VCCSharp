using VCCSharp.Configuration;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.IoC;

internal static class ConfigurationBinding
{
    public static void Bind(IBinder binder)
    {
        //--Let Configuration do its bindings...
        Configuration.ConfigurationBinding.Bind(binder);

        binder
            //--Define system services required by Configuration
            .Singleton<IConfigurationSystem, ConfigurationSystem>()

            //--Define binding mappings for left/right joystick view models
            .Bind<ILeft<JoystickStateViewModel>, LeftJoystickStateViewModel>()
            .Bind<IRight<JoystickStateViewModel>, RightJoystickStateViewModel>()

            .Bind<ILeft<JoystickSourceModel>, LeftJoystickSourceModel>()
            .Bind<IRight<JoystickSourceModel>, RightJoystickSourceModel>()

            .Bind<ILeft<KeyboardSourceViewModel>, LeftKeyboardSourceViewModel>()
            .Bind<IRight<KeyboardSourceViewModel>, RightKeyboardSourceViewModel>()

            .Bind<ILeft<JoystickSourceViewModel>, LeftJoystickSourceViewModel>()
            .Bind<IRight<JoystickSourceViewModel>, RightJoystickSourceViewModel>()

            .Bind<ILeft<JoystickConfigurationViewModel>, LeftJoystickConfigurationViewModel>()
            .Bind<IRight<JoystickConfigurationViewModel>, RightJoystickConfigurationViewModel>()
            ;
    }
}
