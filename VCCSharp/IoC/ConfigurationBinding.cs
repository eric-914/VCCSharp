using VCCSharp.Configuration;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Persistence;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.IoC;

internal static class ConfigurationBinding
{
    public static void Bind(IBinder binder)
    {
        IConfiguration _() => binder.Get<IConfiguration>();

        binder
            .Singleton<IConfigurationPersistence, ConfigurationPersistence>()
            .Singleton<IConfiguration, RootConfiguration>()
            .Singleton<IConfigurationSystem, ConfigurationSystem>()
            .Singleton<IConfigurationPersistenceManager, ConfigurationPersistenceManager>()

            //--Bind a configuration for each of the configuration tabs
            .Bind(() => _().Audio)
            .Bind(() => _().CPU)
            .Bind(() => _().Memory)
            .Bind(() => _().Video)
            .Bind(() => _().Window)
            .Bind(() => _().Keyboard)
            .Bind(() => _().Joysticks)
            .Bind(() => _().Startup)

            //--Define binding mappings for left/right joystick configuration branches
            .Bind(() => (ILeft<IJoystickConfiguration>)_().Joysticks.Left)
            .Bind(() => (IRight<IJoystickConfiguration>)_().Joysticks.Right)
            .Bind(() => (ILeft<IJoystickKeyMappingConfiguration>)_().Joysticks.Left.KeyMap)
            .Bind(() => (IRight<IJoystickKeyMappingConfiguration>)_().Joysticks.Right.KeyMap)

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
