namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// Zero register
/// </summary>
/// <remarks>
/// The 0 register is always zero, independant of reads/writes to it.
/// It enables a zero value to be used in inter-register operations without accessing memory, or changing the value of another register. 
/// If a 0 byte is stored at address <c>$0000</c>, it may also be used to clear large amounts of memory quickly via <code>TFM 0,r+</code>
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
public interface IRegisterZ
{
    ushort Z { get => 0; set { } }
}