using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectSound)]
    public interface IDirectSound
    {
        unsafe long CreateSoundBuffer(DSBUFFERDESC* pBufferDescription, ref IntPtr pInstance, IntPtr pUnknown);
        long GetCaps();
        long DuplicateSoundBuffer();
        long SetCooperativeLevel(IntPtr hwnd, uint dwLevel);
        long Compact();
        long GetSpeakerConfig();
        long SetSpeakerConfig();
        long Initialize();
    }
}
