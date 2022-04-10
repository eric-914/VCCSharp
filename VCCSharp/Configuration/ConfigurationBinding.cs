using VCCSharp.IoC;

namespace VCCSharp.Configuration;

internal static class ConfigurationBinding
{
    public static IBinder Initialize(IBinder binder)
    {
        return binder;
    }
}
