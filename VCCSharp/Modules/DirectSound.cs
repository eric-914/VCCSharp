using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IDirectSound
    {
        void StopAndRelease();
        void SetCurrentPosition(ulong position);
    }

    public class DirectSound : IDirectSound
    {
        public void StopAndRelease()
        {
            Library.DirectSound.StopAndRelease();
        }

        public void SetCurrentPosition(ulong position)
        {
            Library.DirectSound.SetCurrentPosition(position);
        }
    }
}
