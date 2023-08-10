namespace VCCSharp.Models.CPU.OpCodes
{
    public interface IInterrupt
    {
        bool IsInInterrupt { get; set; }
        bool IsSyncWaiting { get; set; }

        int SyncCycle { get; set; }
    }
}
