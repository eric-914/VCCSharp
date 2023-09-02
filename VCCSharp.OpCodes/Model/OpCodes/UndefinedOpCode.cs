namespace VCCSharp.OpCodes.Model.OpCodes;

internal class UndefinedOpCode : IOpCode
{
    public UndefinedOpCode() { }

    public int Exec() => throw new NotImplementedException();
}
