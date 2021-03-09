using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        unsafe AudioState* GetAudioState();
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* aBuffer, ushort length);
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;

        public Audio(IModules modules)
        {
            _modules = modules;
        }

        public unsafe AudioState* GetAudioState()
        {
            return Library.Audio.GetAudioState();
        }

        public short SoundDeInit()
        {
            unsafe
            {
                AudioState* audioState = GetAudioState();

                if (audioState->InitPassed != Define.FALSE)
                {
                    audioState->InitPassed = 0;

                    _modules.DirectSound.StopAndRelease();
                }

                return 0;
            }
        }

        public void ResetAudio()
        {
            Library.Audio.ResetAudio();
        }

        public unsafe void FlushAudioBuffer(uint* aBuffer, ushort length)
        {
            Library.Audio.FlushAudioBuffer(aBuffer, length);
        }
    }
}
