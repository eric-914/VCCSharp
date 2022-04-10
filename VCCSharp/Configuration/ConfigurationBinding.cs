using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Models;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration;

internal static class ConfigurationBinding
{
    public static IBinder Initialize(IBinder binder)
    {
        IConfiguration _() => binder.Get<IConfiguration>();

        //--Bind a configuration for each of the configuration tabs
        binder
            .Bind(() => _().Audio)
            .Bind(() => _().CPU)
            .Bind(() => _().Memory)
            .Bind(() => _().Video)
            .Bind(() => _().Window)
            .Bind(() => _().Keyboard)
            .Bind(() => _().Joysticks)
            .Bind(() => _().Startup)

            .Bind<ILeftJoystickStateViewModel, LeftJoystickStateViewModel>()
            .Bind<IRightJoystickStateViewModel, RightJoystickStateViewModel>()

            .Bind<ILeftJoystickSourceModel, LeftJoystickSourceModel>()
            .Bind<IRightJoystickSourceModel, RightJoystickSourceModel>()
            ;

        return binder;
    }
}
