using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectSound
    {
        int DirectSoundHasBuffer();
        int DirectSoundHasInterface();

        int DirectSoundBufferRelease();
        int DirectSoundCreateSoundBuffer();
        unsafe int DirectSoundInitialize(_GUID* guid);
        int DirectSoundInterfaceRelease();
        unsafe int DirectSoundLock(ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);
        unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);
        int DirectSoundSetCooperativeLevel(HWND hWnd);

        void DirectSoundEnumerateSoundCards();
        void DirectSoundSetCurrentPosition(ulong position);
        void DirectSoundSetupFormatDataStructure(ushort bitRate);
        void DirectSoundSetupSecondaryBuffer(uint sndBuffLength);
        void DirectSoundStopAndRelease();

        int DirectSoundPlay();
        int DirectSoundStop();

        unsafe long DirectSoundGetCurrentPosition(ulong* playCursor, ulong* writeCursor);
    }

    public class DirectSound : IDirectSound
    {
        public int DirectSoundHasBuffer()
        {
            return Library.DirectSound.DirectSoundHasBuffer();
        }

        public int DirectSoundHasInterface()
        {
            return Library.DirectSound.DirectSoundHasInterface();
        }

        public int DirectSoundBufferRelease()
        {
            return Library.DirectSound.DirectSoundBufferRelease();
        }

        public unsafe int DirectSoundInitialize(_GUID* guid)
        {
            return Library.DirectSound.DirectSoundInitialize(guid);
        }

        public int DirectSoundInterfaceRelease()
        {
            return Library.DirectSound.DirectSoundInterfaceRelease();
        }

        public int DirectSoundCreateSoundBuffer()
        {
            return Library.DirectSound.DirectSoundCreateSoundBuffer();
        }

        public unsafe int DirectSoundLock(ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2)
        {
            return Library.DirectSound.DirectSoundLock(buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2)
        {
            return Library.DirectSound.DirectSoundUnlock(sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public int DirectSoundSetCooperativeLevel(HWND hWnd)
        {
            return Library.DirectSound.DirectSoundSetCooperativeLevel(hWnd);
        }

        public void DirectSoundEnumerateSoundCards()
        {
            Library.DirectSound.DirectSoundEnumerateSoundCards();
        }

        public void DirectSoundSetCurrentPosition(ulong position)
        {
            Library.DirectSound.DirectSoundSetCurrentPosition(position);
        }

        public void DirectSoundSetupFormatDataStructure(ushort bitRate)
        {
            Library.DirectSound.DirectSoundSetupFormatDataStructure(bitRate);
        }

        public void DirectSoundSetupSecondaryBuffer(uint sndBuffLength)
        {
            Library.DirectSound.DirectSoundSetupSecondaryBuffer(sndBuffLength);
        }

        public int DirectSoundStop()
        {
            return Library.DirectSound.DirectSoundStop();
        }

        public void DirectSoundStopAndRelease()
        {
            Library.DirectSound.DirectSoundStopAndRelease();
        }

        public int DirectSoundPlay()
        {
            return Library.DirectSound.DirectSoundPlay();
        }

        public unsafe long DirectSoundGetCurrentPosition(ulong* playCursor, ulong* writeCursor)
        {
            return Library.DirectSound.DirectSoundGetCurrentPosition(playCursor, writeCursor);
        }
    }
}
