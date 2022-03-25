﻿using System;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using static System.IntPtr;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules;

public interface ICassette : IModule
{
    HANDLE TapeHandle { get; set; }

    uint FlushCassetteBuffer(byte[] buffer, uint length);
    void LoadCassetteBuffer(byte[] buffer);
    void Motor(byte state);

    //void SetTapeCounter(uint count);
    //void SetTapeMode(byte mode);

    int MountTape(string filename);
    void CloseTapeFile();

    Action<int> UpdateTapeDialog { get; set; }

    byte MotorState { get; set; }
    TapeModes TapeMode { get; set; }
    string? TapeFileName { get; set; }
    uint TapeOffset { get; set; }
}

public class Cassette : ICassette
{
    private readonly IModules _modules;
    private readonly IConfigurationRoot _configuration;
    private readonly IKernel _kernel;

    public byte MotorState { get; set; }
    public TapeModes TapeMode { get; set; } = TapeModes.Stop;

    private readonly byte[] _one = { 0x80, 0xA8, 0xC8, 0xE8, 0xE8, 0xF8, 0xF8, 0xE8, 0xC8, 0xA8, 0x78, 0x50, 0x50, 0x30, 0x10, 0x00, 0x00, 0x10, 0x30, 0x30, 0x50 };
    private readonly byte[] _zero = { 0x80, 0x90, 0xA8, 0xB8, 0xC8, 0xD8, 0xE8, 0xE8, 0xF0, 0xF8, 0xF8, 0xF8, 0xF0, 0xE8, 0xD8, 0xC8, 0xB8, 0xA8, 0x90, 0x78, 0x78, 0x68, 0x50, 0x40, 0x30, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x08, 0x10, 0x10, 0x20, 0x30, 0x40, 0x50, 0x68, 0x68 };

    public string? TapeFileName { get; set; } //[Define.MAX_PATH];
    public uint TapeOffset { get; set; }

    public byte FileType;

    private byte _quiet = 30;

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

    public Cassette(IModules modules, IConfigurationRoot configuration, IKernel kernel)
    {
        _modules = modules;
        _configuration = configuration;
        _kernel = kernel;

        UpdateTapeDialog = _ => { }; //_modules.ConfigurationManager.UpdateTapeDialog((uint) offset);
    }

    public void LoadCassetteBuffer(byte[] buffer)
    {
        uint bytesMoved = 0;

        if (TapeMode != TapeModes.Play)
        {
            return;
        }

        switch ((TapeFileType)(FileType))
        {
            case TapeFileType.WAV:
                LoadCassetteBufferWav(buffer, ref bytesMoved);
                break;

            case TapeFileType.CAS:
                LoadCassetteBufferCas(buffer, ref bytesMoved);
                break;
        }

        UpdateTapeDialog((int)TapeOffset);
    }

    public void LoadCassetteBufferCas(byte[] buffer, ref uint bytesMoved)
    {
        CasToWav(buffer, Define.TAPEAUDIORATE / 60, ref bytesMoved);
    }

    public void CasToWav(byte[] buffer, ushort bytesToConvert, ref uint bytesConverted)
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

            TapeMode = (byte)TapeModes.Stop; //Stop at end of tape

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
    private void LoadCassetteBufferWav(byte[] buffer, ref uint bytesMoved)
    {
        _kernel.SetFilePointer(TapeHandle, Define.FILE_BEGIN, TapeOffset + 44);
        _kernel.ReadFile(TapeHandle, buffer, Define.TAPEAUDIORATE / 60, ref bytesMoved, IntPtr.Zero);

        TapeOffset += bytesMoved;

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
                    case TapeModes.Stop:
                        break;

                    case TapeModes.Play:
                        _quiet = 30;
                        TempIndex = 0;
                        break;

                    case TapeModes.Record:
                        SyncFileBuffer();
                        break;

                    case TapeModes.Eject:
                        break;
                }

                break;

            case 1:
                switch (TapeMode)
                {
                    case TapeModes.Stop:
                        _modules.CoCo.SetSndOutMode(0);
                        break;

                    case TapeModes.Play:
                        _modules.CoCo.SetSndOutMode(2);
                        break;

                    case TapeModes.Record:
                        _modules.CoCo.SetSndOutMode(1);
                        break;

                    case TapeModes.Eject:
                        _modules.CoCo.SetSndOutMode(0);
                        break;
                }

                break;
        }
    }

    //public void SetTapeCounter(uint count)
    //{
    //    TapeOffset = count;

    //    if (TapeOffset > _totalSize)
    //    {
    //        _totalSize = TapeOffset;
    //    }

    //    UpdateTapeDialog((int)TapeOffset);
    //}

    public void CloseTapeFile()
    {
        if (TapeHandle == Zero)
        {
            return;
        }

        SyncFileBuffer();

        _kernel.CloseHandle(TapeHandle);

        TapeHandle = Zero;
        _totalSize = 0;
    }

    public uint FlushCassetteBuffer(byte[] buffer, uint length)
    {
        if (TapeMode == TapeModes.Record)
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

    public void FlushCassetteCas(byte[] buffer, uint length)
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
                            TapeMode = TapeModes.Stop;
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
    public int MountTape(string filename)
    {
        if (TapeHandle != Zero)
        {
            TapeMode = TapeModes.Stop;

            CloseTapeFile();
        }

        //_writeProtect = 0;
        FileType = 0; //0=wav 1=cas

        TapeHandle = _kernel.CreateFile(filename, Define.GENERIC_READ | Define.GENERIC_WRITE, Define.OPEN_ALWAYS);

        if (TapeHandle == Define.INVALID_HANDLE_VALUE) //Can't open read/write. try read only
        {
            TapeHandle = _kernel.CreateFile(filename, Define.GENERIC_READ, Define.OPEN_ALWAYS);
            //_writeProtect = 1;
        }

        if (TapeHandle == Define.INVALID_HANDLE_VALUE)
        {
            MessageBox.Show("Can't Mount", "Error");

            return 0; //Give up
        }

        _totalSize = _kernel.SetFilePointer(TapeHandle, Define.FILE_END);
        TapeOffset = 0;

        var extension = Path.GetExtension(filename).ToUpper();

        if (extension == ".CAS")
        {
            FileType = Define.CAS;
            LastTrans = 0;
            _mask = 0;
            _byte = 0;
            _lastSample = 0;
            TempIndex = 0;

            CasBuffer = new byte[Define.WRITEBUFFERSIZE];

            _kernel.SetFilePointer(TapeHandle, Define.FILE_BEGIN);

            ulong moved = 0;

            //Read the whole file in for .CAS files
            _kernel.ReadFile(TapeHandle, CasBuffer, _totalSize, ref moved);

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
        _kernel.SetFilePointer(TapeHandle, Define.FILE_BEGIN);

        switch (FileType)
        {
            case Define.CAS:
                SyncFileBufferCas();
                break;

            case Define.WAV:
                SyncFileBufferWav();
                break;
        }

        _kernel.FlushFileBuffers(TapeHandle);
    }

    public void SyncFileBufferCas()
    {
        CasBuffer[TapeOffset] = _byte;	//capture the last byte
        LastTrans = 0;	//reset all static inter-call variables
        _mask = 0;
        _byte = 0;
        _lastSample = 0;
        TempIndex = 0;

        _kernel.WriteFile(TapeHandle, CasBuffer, TapeOffset);
    }

    public void SyncFileBufferWav()
    {
        uint formatSize = 16;		//size of WAVE section chunk
        ushort waveType = 1;		//WAVE type format
        ushort channels = 1;		//mono/stereo
        uint bitRate = Define.TAPEAUDIORATE;		//sample rate
        ushort bitsPerSample = 8;	//Bits/sample

        // ReSharper disable UselessBinaryOperation
        uint bytesPerSec = (uint)(bitRate * channels * (bitsPerSample / 8));	//bytes/sec
        ushort blockAlign = (ushort)((bitsPerSample * channels) / 8);		    //Block alignment
        // ReSharper restore UselessBinaryOperation

        uint fileSize = _totalSize + 40 - 8;
        uint chunkSize = fileSize;

        _kernel.WriteFile(TapeHandle, "RIFF", TapeOffset);
        _kernel.WriteFile(TapeHandle, fileSize);

        _kernel.WriteFile(TapeHandle, "WAVE", TapeOffset);

        _kernel.WriteFile(TapeHandle, "fmt ", TapeOffset);
        _kernel.WriteFile(TapeHandle, formatSize);
        _kernel.WriteFile(TapeHandle, waveType);
        _kernel.WriteFile(TapeHandle, channels);
        _kernel.WriteFile(TapeHandle, bitRate);
        _kernel.WriteFile(TapeHandle, bytesPerSec);
        _kernel.WriteFile(TapeHandle, blockAlign);
        _kernel.WriteFile(TapeHandle, bitsPerSample);

        _kernel.WriteFile(TapeHandle, "data", TapeOffset);
        _kernel.WriteFile(TapeHandle, chunkSize);
    }

    private void FlushCassetteWav(byte[] buffer, uint length)
    {
        _kernel.SetFilePointer(TapeHandle, Define.FILE_BEGIN, TapeOffset + 44);

        _kernel.WriteFile(TapeHandle, buffer, length);

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

    public void Reset()
    {
        MotorState = 0;
        TapeFileName = _configuration.CassetteRecorder.TapeFileName;
        TapeMode = _configuration.CassetteRecorder.TapeMode;

        //TODO: This may be a problem.
        TapeOffset = 0; //(uint)_configuration.CassetteRecorder.TapeCounter;

        UpdateTapeDialog = _ => { };

        FileType = 0;

        _quiet = 30;

        LastTrans = 0;

        _byte = 0;
        _lastSample = 0;
        _mask = 0;

        _bytesMoved = 0;
        _totalSize = 0;
    }
}
