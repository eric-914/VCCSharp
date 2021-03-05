using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public class Audio
    {
        public short SoundDeInit()
        {
            return Library.Audio.SoundDeInit();
        }
    }
}
