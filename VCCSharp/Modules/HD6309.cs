using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IHD6309 : IProcessor
    {

    }

    public class HD6309 : IHD6309
    {
        public void Init()
        {
            Library.HD6309.HD6309Init();
        }

        public int Exec(int cycleFor)
        {
            return Library.HD6309.HD6309Exec(cycleFor);
        }

        public void ForcePC(ushort xferAddress)
        {
            Library.HD6309.HD6309ForcePC(xferAddress);
        }

        public void Reset()
        {
            Library.HD6309.HD6309Reset();
        }
    }
}
