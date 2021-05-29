using VCCSharp.Modules;

namespace VCCSharp.BitBanger
{
    public interface IBitBanger
    {
        void ShowDialog(IConfig state);

        void Open();
        void Close();
    }
}
