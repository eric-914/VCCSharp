namespace VCCSharp.Models
{
    public struct TC1014MmuState
    {
        public byte MmuState;	// Composite variable handles MmuTask and MmuEnabled
        public unsafe fixed ushort MemPageOffsets[1024];
        public unsafe byte* Memory;	//Emulated RAM
        public unsafe byte* InternalRomBuffer;

        public byte MmuTask;		// $FF91 bit 0
        public byte MmuEnabled;	// $FF90 bit 6
        public byte RamVectors;	// $FF90 bit 3

        public byte RomMap;		  // $FF90 bit 1-0
        public byte MapType;		// $FFDE/FFDF toggle Map type 0 = ram/rom

        public byte CurrentRamConfig;
        public ushort MmuPrefix;

        public unsafe fixed uint MemConfig[4];
        public unsafe fixed ushort RamMask[4];
        public unsafe fixed byte StateSwitch[4];
        public unsafe fixed byte VectorMask[4];
        public unsafe fixed byte VectorMaska[4];
        public unsafe fixed uint VidMask[4];

        //--TODO: This is really byte* MemPages[1024]
        //public unsafe byte** MemPages; //[1024];
        public unsafe fixed long MemPages[1024];

        //--TODO: This is really ushort MmuRegisters[4][8]
        public unsafe fixed ushort MmuRegisters[32];	//[4][8] // $FFA0 - FFAF
    }
}
