using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AudioState
    {
        public byte AuxBufferPointer;

        public uint SndLength1;
        public uint SndLength2;
        public uint SndBuffLength;
        public uint BuffOffset;
        
        public unsafe void* SndPointer1;
        public unsafe void* SndPointer2;

        public unsafe ushort** AuxBuffer; //[6][44100 / 60]; //Biggest block size possible
    }
}
