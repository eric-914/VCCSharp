using System;
using System.Runtime.InteropServices;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ISound
    {
        unsafe bool DirectSoundInitialize(IntPtr* ds, _GUID* guid);

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
