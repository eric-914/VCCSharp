using System.Windows;
using System.Windows.Threading;

namespace DX8.Tester.Model;

internal class Factory
{
    public DxManager CreateManager(IThreadRunner runner)
    {
        var dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner);
    }

    public IThreadRunner CreateThreadRunner(Dispatcher dispatcher)
    {
        var runner = new ThreadRunner(dispatcher);
        

        return runner;
    }
}