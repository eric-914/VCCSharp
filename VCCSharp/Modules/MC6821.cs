using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMC6821
    {
        unsafe MC6821State* GetMC6821State();
        void MC6821_PiaReset();
        void MC6821_irq_fs(PhaseStates phase);
        void MC6821_irq_hs(PhaseStates phase);
        void MC6821_SetCartAutoStart(byte autoStart);
    }

    public class MC6821 : IMC6821
    {
        private readonly IModules _modules;

        public MC6821(IModules modules)
        {
            _modules = modules;
        }

        public unsafe MC6821State* GetMC6821State()
        {
            return Library.MC6821.GetMC6821State();
        }

        public void MC6821_SetCartAutoStart(byte autoStart)
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                mc6821State->CartAutoStart = autoStart;
            }
        }

        public void MC6821_irq_hs(PhaseStates phase) //63.5 uS
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                switch (phase)
                {
                    case PhaseStates.Falling:	//HS went High to low
                        if ((mc6821State->rega[1] & 2) != 0)
                        { //IRQ on low to High transition
                            return;
                        }

                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;

                    case PhaseStates.Rising:	//HS went Low to High
                        if ((mc6821State->rega[1] & 2) == 0)
                        { //IRQ  High to low transition
                            return;
                        }

                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;

                    case PhaseStates.Any:
                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;
                }
            }
        }

        public void MC6821_irq_fs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                if (mc6821State->CartInserted == 1 && mc6821State->CartAutoStart == 1) {
                    MC6821_AssertCart();
                }

                switch (phase)
                {
                    case PhaseStates.Falling:	//FS went High to low
                        if ((mc6821State->rega[3] & 2) == 0) //IRQ on High to low transition
                        {
                            mc6821State->rega[3] = (byte)(mc6821State->rega[3] | 128);
                        }

                        break;

                    case PhaseStates.Rising:	//FS went Low to High
                        if ((mc6821State->rega[3] & 2) != 0) //IRQ  Low to High transition
                        {
                            mc6821State->rega[3] = (byte)(mc6821State->rega[3] | 128);
                        }

                        break;
                }

                if ((mc6821State->rega[3] & 1) != 0) {
                    CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                }
            }
        }

        public void MC6821_AssertCart()
        {
            Library.MC6821.MC6821_AssertCart();
        }

        public void CPUAssertInterrupt(CPUInterrupts irq, byte flag)
        {
            Library.MC6821.CPUAssertInterrupt((byte)irq, flag);
        }

        public void MC6821_PiaReset()
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                // Clear the PIA registers
                for (byte index = 0; index < 4; index++)
                {
                    mc6821State->rega[index] = 0;
                    mc6821State->regb[index] = 0;
                    mc6821State->rega_dd[index] = 0;
                    mc6821State->regb_dd[index] = 0;
                }
            }
        }
    }
}
