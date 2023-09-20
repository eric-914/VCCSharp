namespace VCCSharp.OpCodes.Model.OpCodes;

internal class UndefinedOpCode : IOpCode
{
    public UndefinedOpCode() { }

    public int CycleCount => throw new NotImplementedException();

    public int Exec() => throw new NotImplementedException();
}
