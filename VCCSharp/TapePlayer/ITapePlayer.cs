using VCCSharp.Modules;

namespace VCCSharp.TapePlayer
{
    public interface ITapePlayer
    {
        void ShowDialog(IConfigurationModule state);

        void Browse();
        void Record();
        void Play();
        void Stop();
        void Eject();
        void Rewind();
    }
}
