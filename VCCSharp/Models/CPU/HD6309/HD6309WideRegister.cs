using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.HD6309;

[StructLayout(LayoutKind.Explicit)]
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
public class HD6309WideRegister
{
    [FieldOffset(0)]
    public uint Reg;

    //--------------------------------------

    [FieldOffset(0)]
    public ushort msw;

    [FieldOffset(2)]
    public ushort lsw;

    //--------------------------------------

    [FieldOffset(0)]
    public byte mswlsb;

    [FieldOffset(1)]
    public byte mswmsb;

    [FieldOffset(2)]
    public byte lswlsb;

    [FieldOffset(3)]
    public byte lswmsb;	//Might be backwards
}
// ReSharper restore IdentifierTypo
// ReSharper restore InconsistentNaming
