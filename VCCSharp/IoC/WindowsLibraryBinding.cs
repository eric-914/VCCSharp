using VCCSharp.Libraries;

namespace VCCSharp.IoC;

internal static class WindowsLibraryBinding
{
    public static void Bind(IBinder binder)
    {
        binder
            .Bind<IKernel, Kernel>()
            .Bind<IUser32, User32>()
            .Bind<IGdi32, Gdi32>()
            .Bind<IWinmm, Winmm>()
            ;
    }
}
