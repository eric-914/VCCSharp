using System.Windows.Threading;
using VCCSharp.Shared.Threading;

namespace DX8.Tester.Model;

internal class DispatcherWrapper : IDispatcher
{
    private readonly Dispatcher _source;

    public DispatcherWrapper(Dispatcher source)
    {
        _source = source;
    }
    public void Invoke(Action callback)
    {
        _source.Invoke(callback);
    }
}