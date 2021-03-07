using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IMC6821
    {
        void MC6821_PiaReset();
    }

    public class MC6821 : IMC6821
    {
        public void MC6821_PiaReset()
        {
            Library.MC6821.MC6821_PiaReset();
        }
    }
}
