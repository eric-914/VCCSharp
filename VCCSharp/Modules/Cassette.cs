using System;
using System.IO;
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

        int MountTape(string filename);
        void CloseTapeFile();

        Action<int> UpdateTapeDialog { get; set; }

        byte MotorState { get; set; }
        byte TapeMode { get; set; }
        string TapeFileName { get; set; }
    }

    public class Cassette : ICassette
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        public byte MotorState { get; set; }
        public byte TapeMode { get; set; } = Define.STOP;

        private readonly byte[] _one = { 0x80, 0xA8, 0xC8, 0xE8, 0xE8, 0xF8, 0xF8, 0xE8, 0xC8, 0xA8, 0x78, 0x50, 0x50, 0x30, 0x10, 0x00, 0x00, 0x10, 0x30, 0x30, 0x50 };
        private readonly byte[] _zero = { 0x80, 0x90, 0xA8, 0xB8, 0xC8, 0xD8, 0xE8, 0xE8, 0xF0, 0xF8, 0xF8, 0xF8, 0xF0, 0xE8, 0xD8, 0xC8, 0xB8, 0xA8, 0x90, 0x78, 0x78, 0x68, 0x50, 0x40, 0x30, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x08, 0x10, 0x10, 0x20, 0x30, 0x40, 0x50, 0x68, 0x68 };

        public string TapeFileName { get; set; } //[Define.MAX_PATH];

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

            if ((instance->TapeOffset > instance->TotalSize) || (instance->TotalSize == 0)) //End of tape return nothing
            {
                //memset(buffer, 0, bytesToConvert);
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = 0;
                }

                TapeMode = (byte)TapeModes.STOP; //Stop at end of tape

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
                            instance->TempBuffer[instance->TempIndex + index] = _zero[index];
                        }

                        instance->TempIndex += 40;
                    }
                    else
                    {
                        //memcpy(&(instance->TempBuffer[instance->TempIndex]), instance->One, 21);
                        for (int index = 0; index < 21; index++)
                        {
                            instance->TempBuffer[instance->TempIndex + index] = _one[index];
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
            else //We ran out of source bytes
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

            _kernel.SetFilePointer(instance->TapeHandle, instance->TapeOffset + 44, null, (uint)Define.FILE_BEGIN);
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

        public void CloseTapeFile()
        {
            unsafe
            {
                CassetteState* instance = GetCassetteState();

                if (instance->TapeHandle == IntPtr.Zero)
                {
                    return;
                }

                SyncFileBuffer();

                _modules.FileOperations.FileCloseHandle(instance->TapeHandle);

                instance->TapeHandle = IntPtr.Zero;
                instance->TotalSize = 0;
            }
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

        public unsafe void FlushCassetteCas(byte* buffer, uint length)
        {
            CassetteState* instance = GetCassetteState();

            for (int index = 0; index < length; index++)
            {
                var sample = buffer[index];

                if ((instance->LastSample <= 0x80) && (sample > 0x80)) //Low to High transition
                {
                    var width = index - instance->LastTrans;

                    if ((width < 10) || (width > 50)) //Invalid Sample Skip it
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

        //Return 1 on success 0 on fail
        public unsafe int MountTape(string filename)
        {
            CassetteState* instance = GetCassetteState();

            if (instance->TapeHandle != IntPtr.Zero)
            {
                TapeMode = Define.STOP;

                CloseTapeFile();
            }

            //_writeProtect = 0;
            instance->FileType = 0; //0=wav 1=cas


            fixed (byte* p = Converter.ToByteArray(filename))
            {
                instance->TapeHandle =
                    _modules.FileOperations.FileCreateFile(p, Define.GENERIC_READ | Define.GENERIC_WRITE);

                if (instance->TapeHandle == Define.INVALID_HANDLE_VALUE) //Can't open read/write. try read only
                {
                    instance->TapeHandle = _modules.FileOperations.FileCreateFile(p, Define.GENERIC_READ);
                    //_writeProtect = 1;
                }
            }

            if (instance->TapeHandle == Define.INVALID_HANDLE_VALUE)
            {
                MessageBox.Show("Can't Mount", "Error");

                return 0; //Give up
            }

            instance->TotalSize =
                (uint)_modules.FileOperations.FileSetFilePointer(instance->TapeHandle, Define.FILE_END);
            instance->TapeOffset = 0;

            var extension = Path.GetExtension(filename)?.ToUpper();

            if (extension == ".CAS")
            {
                instance->FileType = Define.CAS;
                instance->LastTrans = 0;
                instance->Mask = 0;
                instance->Byte = 0;
                instance->LastSample = 0;
                instance->TempIndex = 0;

                ResetCassetteBuffer();

                _modules.FileOperations.FileSetFilePointer(instance->TapeHandle, Define.FILE_BEGIN);

                ulong moved = 0;

                //Read the whole file in for .CAS files
                _modules.FileOperations.FileReadFile(instance->TapeHandle, instance->CasBuffer, instance->TotalSize,
                    &moved);

                instance->BytesMoved = (uint)moved;

                if (instance->BytesMoved != instance->TotalSize)
                {
                    return 0;
                }
            }

            return 1;
        }

        public void SyncFileBuffer()
        {
            unsafe
            {
                CassetteState* instance = GetCassetteState();

                _modules.FileOperations.FileSetFilePointer(instance->TapeHandle, Define.FILE_BEGIN);

                switch (instance->FileType)
                {
                    case Define.CAS:
                        SyncFileBufferCas();
                        break;

                    case Define.WAV:
                        SyncFileBufferWav();
                        break;
                }

                _modules.FileOperations.FileFlushFileBuffers(instance->TapeHandle);
            }
        }

        public void SyncFileBufferCas()
        {
            unsafe
            {
                CassetteState* instance = GetCassetteState();

                instance->CasBuffer[instance->TapeOffset] = instance->Byte;	//capture the last byte
                instance->LastTrans = 0;	//reset all static inter-call variables
                instance->Mask = 0;
                instance->Byte = 0;
                instance->LastSample = 0;
                instance->TempIndex = 0;

                _modules.FileOperations.FileWriteFile(instance->TapeHandle, instance->CasBuffer, (int)(instance->TapeOffset));
            }
        }

        public void SyncFileBufferWav()
        {
            uint formatSize = 16;		//size of WAVE section chunk
            ushort waveType = 1;		//WAVE type format
            ushort channels = 1;		//mono/stereo
            uint bitRate = Define.TAPEAUDIORATE;		//sample rate
            ushort bitsperSample = 8;	//Bits/sample
            uint bytesperSec = (uint)(bitRate * channels * (bitsperSample / 8));		//bytes/sec
            ushort blockAlign = (ushort)((bitsperSample * channels) / 8);		//Block alignment

            unsafe
            {
                CassetteState* instance = GetCassetteState();

                uint fileSize = instance->TotalSize + 40 - 8;
                uint chunkSize = fileSize;

                _modules.FileOperations.FileWriteFile(instance->TapeHandle, "RIFF");
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)&fileSize, 4);

                _modules.FileOperations.FileWriteFile(instance->TapeHandle, "WAVE");

                _modules.FileOperations.FileWriteFile(instance->TapeHandle, "fmt ");
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&formatSize), 4);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&waveType), 2);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&channels), 2);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&bitRate), 4);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&bytesperSec), 4);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&blockAlign), 2);
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&bitsperSample), 2);

                _modules.FileOperations.FileWriteFile(instance->TapeHandle, "data");
                _modules.FileOperations.FileWriteFile(instance->TapeHandle, (byte*)(&chunkSize), 4);
            }
        }

        public unsafe void FlushCassetteWav(byte* buffer, uint length)
        {
            CassetteState* instance = GetCassetteState();

            _modules.FileOperations.FileSetFilePointer(instance->TapeHandle, Define.FILE_BEGIN, instance->TapeOffset + 44);

            _modules.FileOperations.FileWriteFile(instance->TapeHandle, buffer, (int)length);

            if (length != instance->BytesMoved)
            {
                return;
            }

            instance->TapeOffset += length;

            if (instance->TapeOffset > instance->TotalSize)
            {
                instance->TotalSize = instance->TapeOffset;
            }
        }

        public void ResetCassetteBuffer()
        {
            Library.Cassette.ResetCassetteBuffer();
        }
    }
}
