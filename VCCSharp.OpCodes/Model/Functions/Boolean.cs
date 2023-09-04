using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

/// <summary>
/// Used to calculate CC codes during register transfer logic
/// </summary>
internal class Boolean : IFunction
{
    public int Result { get; }

    public bool N { get; }
    public bool Z { get; }
    public bool V { get; } = false;

    public bool E => throw new NotImplementedException();
    public bool F => throw new NotImplementedException();
    public bool H => throw new NotImplementedException();
    public bool I => throw new NotImplementedException();
    public bool C => throw new NotImplementedException();

    public Boolean(byte result)
    {
        Result = result;
        N = result.Bit7();
        Z = result == 0;
    }

    public Boolean(ushort result)
    {
        Result = result;
        N = result.Bit15();
        Z = result == 0;
    }
}
