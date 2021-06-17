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
        unsafe DirectSoundState* GetDirectSoundState();
        unsafe bool DirectSoundInitialize(IntPtr* ds, _GUID* guid);
        int DirectSoundSetCooperativeLevel(IntPtr lpds, HWND hWnd);
        unsafe void DirectSoundSetupFormatDataStructure(DirectSoundState* ds, ushort bitRate);
        unsafe void DirectSoundSetupSecondaryBuffer(DirectSoundState* ds, uint sndBuffLength);
        unsafe int DirectSoundCreateSoundBuffer(DirectSoundState* ds, IntPtr lpds);
        unsafe int DirectSoundLock(DirectSoundState* ds, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);
        unsafe int DirectSoundUnlock(DirectSoundState* ds, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);
        unsafe long DirectSoundGetCurrentPosition(DirectSoundState* ds, ulong* playCursor, ulong* writeCursor);
        unsafe void DirectSoundSetCurrentPosition(DirectSoundState* ds, ulong position);
        unsafe int DirectSoundPlay(DirectSoundState* ds);
        unsafe int DirectSoundStop(DirectSoundState* ds);
        unsafe void DirectSoundStopAndRelease(DirectSoundState* ds, IntPtr lpds);
        unsafe bool DirectSoundHasBuffer(DirectSoundState* ds);
        unsafe int DirectSoundBufferRelease(DirectSoundState* ds);
        unsafe int DirectSoundInterfaceRelease(IntPtr lpds);

        void DirectSoundEnumerateSoundCards();
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

        public unsafe DirectSoundState* GetDirectSoundState()
        {
            return Library.DirectSound.GetDirectSoundState();
        }

        public unsafe bool DirectSoundInitialize(IntPtr* lpds, _GUID* guid)
        {
            return Library.DirectSound.DirectSoundInitialize(lpds, guid) == Define.DS_OK;
        }

        public int DirectSoundSetCooperativeLevel(IntPtr lpds, HWND hWnd)
        {
            return Library.DirectSound.DirectSoundSetCooperativeLevel(lpds, hWnd, Define.DSSCL_NORMAL);
        }

        public unsafe void DirectSoundSetupFormatDataStructure(DirectSoundState* ds, ushort bitRate)
        {
            Library.DirectSound.DirectSoundSetupFormatDataStructure(ds, bitRate);
        }

        public unsafe void DirectSoundSetupSecondaryBuffer(DirectSoundState* ds, uint sndBuffLength)
        {
            int flags = Define.DSBCAPS_GETCURRENTPOSITION2 | Define.DSBCAPS_LOCSOFTWARE | Define.DSBCAPS_STATIC | Define.DSBCAPS_GLOBALFOCUS;
            Library.DirectSound.DirectSoundSetupSecondaryBuffer(ds, sndBuffLength, (uint)flags);
        }

        public unsafe int DirectSoundCreateSoundBuffer(DirectSoundState* ds, IntPtr lpds)
        {
            return Library.DirectSound.DirectSoundCreateSoundBuffer(ds, lpds);
        }

        public unsafe int DirectSoundLock(DirectSoundState* ds, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2)
        {
            return Library.DirectSound.DirectSoundLock(ds, buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe int DirectSoundUnlock(DirectSoundState* ds, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2)
        {
            return Library.DirectSound.DirectSoundUnlock(ds, sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe long DirectSoundGetCurrentPosition(DirectSoundState* ds, ulong* playCursor, ulong* writeCursor)
        {
            return Library.DirectSound.DirectSoundGetCurrentPosition(ds, playCursor, writeCursor);
        }

        public unsafe void DirectSoundSetCurrentPosition(DirectSoundState* ds, ulong position)
        {
            Library.DirectSound.DirectSoundSetCurrentPosition(ds, position);
        }

        public unsafe int DirectSoundPlay(DirectSoundState* ds)
        {
            return Library.DirectSound.DirectSoundPlay(ds);
        }

        public unsafe int DirectSoundStop(DirectSoundState* ds)
        {
            return Library.DirectSound.DirectSoundStop(ds);
        }

        public unsafe void DirectSoundStopAndRelease(DirectSoundState* ds, IntPtr lpds)
        {
            Library.DirectSound.DirectSoundStopAndRelease(ds, lpds);
        }

        public unsafe bool DirectSoundHasBuffer(DirectSoundState* ds)
        {
            return Library.DirectSound.DirectSoundHasBuffer(ds) != 0;
        }

        public unsafe int DirectSoundBufferRelease(DirectSoundState* ds)
        {
            return Library.DirectSound.DirectSoundBufferRelease(ds);
        }

        public unsafe int DirectSoundInterfaceRelease(IntPtr lpds)
        {
            return Library.DirectSound.DirectSoundInterfaceRelease(lpds);
        }

        //.........................................................................//

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
