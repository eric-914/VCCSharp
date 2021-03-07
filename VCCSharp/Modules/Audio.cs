using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        short SoundDeInit();
        void ResetAudio();
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
    }
}
