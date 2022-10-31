namespace VCCSharp.Modules.TCC1014;

/// <summary>
/// MC6847 Video Display Generator (VDG)
/// </summary>
// ReSharper disable InconsistentNaming
public class VDG
{
    public byte Mode { get; set; }
    public byte DisplayOffset { get; set; }

    public void Reset()
    {
        Mode = 0;
        DisplayOffset = 0;
    }
}
