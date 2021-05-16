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
        void Motor(byte state);

        void TapeBrowse();

        void SetTapeCounter(uint count);
        void SetTapeMode(byte mode);
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
        
        public unsafe void LoadCassetteBufferCAS(byte* cassBuffer, uint* bytesMoved)
        {
            CasToWav(cassBuffer, Define.TAPEAUDIORATE / 60, bytesMoved);
        }

        public unsafe void CasToWav(byte* buffer, ushort bytesToConvert, uint* bytesConverted)
        {
            byte _byte = 0;
            byte mask = 0;

            CassetteState* instance = GetCassetteState();

            if (instance->Quiet > 0)
            {
                instance->Quiet--;

                //memset(buffer, 0, bytesToConvert);
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = 0;
                }

                return;
            }

            if ((instance->TapeOffset > instance->TotalSize) || (instance->TotalSize == 0))	//End of tape return nothing
            {
                //memset(buffer, 0, bytesToConvert);
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = 0;
                }

                instance->TapeMode = (byte)TapeModes.STOP;	//Stop at end of tape

                return;
            }

            while ((instance->TempIndex < bytesToConvert) && (instance->TapeOffset <= instance->TotalSize))
            {
                _byte = instance->CasBuffer[(instance->TapeOffset++) % instance->TotalSize];

                for (mask = 0; mask <= 7; mask++)
                {
                    if ((_byte & (1 << mask)) == 0)
                    {
                        //memcpy(&(instance->TempBuffer[instance->TempIndex]), instance->Zero, 40);
                        for (int index = 0; index < 40; index++)
                        {
                            instance->TempBuffer[instance->TempIndex + index] = instance->Zero[index];
                        }

                        instance->TempIndex += 40;
                    }
                    else
                    {
                        //memcpy(&(instance->TempBuffer[instance->TempIndex]), instance->One, 21);
                        for (int index = 0; index < 21; index++)
                        {
                            instance->TempBuffer[instance->TempIndex + index] = instance->One[index];
                        }

                        instance->TempIndex += 21;
                    }
                }
            }

            if (instance->TempIndex >= bytesToConvert)
            {
                //memcpy(buffer, instance->TempBuffer, bytesToConvert); //Fill the return Buffer
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = instance->TempBuffer[index];
                }

                //memcpy(instance->TempBuffer, &(instance->TempBuffer[bytesToConvert]), instance->TempIndex - bytesToConvert);	//Slide the overage to the front
                for (int index = 0; index < instance->TempIndex - bytesToConvert; index++)
                {
                    instance->TempBuffer[index] = instance->TempBuffer[bytesToConvert + index];
                }

                instance->TempIndex -= bytesToConvert; //Point to the Next free byte in the tempbuffer
            }
            else	//We ran out of source bytes
            {
                //memcpy(buffer, instance->TempBuffer, instance->TempIndex);						//Partial Fill of return buffer;
                for (int index = 0; index < instance->TempIndex; index++)
                {
                    buffer[index] = instance->TempBuffer[index];
                }

                //memset(&buffer[instance->TempIndex], 0, bytesToConvert - instance->TempIndex);		//and silence for the rest
                for (int index = 0; index < bytesToConvert - instance->TempIndex; index++)
                {
                    buffer[index + instance->TempIndex] = 0;
                }

                instance->TempIndex = 0;
            }
        }

        //--TODO: Seems this doesn't work
        public unsafe void LoadCassetteBufferWAV(byte* cassBuffer, uint* bytesMoved)
        {
            CassetteState* instance = GetCassetteState();

            _kernel.SetFilePointer(instance->TapeHandle, instance->TapeOffset + 44, null, Define.FILE_BEGIN);
            _kernel.ReadFile(instance->TapeHandle, cassBuffer, Define.TAPEAUDIORATE / 60, bytesMoved, null);

            instance->TapeOffset += *bytesMoved;

            if (instance->TapeOffset > instance->TotalSize)
            {
                instance->TapeOffset = instance->TotalSize;
            }
        }

        public unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            Library.Cassette.FlushCassetteBuffer(buffer, length);
        }

        public void Motor(byte state)
        {
            Library.Cassette.Motor(state);
        }

        public void TapeBrowse()
        {
            Library.Cassette.TapeBrowse();
        }

        public void SetTapeCounter(uint count)
        {
            Library.Cassette.SetTapeCounter(count);
        }

        public void SetTapeMode(byte mode)
        {
            Library.Cassette.SetTapeMode(mode);
        }
    }
}
