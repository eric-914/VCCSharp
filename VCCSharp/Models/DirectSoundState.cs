using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DirectSoundState
    {
        // directsound description
        public DSBUFFERDESC dsbd;

        //generic waveformat structure
        public WAVEFORMATEX pcmwf;
    }

    [StructLayout(LayoutKind.Explicit, Size = 40, CharSet = CharSet.Ansi)]
    public struct DSBUFFERDESC
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwFlags;

        [FieldOffset(8)]
        public uint dwBufferBytes;

        [FieldOffset(12)]
        public uint dwReserved;

        [FieldOffset(16)]
        public IntPtr lpwfxFormat; //--WAVEFORMATEX*
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WAVEFORMATEX
    {
        public ushort wFormatTag;         /* format type */
        public ushort nChannels;          /* number of channels (i.e. mono, stereo...) */
        public uint nSamplesPerSec;       /* sample rate */
        public uint nAvgBytesPerSec;      /* for buffer estimation */
        public ushort nBlockAlign;        /* block size of data */
        public ushort wBitsPerSample;     /* number of bits per sample of mono data */
        public ushort cbSize;             /* the count in bytes of the size of extra information (after cbSize) */
    }
}
