namespace VCCSharp.OpCodes.Model.Support;

/// <summary>
/// Used by functions that will modify the CC flags
/// </summary>
internal interface IFlags
{
    bool E { get; } //7
    bool F { get; } //6
    bool H { get; } //5
    bool I { get; } //4
    bool N { get; } //3
    bool Z { get; } //2
    bool V { get; } //1
    bool C { get; } //0
}
