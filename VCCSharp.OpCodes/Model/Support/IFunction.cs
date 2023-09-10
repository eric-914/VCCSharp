using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.OpCodes.Model.Support;

internal interface IFunction : IFlags
{
    int Result { get; }
    int Remainder { get; }
    int Cycles { get; }
    DivisionErrors Error { get; }
}