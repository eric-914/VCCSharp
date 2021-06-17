using System.Runtime.InteropServices;

namespace VCCSharp.Models.DirectX
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectSound)]
    public interface IDirectSound
    {
        long CreateSoundBuffer();
        long GetCaps();
        long DuplicateSoundBuffer();
        long SetCooperativeLevel();
        long Compact();
        long GetSpeakerConfig();
        long SetSpeakerConfig();
        long Initialize();
    }
}
