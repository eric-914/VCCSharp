using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Modules;
using VCCSharp.Shared.Configuration;
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
            .Singleton<IConfiguration, ConfigurationRoot>()
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
            .Bind(() => (ILeftJoystickConfiguration)_().Joysticks.Left)
            .Bind(() => (IRightJoystickConfiguration)_().Joysticks.Right)
            .Bind(() => (ILeftJoystickKeyMapping)_().Joysticks.Left.KeyMap)
            .Bind(() => (IRightJoystickKeyMapping)_().Joysticks.Right.KeyMap)

            //--Define binding mappings for left/right joystick view models
            .Bind<ILeftJoystickStateViewModel, LeftJoystickStateViewModel>()
            .Bind<IRightJoystickStateViewModel, RightJoystickStateViewModel>()

            .Bind<ILeftJoystickSourceModel, LeftJoystickSourceModel>()
            .Bind<IRightJoystickSourceModel, RightJoystickSourceModel>()

            .Bind<ILeftKeyboardSourceViewModel, LeftKeyboardSourceViewModel>()
            .Bind<IRightKeyboardSourceViewModel, RightKeyboardSourceViewModel>()

            .Bind<ILeftJoystickSourceViewModel, LeftJoystickSourceViewModel>()
            .Bind<IRightJoystickSourceViewModel, RightJoystickSourceViewModel>()

            .Bind<ILeftJoystickConfigurationViewModel, LeftJoystickConfigurationViewModel>()
            .Bind<IRightJoystickConfigurationViewModel, RightJoystickConfigurationViewModel>()
            ;
    }
}
