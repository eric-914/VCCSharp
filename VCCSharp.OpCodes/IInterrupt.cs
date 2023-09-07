namespace VCCSharp.Models.CPU.OpCodes;

public interface IInterrupt
{
    void EndInterrupt();
    int SyncWait();
}
