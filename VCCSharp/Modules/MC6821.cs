using VCCSharp.Enums;
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

        public void MC6821_PiaReset()
        {
            Library.MC6821.MC6821_PiaReset();
        }

        public void MC6821_irq_fs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
        {
            Library.MC6821.MC6821_irq_fs((int)phase);
        }

        public void MC6821_irq_hs(PhaseStates phase)
        {
            Library.MC6821.MC6821_irq_hs((int)phase);
        }
    }
}
