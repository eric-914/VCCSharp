// ReSharper disable InconsistentNaming
namespace VCCSharp.Modules.TCC1014;

public interface IVDG
{
    byte Mode { get; set; }
    byte DisplayOffset { get; set; }
    void Reset();
}

/// <summary>
/// MC6847 Video Display Generator (VDG)
/// </summary>
public class VDG : IVDG
{
    public byte Mode { get; set; }
    public byte DisplayOffset { get; set; }

    public void Reset()
    {
        Mode = 0;
        DisplayOffset = 0;
    }
}
