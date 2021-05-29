using System;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface ICassette
    {
        HANDLE TapeHandle { get; set; }

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
        uint TapeOffset { get; set; }
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
        public uint TapeOffset { get; set; }

        public byte FileType;

        private byte _quiet = 30;
        //private byte _writeProtect;

        public int LastTrans;

        private byte _byte;
        private byte _lastSample;
        private byte _mask;

        private uint _bytesMoved;
        private uint _totalSize;

        public HANDLE TapeHandle { get; set; }

        public uint TempIndex;
        public byte[] TempBuffer = new byte[8192];

        public byte[] CasBuffer = new byte[Define.WRITEBUFFERSIZE];

        public Action<int> UpdateTapeDialog { get; set; }

        public Cassette(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;

            UpdateTapeDialog = offset => { }; //_modules.Config.UpdateTapeDialog((uint) offset);
        }

        public unsafe void LoadCassetteBuffer(byte* buffer)
        {
            uint bytesMoved = 0;

            if (TapeMode != (byte)TapeModes.PLAY)
            {
                return;
            }

            switch ((TapeFileType)(FileType))
            {
                case TapeFileType.WAV:
                    LoadCassetteBufferWav(buffer, &bytesMoved);
                    break;

                case TapeFileType.CAS:
                    LoadCassetteBufferCas(buffer, &bytesMoved);
                    break;
            }

            UpdateTapeDialog((int)TapeOffset);
        }

        public unsafe void LoadCassetteBufferCas(byte* buffer, uint* bytesMoved)
        {
            CasToWav(buffer, Define.TAPEAUDIORATE / 60, bytesMoved);
        }

        public unsafe void CasToWav(byte* buffer, ushort bytesToConvert, uint* bytesConverted)
        {
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

            if ((TapeOffset > _totalSize) || (_totalSize == 0)) //End of tape return nothing
            {
                //memset(buffer, 0, bytesToConvert);
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = 0;
                }

                TapeMode = (byte)TapeModes.STOP; //Stop at end of tape

                return;
            }

            while ((TempIndex < bytesToConvert) && (TapeOffset <= _totalSize))
            {
                var b = CasBuffer[(TapeOffset++) % _totalSize];

                byte mask;
                for (mask = 0; mask <= 7; mask++)
                {
                    if ((b & (1 << mask)) == 0)
                    {
                        //memcpy(&(instance->TempBuffer[instance->TempIndex]), instance->Zero, 40);
                        for (int index = 0; index < 40; index++)
                        {
                            TempBuffer[TempIndex + index] = _zero[index];
                        }

                        TempIndex += 40;
                    }
                    else
                    {
                        //memcpy(&(instance->TempBuffer[instance->TempIndex]), instance->One, 21);
                        for (int index = 0; index < 21; index++)
                        {
                            TempBuffer[TempIndex + index] = _one[index];
                        }

                        TempIndex += 21;
                    }
                }
            }

            if (TempIndex >= bytesToConvert)
            {
                //memcpy(buffer, instance->TempBuffer, bytesToConvert); //Fill the return Buffer
                for (int index = 0; index < bytesToConvert; index++)
                {
                    buffer[index] = TempBuffer[index];
                }

                //memcpy(instance->TempBuffer, &(instance->TempBuffer[bytesToConvert]), instance->TempIndex - bytesToConvert);	//Slide the overage to the front
                for (int index = 0; index < TempIndex - bytesToConvert; index++)
                {
                    TempBuffer[index] = TempBuffer[bytesToConvert + index];
                }

                TempIndex -= bytesToConvert; //Point to the Next free byte in the temp buffer
            }
            else //We ran out of source bytes
            {
                //memcpy(buffer, instance->TempBuffer, instance->TempIndex);						//Partial Fill of return buffer;
                for (int index = 0; index < TempIndex; index++)
                {
                    buffer[index] = TempBuffer[index];
                }

                //memset(&buffer[instance->TempIndex], 0, bytesToConvert - instance->TempIndex);		//and silence for the rest
                for (int index = 0; index < bytesToConvert - TempIndex; index++)
                {
                    buffer[index + TempIndex] = 0;
                }

                TempIndex = 0;
            }
        }

        //--TODO: Seems this doesn't work
        public unsafe void LoadCassetteBufferWav(byte* buffer, uint* bytesMoved)
        {
            _kernel.SetFilePointer(TapeHandle, TapeOffset + 44, null, (uint)Define.FILE_BEGIN);
            _kernel.ReadFile(TapeHandle, buffer, Define.TAPEAUDIORATE / 60, bytesMoved, null);

            TapeOffset += *bytesMoved;

            if (TapeOffset > _totalSize)
            {
                TapeOffset = _totalSize;
            }
        }

        public void Motor(byte state)
        {
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
                            TempIndex = 0;
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

        //public void SetTapeCounter(uint count)
        //{
        //    unsafe
        //    {
        //        CassetteState* instance = GetCassetteState();

        //        instance->TapeOffset = count;

        //        if (instance->TapeOffset > instance->TotalSize)
        //        {
        //            instance->TotalSize = instance->TapeOffset;
        //        }

        //        UpdateTapeDialog((int)instance->TapeOffset);
        //    }
        //}

        public void CloseTapeFile()
        {
            if (TapeHandle == Zero)
            {
                return;
            }

            SyncFileBuffer();

            _modules.FileOperations.FileCloseHandle(TapeHandle);

            TapeHandle = Zero;
            _totalSize = 0;
        }

        public unsafe uint FlushCassetteBuffer(byte* buffer, uint length)
        {
            if (TapeMode == Define.REC)
            {
                switch (FileType)
                {
                    case Define.WAV:
                        FlushCassetteWav(buffer, length);
                        break;

                    case Define.CAS:
                        FlushCassetteCas(buffer, length);
                        break;
                }
            }

            return TapeOffset;
        }

        public unsafe void FlushCassetteCas(byte* buffer, uint length)
        {
            for (int index = 0; index < length; index++)
            {
                var sample = buffer[index];

                if ((_lastSample <= 0x80) && (sample > 0x80)) //Low to High transition
                {
                    var width = index - LastTrans;

                    if ((width < 10) || (width > 50)) //Invalid Sample Skip it
                    {
                        _lastSample = 0;
                        LastTrans = index;
                        _mask = 0;
                        _byte = 0;
                    }
                    else
                    {
                        byte bit = 1;

                        if (width > 30)
                        {
                            bit = 0;
                        }

                        _byte |= (byte)(bit << _mask);
                        _mask++;
                        _mask &= 7;

                        if (_mask == 0)
                        {
                            CasBuffer[TapeOffset++] = _byte;
                            _byte = 0;

                            //Don't blow past the end of the buffer
                            if (TapeOffset >= Define.WRITEBUFFERSIZE)
                            {
                                TapeMode = Define.STOP;
                            }
                        }
                    }

                    LastTrans = index;
                }

                _lastSample = sample;
            }

            LastTrans -= (int)length;

            if (TapeOffset > _totalSize)
            {
                _totalSize = TapeOffset;
            }
        }

        //Return 1 on success 0 on fail
        public unsafe int MountTape(string filename)
        {
            if (TapeHandle != Zero)
            {
                TapeMode = Define.STOP;

                CloseTapeFile();
            }

            //_writeProtect = 0;
            FileType = 0; //0=wav 1=cas

            TapeHandle =
                _modules.FileOperations.FileOpenFile(filename, Define.GENERIC_READ | Define.GENERIC_WRITE);

            if (TapeHandle == Define.INVALID_HANDLE_VALUE) //Can't open read/write. try read only
            {
                TapeHandle = _modules.FileOperations.FileOpenFile(filename, Define.GENERIC_READ);
                //_writeProtect = 1;
            }

            if (TapeHandle == Define.INVALID_HANDLE_VALUE)
            {
                MessageBox.Show("Can't Mount", "Error");

                return 0; //Give up
            }

            _totalSize = (uint)_modules.FileOperations.FileSetFilePointer(TapeHandle, Define.FILE_END);
            TapeOffset = 0;

            var extension = Path.GetExtension(filename)?.ToUpper();

            if (extension == ".CAS")
            {
                FileType = Define.CAS;
                LastTrans = 0;
                _mask = 0;
                _byte = 0;
                _lastSample = 0;
                TempIndex = 0;

                CasBuffer = new byte[Define.WRITEBUFFERSIZE];

                _modules.FileOperations.FileSetFilePointer(TapeHandle, Define.FILE_BEGIN);

                ulong moved = 0;

                //Read the whole file in for .CAS files
                fixed (byte* p = CasBuffer)
                {
                    _modules.FileOperations.FileReadFile(TapeHandle, p, _totalSize, &moved);
                }

                _bytesMoved = (uint)moved;

                if (_bytesMoved != _totalSize)
                {
                    return 0;
                }
            }

            return 1;
        }

        public void SyncFileBuffer()
        {
            _modules.FileOperations.FileSetFilePointer(TapeHandle, Define.FILE_BEGIN);

            switch (FileType)
            {
                case Define.CAS:
                    SyncFileBufferCas();
                    break;

                case Define.WAV:
                    SyncFileBufferWav();
                    break;
            }

            _modules.FileOperations.FileFlushFileBuffers(TapeHandle);
        }

        public void SyncFileBufferCas()
        {
            unsafe
            {
                CasBuffer[TapeOffset] = _byte;	//capture the last byte
                LastTrans = 0;	//reset all static inter-call variables
                _mask = 0;
                _byte = 0;
                _lastSample = 0;
                TempIndex = 0;

                fixed (byte* p = CasBuffer)
                {
                    _modules.FileOperations.FileWriteFile(TapeHandle, p, (int) (TapeOffset));
                }
            }
        }

        public void SyncFileBufferWav()
        {
            uint formatSize = 16;		//size of WAVE section chunk
            ushort waveType = 1;		//WAVE type format
            ushort channels = 1;		//mono/stereo
            uint bitRate = Define.TAPEAUDIORATE;		//sample rate
            ushort bitsPerSample = 8;	//Bits/sample
            uint bytesPerSec = (uint)(bitRate * channels * (bitsPerSample / 8));		//bytes/sec
            ushort blockAlign = (ushort)((bitsPerSample * channels) / 8);		//Block alignment

            unsafe
            {
                uint fileSize = _totalSize + 40 - 8;
                uint chunkSize = fileSize;

                _modules.FileOperations.FileWriteFile(TapeHandle, "RIFF");
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)&fileSize, 4);

                _modules.FileOperations.FileWriteFile(TapeHandle, "WAVE");

                _modules.FileOperations.FileWriteFile(TapeHandle, "fmt ");
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&formatSize), 4);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&waveType), 2);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&channels), 2);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&bitRate), 4);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&bytesPerSec), 4);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&blockAlign), 2);
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&bitsPerSample), 2);

                _modules.FileOperations.FileWriteFile(TapeHandle, "data");
                _modules.FileOperations.FileWriteFile(TapeHandle, (byte*)(&chunkSize), 4);
            }
        }

        public unsafe void FlushCassetteWav(byte* buffer, uint length)
        {
            _modules.FileOperations.FileSetFilePointer(TapeHandle, Define.FILE_BEGIN, TapeOffset + 44);

            _modules.FileOperations.FileWriteFile(TapeHandle, buffer, (int)length);

            if (length != _bytesMoved)
            {
                return;
            }

            TapeOffset += length;

            if (TapeOffset > _totalSize)
            {
                _totalSize = TapeOffset;
            }
        }
    }
}
