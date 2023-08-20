using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Persistence;
using VCCSharp.Configuration.Support;
using VCCSharp.IoC;

namespace VCCSharp.Configuration
{
    public static class ConfigurationBinding
    {
        public static void Bind(IBinder binder)
        {
            //--This needs to remain as a (to-be-called) function
            IConfiguration _() => binder.Get<IConfiguration>();

            binder
                .Singleton<IConfiguration, RootConfiguration>()
                .Singleton<IConfigurationValidator, ConfigurationValidator>()
                .Singleton<IConfigurationPersistence, ConfigurationPersistence>()
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
                ;
        }
    }
}