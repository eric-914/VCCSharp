namespace VCCSharp.Models
{
    public interface IProcessor
    {
        void Init();
        int Exec(int cycleFor);
    }
}
