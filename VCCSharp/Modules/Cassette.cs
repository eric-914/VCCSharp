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

        public Cassette(IModules modules)
        {
            _modules = modules;
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
            CassetteState* instance = GetCassetteState();

            uint bytesMoved = 0;

            if (instance->TapeMode != (byte)TapeModes.PLAY)
            {
                return;
            }

            switch ((TapeFileType)(instance->FileType))
            {
                case TapeFileType.WAV:
                    LoadCassetteBufferWAV(cassBuffer, &bytesMoved);
                    break;

                case TapeFileType.CAS:
                    LoadCassetteBufferCAS(cassBuffer, &bytesMoved);
                    break;
            }

            _modules.Config.UpdateTapeDialog((ushort)instance->TapeOffset, instance->TapeMode);

            //Library.Cassette.LoadCassetteBuffer(cassBuffer);
        }

        public unsafe void LoadCassetteBufferWAV(byte* cassBuffer, uint* bytesMoved)
        {
            Library.Cassette.LoadCassetteBufferWAV(cassBuffer, bytesMoved);
        }

        public unsafe void LoadCassetteBufferCAS(byte* cassBuffer, uint* bytesMoved)
        {
            Library.Cassette.LoadCassetteBufferCAS(cassBuffer, bytesMoved);
        }
    }
}
