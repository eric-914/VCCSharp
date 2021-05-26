using System;
using System.Windows;
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
        unsafe void LoadCassetteBuffer(byte* buffer);
        void Motor(byte state);

        //void SetTapeCounter(uint count);
        //void SetTapeMode(byte mode);

        unsafe int MountTape(byte* filename);
        void CloseTapeFile();

        Action<int> UpdateTapeDialog { get; set; }

        byte MotorState { get; set; }
        byte TapeMode { get; set; }
    }

    public class Cassette : ICassette
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        public byte MotorState { get; set; }
        public byte TapeMode { get; set; } = Define.STOP;

        private byte _quiet = 30;
        //private byte _writeProtect;

        public int LastTrans;

        public Action<int> UpdateTapeDialog { get; set; }

        public Cassette(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;

            UpdateTapeDialog = offset => { }; //_modules.Config.UpdateTapeDialog((uint) offset);
        }

        public unsafe CassetteState* GetCassetteState()
        {
            return Library.Cassette.GetCassetteState();
        }

        public unsafe void LoadCassetteBuffer(byte* buffer)
        {
            CassetteState* instance = GetCassetteState();

            uint bytesMoved = 0;

            if (TapeMode != (byte)TapeModes.PLAY)
            {
                return;
            }

            switch ((TapeFileType)(instance->FileType))
            {
                case TapeFileType.WAV:
                    LoadCassetteBufferWav(buffer, &bytesMoved);
                    break;

                case TapeFileType.CAS:
                    LoadCassetteBufferCas(buffer, &bytesMoved);
                    break;
            }

            UpdateTapeDialog((int)instance->TapeOffset);
        }

        public unsafe void LoadCassetteBufferCas(byte* buffer, uint* bytesMoved)
        {
            CasToWav(buffer, Define.TAPEAUDIORATE / 60, bytesMoved);
        }

        public unsafe void CasToWav(byte* buffer, ushort bytesToConvert, uint* bytesConverted)
        {
            CassetteState* instance = GetCassetteState();

            if (_quiet > 0)
            {
                _quiet--;

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

                TapeMode = (byte)TapeModes.STOP;	//Stop at end of tape

                return;
            }

            while ((instance->TempIndex < bytesToConvert) && (instance->TapeOffset <= instance->TotalSize))
            {
                var _byte = instance->CasBuffer[(instance->TapeOffset++) % instance->TotalSize];

                byte mask;
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

                instance->TempIndex -= bytesToConvert; //Point to the Next free byte in the temp buffer
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
        public unsafe void LoadCassetteBufferWav(byte* buffer, uint* bytesMoved)
        {
            CassetteState* instance = GetCassetteState();

            _kernel.SetFilePointer(instance->TapeHandle, instance->TapeOffset + 44, null, Define.FILE_BEGIN);
            _kernel.ReadFile(instance->TapeHandle, buffer, Define.TAPEAUDIORATE / 60, bytesMoved, null);

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

                MotorState = state;

                switch (MotorState)
                {
                    case 0:
                        _modules.CoCo.SetSndOutMode(0);

                        switch (TapeMode)
                        {
                            case Define.STOP:
                                break;

                            case Define.PLAY:
                                _quiet = 30;
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
                        switch (TapeMode)
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

                UpdateTapeDialog((int)instance->TapeOffset);
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

            if (TapeMode == Define.REC)
            {
                switch (instance->FileType)
                {
                    case Define.WAV:
                        FlushCassetteWav(buffer, length);
                        break;

                    case Define.CAS:
                        FlushCassetteCas(buffer, length);
                        break;
                }
            }

            return instance->TapeOffset;
        }

        public unsafe void FlushCassetteWav(byte* buffer, uint length)
        {
            Library.Cassette.FlushCassetteWAV(buffer, length);

            CassetteState* instance = GetCassetteState();

            if (length != instance->BytesMoved) {
                return;
            }

            instance->TapeOffset += length;

            if (instance->TapeOffset > instance->TotalSize) {
                instance->TotalSize = instance->TapeOffset;
            }
        }

        public unsafe void FlushCassetteCas(byte* buffer, uint length)
        {
            CassetteState* instance = GetCassetteState();

            for (int index = 0; index < length; index++)
            {
                var sample = buffer[index];

                if ((instance->LastSample <= 0x80) && (sample > 0x80)) //Low to High transition
                {
                    var width = index - instance->LastTrans;

                    if ((width < 10) || (width > 50))	//Invalid Sample Skip it
                    {
                        instance->LastSample = 0;
                        instance->LastTrans = index;
                        instance->Mask = 0;
                        instance->Byte = 0;
                    }
                    else
                    {
                        byte bit = 1;

                        if (width > 30)
                        {
                            bit = 0;
                        }

                        instance->Byte |= (byte)(bit << instance->Mask);
                        instance->Mask++;
                        instance->Mask &= 7;

                        if (instance->Mask == 0)
                        {
                            instance->CasBuffer[instance->TapeOffset++] = instance->Byte;
                            instance->Byte = 0;

                            //Don't blow past the end of the buffer
                            if (instance->TapeOffset >= Define.WRITEBUFFERSIZE)
                            {	
                                TapeMode = Define.STOP;
                            }
                        }
                    }

                    instance->LastTrans = index;
                }

                instance->LastSample = sample;
            }
            
            instance->LastTrans -= (int)length;

            if (instance->TapeOffset > instance->TotalSize)
            {
                instance->TotalSize = instance->TapeOffset;
            }
        }

        public unsafe int MountTape(byte* filename)
        {
            CassetteState* instance = GetCassetteState();

            if (instance->TapeHandle != IntPtr.Zero)
            {
                TapeMode = Define.STOP;

                CloseTapeFile();
            }

            //_writeProtect = 0;
            instance->FileType = 0;	//0=wav 1=cas

            instance->TapeHandle = _modules.FileOperations.FileCreateFile(filename, Define.GENERIC_READ | Define.GENERIC_WRITE);

            if (instance->TapeHandle == Define.INVALID_HANDLE_VALUE)	//Can't open read/write. try read only
            {
                instance->TapeHandle = _modules.FileOperations.FileCreateFile(filename, Define.GENERIC_READ);
                //_writeProtect = 1;
            }

            if (instance->TapeHandle == Define.INVALID_HANDLE_VALUE)
            {
                MessageBox.Show("Can't Mount", "Error");

                return 0;	//Give up
            }

            return Library.Cassette.MountTape(filename);


        }
    }
}
