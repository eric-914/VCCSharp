using DX8;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;

namespace VCCSharp.Shared;

public class SharedFactory
{
    public IThreadRunner CreateThreadRunner(IDispatcher dispatcher)
    {
        return new ThreadRunner(dispatcher);
    }

    public IDxManager CreateManager(IDxInput dxInput, IThreadRunner runner)
    {
        return new DxManager(dxInput, runner);
    }
}
