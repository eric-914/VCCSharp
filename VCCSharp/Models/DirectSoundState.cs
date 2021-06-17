using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DirectSoundState
    {
        // directsound description
        public DSBUFFERDESC dsbd;

        // directsound caps
        public DSCAPS dscaps;

        // directsound buffer caps
        public DSBCAPS dsbcaps;

        // directsound description
        public DSCBUFFERDESC dsbdin;

        //generic waveformat structure
        public WAVEFORMATEX pcmwf;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DSBUFFERDESC
    {

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DSCAPS
    {

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DSBCAPS
    {

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DSCBUFFERDESC
    {

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WAVEFORMATEX
    {

    }
}
