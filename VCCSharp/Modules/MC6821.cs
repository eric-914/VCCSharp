using VCCSharp.Enums;
using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IMC6821
    {
        void MC6821_PiaReset();
        void MC6821_irq_fs(PhaseStates phase);
        void MC6821_irq_hs(PhaseStates phase);
        byte MC6821_SetCartAutoStart(byte autostart);
    }

    public class MC6821 : IMC6821
    {
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

        public byte MC6821_SetCartAutoStart(byte autostart)
        {
            return Library.MC6821.MC6821_SetCartAutoStart(autostart);
        }
    }
}
