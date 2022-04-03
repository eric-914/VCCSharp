using VCCSharp.Shared;

namespace DX8.Tester.Model;

internal class Factory
{
    public DxManager CreateManager(IThreadRunner runner)
    {
        var dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner);
    }

    public IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        return new ThreadRunner(dispatcher);
    }
}
