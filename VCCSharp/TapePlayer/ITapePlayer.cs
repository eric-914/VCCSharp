using VCCSharp.Configuration;

namespace VCCSharp.TapePlayer
{
    public interface ITapePlayer
    {
        void ShowDialog(IConfigurationManager state);

        void Browse();
        void Record();
        void Play();
        void Stop();
        void Eject();
        void Rewind();
    }
}
