using System;
using System.Runtime.InteropServices;

namespace VCCSharp.DX8.Interfaces
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(DxGuid.DirectSoundBuffer)]
    public interface IDirectSoundBuffer
    {
        public long GetCaps();
        public unsafe long GetCurrentPosition(ulong* playCursor, ulong* writeCursor);
        public long GetFormat();
        public long GetVolume();
        public long GetPan();
        public long GetFrequency();
        public long GetStatus();
        public long Initialize();
        public unsafe long Lock(uint buffOffset, ushort length, IntPtr* sndPointer1, uint* sndLength1, IntPtr* sndPointer2, uint* sndLength2, uint dwFlags);
        public long Play(uint dwReserved1, uint dwPriority, uint dwFlags);
        public long SetCurrentPosition(uint position);
        public long SetFormat();
        public long SetVolume();
        public long SetPan();
        public long SetFrequency();
        public long Stop();
        public long Unlock(IntPtr sndPointer1, uint sndLength1, IntPtr sndPointer2, uint sndLength2);
        public long Restore();
    }
}
