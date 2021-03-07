using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ICassette
    {
        unsafe void FlushCassetteBuffer(byte* buffer, uint length);
        unsafe void LoadCassetteBuffer(byte* cassBuffer);
    }

    public class Cassette : ICassette
    {
        public unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            Library.Cassette.FlushCassetteBuffer(buffer, length);
        }

        public unsafe void LoadCassetteBuffer(byte* cassBuffer)
        {
            Library.Cassette.LoadCassetteBuffer(cassBuffer);
        }
    }
}
