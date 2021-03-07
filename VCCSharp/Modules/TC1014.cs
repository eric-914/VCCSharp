using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ITC1014
    {
        void MC6883Reset();
        void CopyRom();
        void MmuReset();
        byte MmuInit(byte ramSizeOption);
        void MemWrite8(byte data, ushort address);
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

        public byte MmuInit(byte ramSizeOption)
        {
            return Library.TC1014.MmuInit(ramSizeOption);
        }

        public void MemWrite8(byte data, ushort address)
        {
            Library.TC1014.MemWrite8(data, address);
        }
    }
}
