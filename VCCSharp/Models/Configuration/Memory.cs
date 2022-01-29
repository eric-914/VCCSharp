namespace VCCSharp.Models.Configuration
{
    public class Memory
    {
        public byte RamSize { get; set; } = 1;
        public string ExternalBasicImage { get; private set; } = ""; //## READ-ONLY ##//

        public void SetExternalBasicImage(string value) => ExternalBasicImage = value;
    }
}