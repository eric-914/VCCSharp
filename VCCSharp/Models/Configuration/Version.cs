namespace VCCSharp.Models.Configuration;

public class Version
{
    private const string AppTitleDefault = "VCCSharp 1.0 Tandy Color Computer 1/2/3 Emulator";

    //## WRITE-ONLY ##// 
    //TODO: Bind this text to actual version info
    public string Release { private get; set; } = AppTitleDefault;

    public string GetRelease() => Release;
}
