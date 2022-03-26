namespace VCCSharp.Models.Keyboard;

public class KeyTranslationEntry
{
    public byte ScanCode1 { get; set; }
    public byte ScanCode2 { get; set; }
    public byte Row1 { get; set; }
    public byte Col1 { get; set; }
    public byte Row2 { get; set; }
    public byte Col2 { get; set; }
}
