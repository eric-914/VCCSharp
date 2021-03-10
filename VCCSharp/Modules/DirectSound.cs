using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IDirectSound
    {
        void StopAndRelease();
        void SetCurrentPosition(ulong position);

        unsafe int DirectSoundLock(ulong buffOffset, ushort length,
            void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

        unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);
    }

    public class DirectSound : IDirectSound
    {
        public void StopAndRelease()
        {
            Library.DirectSound.DirectSoundStopAndRelease();
        }

        public void SetCurrentPosition(ulong position)
        {
            Library.DirectSound.DirectSoundSetCurrentPosition(position);
        }

        public unsafe int DirectSoundLock(ulong buffOffset, ushort length,
            void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2)
        {
            return Library.DirectSound.DirectSoundLock(buffOffset, length, 
                sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2)
        {
            return Library.DirectSound.DirectSoundUnlock(sndPointer1, sndLength1, sndPointer2, sndLength2);
        }
    }
}
