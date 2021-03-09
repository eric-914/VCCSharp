using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IDirectSound
    {
        void StopAndRelease();
    }

    public class DirectSound : IDirectSound
    {
        public void StopAndRelease()
        {
            Library.DirectSound.StopAndRelease();
        }
    }
}
