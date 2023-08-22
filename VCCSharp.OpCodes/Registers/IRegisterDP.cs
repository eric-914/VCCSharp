namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// DIRECT PAGE REGISTER (DP)
/// </summary>
/// <remarks>
/// The direct page register of the MC6809E serves to enhance the direct addressing mode. 
/// The content of this register appears at the higher address outputs (A8-A15) during direct addressing instruction execution. 
/// This allows the direct mode to be used at any place in memory, under program control. 
/// To ensure M6800 compatibility, all bits of this register are cleared during processor reset.
/// 
/// 16 bits -- DP in upper 8 bits; Zero in lower 8 bits
/// </remarks>
public interface IRegisterDP
{
    //TODO: DP_REG is always equal to DPA * 0x100;
    //ushort DP_REG { get; set; } //DIRECT PAGE REGISTER

    byte DP { get; set; }
}
