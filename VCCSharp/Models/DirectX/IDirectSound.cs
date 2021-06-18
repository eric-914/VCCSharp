using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models.DirectX
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectSound)]
    public interface IDirectSound
    {
        unsafe long CreateSoundBuffer(DSBUFFERDESC* dsbd, IntPtr* ppDSBuffer, IntPtr pUnkOuter);
        long GetCaps();
        long DuplicateSoundBuffer();
        long SetCooperativeLevel(IntPtr hwnd, uint dwLevel);
        long Compact();
        long GetSpeakerConfig();
        long SetSpeakerConfig();
        long Initialize();
    }
}
