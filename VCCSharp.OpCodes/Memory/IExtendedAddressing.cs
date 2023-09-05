namespace VCCSharp.OpCodes.Memory;

public interface IExtendedAddressing
{
    ushort CalculateEA(byte postByte);
}
