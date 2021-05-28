namespace VCCSharp.Models
{
    public struct TC1014State
    {
        public byte MmuState;	// Composite variable handles MmuTask and MmuEnabled
        public unsafe fixed ushort MemPageOffsets[1024];

        public byte MapType;	// $FFDE/FFDF toggle Map type 0 = ram/rom

        public byte CurrentRamConfig;

        public unsafe fixed byte VectorMask[4];
        public unsafe fixed byte VectorMaska[4];

        //--TODO: This is really byte* MemPages[1024]
        //public unsafe byte** MemPages; //[1024];
        public unsafe fixed long MemPages[1024];

        //--TODO: This is really ushort MmuRegisters[4][8]
        public unsafe fixed ushort MmuRegisters[32];	//[4][8] // $FFA0 - FFAF
    }
}
