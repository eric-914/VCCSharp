using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>2F/BLE/RELATIVE</code>
/// Branch If Less than or Equal to Zero (signed)
/// <code>IF (CC.N ≠ CC.V) OR (CC.Z = 1) then PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// This instruction performs a relative branch if the value of the Zero (Z) flag is 1, OR if the values of the Negative (N) and Overflow (V) flags are not equal. 
/// If the N and V flags have the same value and the Z flag is not set then the CPU continues executing the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected by this instruction.
/// 
/// When used following a subtract or compare of signed (twos-complement) values, the BLE instruction will branch if the source value was less than or equal to the original destination value.
/// 
/// The branch address is calculated by adding the current value of the PC register (after the BLE instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
/// The range of the branch destination is limited to -126 to +129 bytes from the address of the BLE instruction. If a larger range is required then the LBLE instruction may be used instead.
/// 
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BGT, BLS, LBLE
internal class _2F_Ble_R : OpCode, IOpCode
{
    public int Exec()
    {
        if (CC_Z | (CC_N ^ CC_V))
        {
            PC += (ushort)(sbyte)M8[PC];
        }

        PC++;

        return 3;
    }
}
