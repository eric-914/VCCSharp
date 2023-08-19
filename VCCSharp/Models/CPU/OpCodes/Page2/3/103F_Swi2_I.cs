using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// SWI2
    /// Software interrupt (absolute indirect) 
    /// Software Interrupt
    /// INHERENT
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SWI2            INHERENT            103F        20 / 22     2
    /// </summary>
    /// <remarks>
    /// The SWI, SWI2 and SWI3 instructions each invoke a Software Interrupt.
    /// 
    /// Each of these instructions first set the E flag in the CC register and then push the machine state onto the hardware stack (S).
    /// 
    /// After stacking the machine state, the SWI instruction sets the I and F interrupt masks in the CC register. 
    /// SWI2 and SWI3 do not modify the mask.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///     ╭─────────────────╮  ╭────────────────────────────╮
    ///     | SWI, SWI2, SWI3 |  │                            ▼
    ///     ╰────────┬────────╯  │                    ┌───────────────┐      
    ///              ▼           │                    │ PUSH B, A, CC │
    ///       ┌────────────┐     │                    └───────┬───────┘
    ///       | SET: E = 1 |     │                            ▼
    ///       └──────┬─────┘     │    ┌─────────────────────────────────────────────┐
    ///              ▼           │    │        SWI           SWI2            SWI3   │  
    ///      ┌─────────────────┐ │    └─────────┬─────────────┬───────────────┬─────┘
    ///      │      PUSH       │ │              ▼             │               ▼
    ///      │ PC, U, Y, X, DP │ │    ┌───────────────────┐   │     ┌───────────────┐
    ///      └───────┬─────────┘ │    │ SET: I = 1; F = 1 │   │     │ PC ← [FFF2:3] │
    ///   ╭┄┄┄┄┄┄┄┄┄┄▼┄┄┄┄┄┄┄┄┄┄┄│┄╮  └─────────┬─────────┘   ▼     └─────────┬─────┘
    ///   ┊      ╔════════╗      │ ┊            │     ┌───────────────┐       │     
    ///   ┊      ║ NM=1?  ╟──NO──╯ ┊         NO |     │ PC ← [FFF4:5] │       │
    ///   ┊      ╚═══╤════╝      ▲ ┊            ▼     └─────────┬─────┘       │
    ///   ┊      YES |           │ ┊   ┌──────────────────┐     │             │
    ///   ┊          │           │ ┊   | PC ← [FFFA:B]    |     │             │
    ///   ┊          ▼           │ ┊   └────────┬─────────┘     │             │
    ///   ┊     ┌─────────┐      │ ┊            ╰──────────────▶│◀────────────╯
    ///   ┊     | PUSH FE |──────╯ ┊                            ▼
    ///   ┊     └─────────┘        ┊                        ╭──────╮
    ///   ┊ 6309 Only              ┊                        │ DONE │
    ///   ╰┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄╯                        ╰──────╯
    ///     SWI Instruction Flow
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// Finally, control is transferred to the interrupt service routine whose address is obtained from the vector which corresponds to the particular instruction.
    /// 
    /// The state of the NM bit in the MD register determines whether or not the E and F accumulators are included in the stacked machine state. 
    /// Service routines should be written to work properly regardless of the current state of the NM bit. 
    /// This is best accomplished by avoiding modification of the NM bit and using the RTI instruction to return control to the interrupted task. 
    /// If an SWI service routine needs to examine or modify the stacked machine state, it may first need to determine the current state of the NM bit. 
    /// See page 144 for the listing of a subroutine that will accomplish this task. 
    /// 
    /// NOTE: When Motorola introduced the 6809, they designated SWI2 as an instruction reserved for the end user, and not to be used in packaged software. 
    /// Under the OS9 operating system, SWI2 is used to invoke Service Requests.
    /// 
    /// See Also: RTI
    /// </remarks>
    public class _103F_Swi2_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles, Func<int> addendum)
        {
            cpu.CC_E = true;

            cpu.MemWrite8(cpu.PC_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.PC_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.DPA, --cpu.S_REG);

            cycles += addendum();

            cpu.MemWrite8(cpu.B_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.A_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.CC, --cpu.S_REG);

            cpu.PC_REG = cpu.MemRead16(Define.VSWI2);

            return cycles;
        }

        public int Exec(IMC6809 cpu)
        {
            return Exec(cpu, 20, () => 0);
        }

        public int Exec(IHD6309 cpu)
        {
            int Addendum()
            {
                if (cpu.MD_NATIVE6309)
                {
                    cpu.MemWrite8(cpu.F_REG, --cpu.S_REG);
                    cpu.MemWrite8(cpu.E_REG, --cpu.S_REG);

                    return 2;
                }

                return 0;
            }

            return Exec(cpu, 20, Addendum);
        }
    }
}
