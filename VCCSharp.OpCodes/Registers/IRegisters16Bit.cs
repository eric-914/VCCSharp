namespace VCCSharp.OpCodes.Registers;

internal interface IRegisters16Bit
{
    ushort this[int index] { get; set; }
}
