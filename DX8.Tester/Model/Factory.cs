using System.Windows;
using System.Windows.Threading;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace DX8.Tester.Model;

internal class Factory
{
    public static Factory Instance { get; }= new();

    public TestWindowViewModel CreateViewModel()
    {
        var model = CreateModel();

        return new TestWindowViewModel(model);
    }

    public TestWindowModel CreateModel()
    {
        var manager = CreateManager();

        return new TestWindowModel(manager);
    }

    public IDxManager CreateManager()
    {
        var dispatcher = CreateDispatcher();
        var runner = CreateThreadRunner(dispatcher);

        return CreateManager(runner);
    }

    public IDxManager CreateManager(IThreadRunner runner)
    {
        IDxInput dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner);
    }

    public IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        var runner = new ThreadRunner(dispatcher);
        
        Application.Current.Exit += (_, _) => runner.Stop();

        return runner;
    }

    public IDispatcher CreateDispatcher()
    {
        return new DispatcherWrapper(Application.Current.Dispatcher);
    }
}
