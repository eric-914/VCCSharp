namespace VCCSharp.Models.CPU.OpCodes;

public interface IInterrupt
{
    bool IsInInterrupt { get; set; }
    int SyncWait();
}
