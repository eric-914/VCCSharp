using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;

namespace VCCSharp.OpCodes;

public static class OpCodesFactory
{
    public static IOpCodes Create(MC6809.IState cpu)
    {
        var instance = new OpCodes(new Page1OpCodes6809().OpCodes);

        IEnumerable<IOpCode> all = All(instance.Page1);

        SetState6809(cpu, all);

        return instance;
    }

    public static IOpCodes Create(HD6309.IState cpu)
    {
        var instance = new OpCodes(new Page1OpCodes6809().OpCodes);

        IEnumerable<IOpCode> all = All(instance.Page1);

        SetState6809(cpu, all);
        SetState6309(cpu, all);

        return instance;
    }

    private static IEnumerable<IOpCode> All(IOpCode[] page1)
    {
        var page2 = (IPage2)page1[0x10];
        var page3 = (IPage3)page1[0x11];

        return page1.Union(page2.Page2).Union(page3.Page3);
    }

    private static void SetState6809(MC6809.IState cpu, IEnumerable<IOpCode> opcodes)
    {
        var ss = new MC6809.SystemState(cpu);

        var match = opcodes.Where(x => x is OpCode).Cast<OpCode>();

        foreach (var opcode in match)
        {
            opcode.System = ss;
        }
    }

    private static void SetState6309(HD6309.IState cpu, IEnumerable<IOpCode> opcodes)
    {
        var ss = new HD6309.SystemState(cpu);

        var match = opcodes.Where(x => x is OpCode6309).Cast<OpCode6309>();

        foreach (var opcode in match)
        {
            opcode.System = ss;
        }
    }
}
