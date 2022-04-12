using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.IoC;

internal static class CpuBinding
{
    public static void Bind(IBinder binder)
    {
        binder
            .Singleton<IHD6309, HD6309>()
            .Singleton<IMC6809, MC6809>()
            ;
    }
}