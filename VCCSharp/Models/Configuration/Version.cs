namespace VCCSharp.Models.Configuration
{
    public class Version
    {
        //## WRITE-ONLY ##// 
        //TODO: Bind this text to actual version info
        public string Release { private get; set; } = "VCCSharp 1.0 Tandy Color Computer 1/2/3 Emulator";

        public string GetRelease() => Release;
    }
}
