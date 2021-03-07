using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* aBuffer, ushort length);
    }

    public class Audio : IAudio
    {
        public short SoundDeInit()
        {
            return Library.Audio.SoundDeInit();
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
