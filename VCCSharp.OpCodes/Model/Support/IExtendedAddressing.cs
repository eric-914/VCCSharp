namespace VCCSharp.OpCodes.Model.Support;

public interface IExtendedAddressing
{
    ushort CalculateEA(byte postByte);
}
