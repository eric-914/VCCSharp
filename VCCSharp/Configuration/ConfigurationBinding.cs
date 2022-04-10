using VCCSharp.IoC;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration;

internal static class ConfigurationBinding
{
    public static IBinder Initialize(IBinder binder)
    {
        IConfiguration _() => binder.Get<IConfiguration>();

        binder
            .Bind(() => _().Audio)
            .Bind(() => _().CPU)
            .Bind(() => _().Memory)
            .Bind(() => _().Video)
            .Bind(() => _().Window)
            .Bind(() => _().Keyboard)
            ;

        return binder;
    }
}
