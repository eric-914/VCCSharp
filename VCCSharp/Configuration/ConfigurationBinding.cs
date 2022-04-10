using VCCSharp.IoC;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration;

internal static class ConfigurationBinding
{
    public static IBinder Initialize(IBinder binder)
    {
        binder
            .Bind(() => binder.Get<IConfiguration>().Audio)
            //.Bind(() => binder.Get<IModules>().Audio)
            ;

        return binder;
    }
}
