using System.Windows;
using DX8;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace VCCSharp.DX8;

internal static class DxBinding
{
    public static IBinder Initialize(IBinder binder)
    {
        binder
            .Bind<IDxDraw, Dx>()
            .Bind<IDxSound, Dx>()
            .Bind<IDxInput, Dx>()

            .Bind<IDispatcher>(() => new DispatcherWrapper(Application.Current.Dispatcher))
            .Bind<IThreadRunner, ThreadRunner>()
            .Singleton<IDxManager, DxManager>()
            ;

        return binder;
    }
}