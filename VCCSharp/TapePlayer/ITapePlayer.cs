using VCCSharp.Models;

namespace VCCSharp.TapePlayer
{
    public interface ITapePlayer
    {
        unsafe void ShowDialog(ConfigState* state);

        void Browse();
        void Record();
        void Play();
        void Stop();
        void Eject();
        void Rewind();
    }
}
