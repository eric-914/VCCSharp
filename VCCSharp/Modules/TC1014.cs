using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ITC1014
    {
        void MC6883Reset();
        void CopyRom();
        void MmuReset();
    }

    public class TC1014 : ITC1014
    {
        public void MC6883Reset()
        {
            Library.TC1014.MC6883Reset();
        }

        public void CopyRom()
        {
            Library.TC1014.CopyRom();
        }

        public void MmuReset()
        {
            Library.TC1014.MmuReset();
        }
    }
}
