using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IDirectSound
    {
        void StopAndRelease();
        void SetCurrentPosition(ulong position);

        unsafe int DirectSoundLock(ulong buffOffset, ushort length,
            void** sndPointer1, ulong* sndLength1, void** sndPointer2, ulong* sndLength2);
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
            void** sndPointer1, ulong* sndLength1, void** sndPointer2, ulong* sndLength2)
        {
            return Library.DirectSound.DirectSoundLock(buffOffset, length, 
                sndPointer1, sndLength1, sndPointer2, sndLength2);
        }
    }
}
