using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.OpCodes.Model.Support;

/// <summary>
/// For Division functions which result in more processing details than just the result.
/// </summary>
internal interface IFunctionDiv : IFunction
{
    int Remainder { get; }
    int Cycles { get; }
    DivisionErrors Error { get; }
}