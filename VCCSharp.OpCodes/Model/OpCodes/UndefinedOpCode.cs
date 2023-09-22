namespace VCCSharp.OpCodes.Model.OpCodes;

internal class UndefinedOpCode : IOpCode
{
    public UndefinedOpCode() { }

    public int CycleCount => throw new NotImplementedException();

    public int Cycles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Exec() => throw new NotImplementedException();
}
