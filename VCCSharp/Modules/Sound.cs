using System;
using System.Runtime.InteropServices;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface ISound
    {
        unsafe bool DirectSoundInitialize(IntPtr* ds, _GUID* guid);
        unsafe int DirectSoundLock(IntPtr buffer, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);
        unsafe int DirectSoundUnlock(IntPtr buffer, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);
        unsafe long DirectSoundGetCurrentPosition(IntPtr buffer, ulong* playCursor, ulong* writeCursor);
        void DirectSoundSetCurrentPosition(IntPtr buffer, ulong position);
        int DirectSoundPlay(IntPtr buffer);
        int DirectSoundStop(IntPtr buffer);
        int DirectSoundBufferRelease(IntPtr buffer);

        void DirectSoundEnumerateSoundCards();
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int DirectSoundEnumerateCallbackTemplate(IntPtr guid, IntPtr description, IntPtr module, IntPtr context);

    public class Sound : ISound
    {
        private static DirectSoundEnumerateCallbackTemplate _delegateInstance;

        private readonly IModules _modules;

        public Sound(IModules modules)
        {
            _modules = modules;
        }

        public unsafe bool DirectSoundInitialize(IntPtr* lpds, _GUID* guid)
        {
            return Library.DirectSound.DirectSoundInitialize(lpds, guid) == Define.DS_OK;
        }

        public unsafe int DirectSoundLock(IntPtr buffer, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2)
        {
            return Library.DirectSound.DirectSoundLock(buffer, buffOffset, length, sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe int DirectSoundUnlock(IntPtr buffer, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2)
        {
            return Library.DirectSound.DirectSoundUnlock(buffer, sndPointer1, sndLength1, sndPointer2, sndLength2);
        }

        public unsafe long DirectSoundGetCurrentPosition(IntPtr buffer, ulong* playCursor, ulong* writeCursor)
        {
            return Library.DirectSound.DirectSoundGetCurrentPosition(buffer, playCursor, writeCursor);
        }

        public void DirectSoundSetCurrentPosition(IntPtr buffer, ulong position)
        {
            Library.DirectSound.DirectSoundSetCurrentPosition(buffer, position);
        }

        public int DirectSoundPlay(IntPtr buffer)
        {
            return Library.DirectSound.DirectSoundPlay(buffer);
        }

        public int DirectSoundStop(IntPtr buffer)
        {
            return Library.DirectSound.DirectSoundStop(buffer);
        }

        public int DirectSoundBufferRelease(IntPtr buffer)
        {
            return Library.DirectSound.DirectSoundBufferRelease(buffer);
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
