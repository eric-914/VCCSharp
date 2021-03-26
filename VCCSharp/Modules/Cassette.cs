using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICassette
    {
        unsafe CassetteState* GetCassetteState();
        unsafe void FlushCassetteBuffer(byte* buffer, uint length);
        unsafe void LoadCassetteBuffer(byte* cassBuffer);
    }

    public class Cassette : ICassette
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        public Cassette(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public unsafe CassetteState* GetCassetteState()
        {
            return Library.Cassette.GetCassetteState();
        }

        public unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            Library.Cassette.FlushCassetteBuffer(buffer, length);
        }

        public unsafe void LoadCassetteBuffer(byte* cassBuffer)
        {
            CassetteState* cassetteState = GetCassetteState();

            uint bytesMoved = 0;

            if (cassetteState->TapeMode != (byte)TapeModes.PLAY)
            {
                return;
            }

            switch ((TapeFileType)(cassetteState->FileType))
            {
                case TapeFileType.WAV:
                    LoadCassetteBufferWAV(cassBuffer, &bytesMoved);
                    break;

                case TapeFileType.CAS:
                    LoadCassetteBufferCAS(cassBuffer, &bytesMoved);
                    break;
            }

            _modules.Config.UpdateTapeDialog((ushort)cassetteState->TapeOffset, cassetteState->TapeMode);
        }

        public unsafe void LoadCassetteBufferWAV(byte* cassBuffer, uint* bytesMoved)
        {
            CassetteState* instance = GetCassetteState();

            Library.Cassette.LoadCassetteBufferWAV(cassBuffer, bytesMoved);
        }

        public unsafe void LoadCassetteBufferCAS(byte* cassBuffer, uint* bytesMoved)
        {
            CasToWav(cassBuffer, Define.TAPEAUDIORATE / 60, bytesMoved);
        }

        public unsafe void CasToWav(byte* buffer, ushort bytesToConvert, uint* bytesConverted)
        {
            Library.Cassette.CasToWav(buffer, bytesToConvert, bytesConverted);
        }
    }
}
