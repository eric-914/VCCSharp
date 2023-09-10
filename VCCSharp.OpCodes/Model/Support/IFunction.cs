namespace VCCSharp.OpCodes.Model.Support;

internal interface IFunction : IFlags
{
    int Result { get; }
    int Remainder { get; }
}