namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// STACK POINTER (S)
/// </summary>
/// <remarks>
/// The hardware stack pointer (S) is used automatically by the processor during subroutine calls and interrupts. 
/// The user stack pointer (U) is controlled exclusively by the programmer. 
/// This allows arguments to be passed to and from subroutines with ease. 
/// The U register is frequently used as a stack marker. 
/// Both stack pointers have the same indexed mode addressing capabilities as the X and Y registers, but also support Push and Pull instructions. 
/// This allows the MC6809E to be used efficiently as a stack processor, greatly enhancing its ability to support higher level languages and modular programming.
/// </remarks>
public interface IRegisterS
{
    ushort S_REG { get;set;}

    byte S_L { get; set; }
    byte S_H { get; set; }
}
