using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AudioState
    {
        public byte InitPassed;
        public byte AudioPause;
        public byte AuxBufferPointer;

        public ushort CurrentRate;
        public ushort BitRate;
        public ushort BlockSize;

        public short CardCount;

        public uint SndLength1;
        public uint SndLength2;
        public uint SndBuffLength;
        public uint WritePointer;
        public uint BuffOffset;

        public unsafe fixed ushort iRateList[4];
        
        public int hr;

        public unsafe void* SndPointer1;
        public unsafe void* SndPointer2;

        public unsafe SoundCardList* Cards;

        public unsafe ushort** AuxBuffer; //[6][44100 / 60]; //Biggest block size possible
    }
}
