using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMC6809 : IProcessor
    {

    }

    public class MC6809 : IMC6809
    {
        public void Init()
        {
            Library.MC6809.MC6809Init();
        }

        public int Exec(int cycleFor)
        {
            return Library.MC6809.MC6809Exec(cycleFor);
        }
    }
}
