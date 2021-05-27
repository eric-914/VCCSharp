using System;
using System.Runtime.InteropServices;
using VCCSharp.IoC;
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

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int DirectSoundEnumerateCallbackTemplate(IntPtr guid, IntPtr description, IntPtr module, IntPtr context);

    public class DirectSound : IDirectSound
    {
        private static DirectSoundEnumerateCallbackTemplate _delegateInstance;

        private readonly IModules _modules;

        public DirectSound(IModules modules)
        {
            _modules = modules;
        }

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

        public unsafe void DirectSoundEnumerateSoundCards()
        {
            _delegateInstance = DirectSoundEnumerateCallback;

            void* fn = (void*)Marshal.GetFunctionPointerForDelegate(_delegateInstance);

            Library.DirectSound.DirectSoundEnumerateSoundCards(fn);
        }

        public int DirectSoundEnumerateCallback(IntPtr guid, IntPtr description, IntPtr module, IntPtr context)
        {
            unsafe
            {
                var text = Converter.ToString((byte*)description);
                var index = _modules.Config.NumberOfSoundCards;

                var cards = _modules.Config.SoundCards;

                fixed (SoundCardList* card = &(cards[index]))
                {
                    Converter.ToByteArray(text, (*card).CardName);
                    (*card).Guid =(_GUID*)guid;
                }

                _modules.Config.NumberOfSoundCards++;

                return (_modules.Config.NumberOfSoundCards < Define.MAXCARDS) ? Define.TRUE : Define.FALSE;
            }
        }
    }
}
