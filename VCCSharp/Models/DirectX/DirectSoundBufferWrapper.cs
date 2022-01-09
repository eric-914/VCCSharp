using System;

namespace VCCSharp.Models.DirectX
{
    public interface IDirectSoundBufferWrapper : IDirectSoundBuffer
    {
        public void SetInstance(IDirectSoundBuffer instance);
        public bool Mute { get; set; }
    }

    /// <summary>
    /// Seems there's some issue with the COM pointer getting corrupted if you switch from sound/mute.
    /// Using this to isolate.
    /// </summary>
    public class DirectSoundBufferWrapper : IDirectSoundBufferWrapper
    {
        private IDirectSoundBuffer _instance;

        public bool Mute { get; set; }

        public void SetInstance(IDirectSoundBuffer instance) => _instance = instance;

        public unsafe long GetCurrentPosition(ulong* playCursor, ulong* writeCursor)
            => !Mute ? _instance.GetCurrentPosition(playCursor, writeCursor) : 0;

        public unsafe long Lock(uint buffOffset, ushort length, IntPtr* sndPointer1, uint* sndLength1, IntPtr* sndPointer2, uint* sndLength2, uint dwFlags)
            => _instance.Lock(buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2, dwFlags);

        public long Play(uint dwReserved1, uint dwPriority, uint dwFlags)
            => !Mute ? _instance.Play(dwReserved1, dwPriority, dwFlags) : 0;

        public long SetCurrentPosition(uint position)
            => !Mute ? _instance.SetCurrentPosition(position) : 0;

        public long Stop()
            => _instance.Stop();

        public long Unlock(IntPtr sndPointer1, uint sndLength1, IntPtr sndPointer2, uint sndLength2)
            => _instance.Unlock(sndPointer1, sndLength1, sndPointer2, sndLength2);

        #region Not Implemented

        public long GetCaps() => throw new NotImplementedException();
        public long GetFormat() => throw new NotImplementedException();
        public long GetVolume() => throw new NotImplementedException();
        public long GetPan() => throw new NotImplementedException();
        public long GetFrequency() => throw new NotImplementedException();
        public long GetStatus() => throw new NotImplementedException();
        public long Initialize() => throw new NotImplementedException();
        public long SetFormat() => throw new NotImplementedException();
        public long SetVolume() => throw new NotImplementedException();
        public long SetPan() => throw new NotImplementedException();
        public long SetFrequency() => throw new NotImplementedException();
        public long Restore() => throw new NotImplementedException();

        #endregion
    }
}
