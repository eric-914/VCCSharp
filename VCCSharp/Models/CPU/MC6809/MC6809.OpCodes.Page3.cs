namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        public void SWI3_I() //113F 
        {
            LIB3(0x3F); 
        }

        public void CMPU_M() //1183 
        {
            LIB3(0x83); 
        }

        public void CMPS_M() //118C 
        {
            LIB3(0x8C); 
        }

        public void CMPU_D() //1193 
        {
            LIB3(0x93); 
        }

        public void CMPS_D() //119C 
        {
            LIB3(0x9C); 
        }

        public void CMPU_X() //11A3 
        {
            LIB3(0xA3); 
        }

        public void CMPS_X() //11AC 
        {
            LIB3(0xAC); 
        }

        public void CMPU_E() //11B3 
        {
            LIB3(0xB3); 
        }

        public void CMPS_E() //11BC 
        {
            LIB3(0xBC); 
        }
    }
}
