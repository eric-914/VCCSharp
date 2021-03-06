using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        short SoundDeInit();
    }

    public class Audio : IAudio
    {
        public short SoundDeInit()
        {
            return Library.Audio.SoundDeInit();
        }
    }
}
