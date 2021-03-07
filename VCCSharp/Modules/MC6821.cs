using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IMC6821
    {
        void MC6821_PiaReset();
        void MC6821_irq_fs(int phase);
    }

    public class MC6821 : IMC6821
    {
        public void MC6821_PiaReset()
        {
            Library.MC6821.MC6821_PiaReset();
        }

        public void MC6821_irq_fs(int phase) //60HZ Vertical sync pulse 16.667 mS
        {
            Library.MC6821.MC6821_irq_fs(phase);
        }
    }
}
