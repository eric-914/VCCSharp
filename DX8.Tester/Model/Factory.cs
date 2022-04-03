using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace DX8.Tester.Model;

internal class Factory
{
    public IDxManager CreateManager(IThreadRunner runner)
    {
        IDxInput dxInput = DxFactory.Instance.CreateDxInput();

        return new DxManager(dxInput, runner);
    }

    public IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        return new ThreadRunner(dispatcher);
    }
}
