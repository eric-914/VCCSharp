using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.MC6809;

public interface IMemory : IMemory8bit, IMemory16bit, IAddressDP
{
}
