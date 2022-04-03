using VCCSharp.Shared;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace DX8.Tester.Model;

internal class Factory
{
    private static readonly SharedFactory Shared = new();

    public IDxManager CreateManager(IThreadRunner runner)
    {
        IDxInput dxInput = DxFactory.Instance.CreateDxInput();

        return Shared.CreateManager(dxInput, runner);
    }

    public IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        return Shared.CreateThreadRunner(dispatcher);
    }
}
