using VCCSharp.Models;

namespace VCCSharp.BitBanger
{
    public interface IBitBanger
    {
        unsafe void ShowDialog(ConfigState* state);

        void Open();
        void Close();
    }
}
