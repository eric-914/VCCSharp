namespace VCCSharp.OpCodes.Model.Support;

/// <summary>
/// Handles the details of executing an opcode.
/// Called by the main (Page1) execution thread, as well as Page2/Page3 execution.
/// </summary>
internal class Executor
{
    public int Exec(IOpCode vector)
    {
        vector.Cycles = vector.CycleCount;

        vector.Exec();

        return vector.Cycles;
    }
}
