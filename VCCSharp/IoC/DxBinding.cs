using DX8;
using System.Windows;
using VCCSharp.DX8;
using VCCSharp.Models;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace VCCSharp.IoC;

internal static class DxBinding
{
    public static void Bind(IBinder binder)
    {
        binder
            .Bind<IDxDraw, Dx>()
            .Bind<IDxSound, Dx>()
            .Bind<IDxInput, Dx>()

            .Bind<IDispatcher>(() => new DispatcherWrapper(Application.Current.Dispatcher))
            .Bind<IThreadRunner, ThreadRunner>()
            .Singleton<IDxManager, DxManager>()
            ;
    }

    public static void Initialize(IFactory factory)
    {
        factory.Get<IDxManager>()
            .Initialize()
            .EnumerateDevices();
    }
}
