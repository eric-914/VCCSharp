using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICassette
    {
        unsafe CassetteState* GetCassetteState();
        unsafe uint FlushCassetteBuffer(byte* buffer, uint length);
        unsafe void LoadCassetteBuffer(byte* cassBuffer);
        void Motor(byte state);

        void SetTapeCounter(uint count);
        //void SetTapeMode(byte mode);

        unsafe int MountTape(byte* filename);
        void CloseTapeFile();
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

            UpdateTapeDialog(instance->TapeOffset);
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

        public void Motor(byte state)
        {
            unsafe
            {
                CassetteState* instance = GetCassetteState();

                instance->MotorState = state;

                switch (instance->MotorState)
                {
                    case 0:
                        _modules.CoCo.SetSndOutMode(0);

                        switch (instance->TapeMode)
                        {
                            case Define.STOP:
                                break;

                            case Define.PLAY:
                                instance->Quiet = 30;
                                instance->TempIndex = 0;
                                break;

                            case Define.REC:
                                SyncFileBuffer();
                                break;

                            case Define.EJECT:
                                break;
                        }

                        break;

                    case 1:
                        switch (instance->TapeMode)
                        {
                            case Define.STOP:
                                _modules.CoCo.SetSndOutMode(0);
                                break;

                            case Define.PLAY:
                                _modules.CoCo.SetSndOutMode(2);
                                break;

                            case Define.REC:
                                _modules.CoCo.SetSndOutMode(1);
                                break;

                            case Define.EJECT:
                                _modules.CoCo.SetSndOutMode(0);
                                break;
                        }

                        break;
                }
            }
        }

        public void SetTapeCounter(uint count)
        {
            unsafe
            {
                CassetteState* instance = GetCassetteState();

                instance->TapeOffset = count;

                if (instance->TapeOffset > instance->TotalSize)
                {
                    instance->TotalSize = instance->TapeOffset;
                }

                UpdateTapeDialog(instance->TapeOffset);
            }
        }

        public void SyncFileBuffer()
        {
            Library.Cassette.SyncFileBuffer();
        }

        public void CloseTapeFile()
        {
            Library.Cassette.CloseTapeFile();
        }

        public unsafe uint FlushCassetteBuffer(byte* buffer, uint length)
        {
            CassetteState* instance = GetCassetteState();

            if (instance->TapeMode == Define.REC)
            {
                Library.Cassette.FlushCassetteBuffer(buffer, length);
            }

            return instance->TapeOffset;
        }

        public unsafe int MountTape(byte* filename)
        {
            return Library.Cassette.MountTape(filename);
        }

        public void UpdateTapeDialog(uint offset)
        {
            _modules.Config.UpdateTapeDialog(offset);
        }
    }
}
