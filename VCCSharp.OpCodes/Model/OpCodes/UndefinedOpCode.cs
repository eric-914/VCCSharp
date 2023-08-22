namespace VCCSharp.OpCodes.Model.OpCodes
{
    internal class UndefinedOpCode : OpCode, IOpCode
    {
        public UndefinedOpCode() : base(null!) { }

        public int Exec() => throw new NotImplementedException();
    }
}
