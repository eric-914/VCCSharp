using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Audio;
using VCCSharp.Models.Configuration;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules;

// ReSharper disable once InconsistentNaming
public interface IMC6821 : IModule, IChip
{
    void IrqFs(PhaseStates phase);
    void IrqHs(PhaseStates phase);
    void ClosePrintFile();
    void SetMonState(bool state);
    void SetSerialParams(bool textMode);
    int OpenPrintFile(string filename);

    byte Pia0_Read(byte port);
    byte Pia1_Read(byte port);
    void Pia0_Write(byte data, byte port);
    void Pia1_Write(byte data, byte port);

    //TODO: Probably should be returning Pots enum
    byte GetMuxState();
    byte DACState();
    void GetDACSample(DACSample dacSample);
    void SetCassetteSample(byte sample);
    byte GetCassetteSample();
}

/// <summary>
/// PERIPHERAL INTERFACE ADAPTER
/// </summary>
// ReSharper disable once InconsistentNaming
public class MC6821 : IMC6821
{
    private readonly IModules _modules;
    private readonly IKernel _kernel;

    private IConfiguration Configuration => _modules.Configuration;

    private bool _addLf;
    private bool _monState;

    private HANDLE _hPrintFile;
    //private HANDLE _hOut;

    // ReSharper disable IdentifierTypo
    private readonly byte[] _regadd = { 0, 0, 0, 0 };
    private readonly byte[] _regbdd = { 0, 0, 0, 0 };
    private readonly byte[] _rega = { 0, 0, 0, 0 };
    private readonly byte[] _regb = { 0, 0, 0, 0 };
    // ReSharper restore IdentifierTypo

    private byte _audioSample;
    private byte _singleBitSample;
    private byte _cassetteSample;
    private byte _bitMask = 1;
    private byte _startWait = 1;

    private bool _cartAutoStart;

    public MC6821(IModules modules, IKernel kernel)
    {
        _modules = modules;
        _kernel = kernel;
    }

    private void SetCartAutoStart()
    {
        _cartAutoStart = Configuration.Startup.CartridgeAutoStart;
    }

    public void IrqHs(PhaseStates phase) //63.5 uS
    {
        switch (phase)
        {
            case PhaseStates.Falling:	//HS went High to low
                if ((_rega[1] & 2) != 0)
                { //IRQ on low to High transition
                    return;
                }

                _rega[1] = (byte)(_rega[1] | 128);

                if ((_rega[1] & 1) != 0)
                {
                    _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 1);
                }

                break;

            case PhaseStates.Rising:	//HS went Low to High
                if ((_rega[1] & 2) == 0)
                {
                    //IRQ  High to low transition
                    return;
                }

                _rega[1] = (byte)(_rega[1] | 128);

                if ((_rega[1] & 1) != 0)
                {
                    _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 1);
                }

                break;

            case PhaseStates.Any:
                _rega[1] = (byte)(_rega[1] | 128);

                if ((_rega[1] & 1) != 0)
                {
                    _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 1);
                }

                break;
        }
    }

    public void IrqFs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
    {
        if (_modules.PAKInterface.CartInserted == 1 && _cartAutoStart)
        {
            MC6821_AssertCart();
        }

        switch (phase)
        {
            case PhaseStates.Falling:	//FS went High to low
                if ((_rega[3] & 2) == 0) //IRQ on High to low transition
                {
                    _rega[3] = (byte)(_rega[3] | 128);
                }

                break;

            case PhaseStates.Rising:	//FS went Low to High
                if ((_rega[3] & 2) != 0) //IRQ  Low to High transition
                {
                    _rega[3] = (byte)(_rega[3] | 128);
                }

                break;
        }

        if ((_rega[3] & 1) != 0)
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.IRQ, 1);
        }
    }

    private void MC6821_AssertCart()
    {
        _regb[3] = (byte)(_regb[3] | 128);

        if ((_regb[3] & 1) != 0)
        {
            _modules.CPU.AssertInterrupt(CPUInterrupts.FIRQ, 0);
        }
        else
        {
            _modules.CPU.DeAssertInterrupt(CPUInterrupts.FIRQ); //Kludge but working
        }
    }

    public void ClosePrintFile()
    {
        _kernel.CloseHandle(_hPrintFile);

        _hPrintFile = Define.INVALID_HANDLE_VALUE;

        _kernel.FreeConsole();

        //_hOut = IntPtr.Zero;
    }

    public void SetMonState(bool state)
    {
        if (_monState && state)
        {
            _kernel.FreeConsole();

            //_hOut = IntPtr.Zero;
        }

        _monState = state;
    }

    public byte Pia0_Read(byte port)
    {
        var dda = (byte)(_rega[1] & 4);
        var ddb = (byte)(_rega[3] & 4);

        switch (port)
        {
            case 1:
                return (_rega[port]);

            case 3:
                return (_rega[port]);

            case 0:
                if (dda != 0)
                {
                    _rega[1] = (byte)(_rega[1] & 63);

                    return _modules.Keyboard.KeyboardGetScan(_rega[2]); //Read
                }
                else
                {
                    return _regadd[port];
                }

            case 2: //WritePrint 
                if (ddb != 0)
                {
                    _rega[3] = (byte)(_rega[3] & 63);

                    return (byte)(_rega[port] & _regadd[port]);
                }
                else
                {
                    return _regadd[port];
                }
        }

        return 0;
    }

    public byte Pia1_Read(byte port)
    {
        port -= 0x20;

        var dda = (byte)(_regb[1] & 4);
        var ddb = (byte)(_regb[3] & 4);

        switch (port)
        {
            case 1:
            //	return 0;

            case 3:
                return _regb[port];

            case 2:
                if (ddb != 0)
                {
                    _regb[3] = (byte)(_regb[3] & 63);

                    return (byte)(_regb[port] & _regbdd[port]);
                }
                else
                {
                    return _regbdd[port];
                }

            case 0:
                if (dda != 0)
                {
                    _regb[1] = (byte)(_regb[1] & 63); //Cassette In
                    byte flag = _regb[port];

                    return flag;
                }
                else
                {
                    return _regbdd[port];
                }
        }

        return 0;
    }

    public void Pia0_Write(byte data, byte port)
    {
        var dda = (byte)(_rega[1] & 4);
        var ddb = (byte)(_rega[3] & 4);

        switch (port)
        {
            case 0:
                if (dda != 0)
                {
                    _rega[port] = data;
                }
                else
                {
                    _regadd[port] = data;
                }

                return;

            case 2:
                if (ddb != 0)
                {
                    _rega[port] = data;
                }
                else
                {
                    _regadd[port] = data;
                }

                return;

            case 1:
                _rega[port] = (byte)(data & 0x3F);

                return;

            case 3:
                _rega[port] = (byte)(data & 0x3F);

                return;
        }
    }

    public void Pia1_Write(byte data, byte port)
    {
        port -= 0x20;

        var dda = (byte)(_regb[1] & 4);
        var ddb = (byte)(_regb[3] & 4);

        switch (port)
        {
            case 0:
                if (dda != 0)
                {
                    _regb[port] = data;

                    MC6821_CaptureBit((byte)((_regb[0] & 2) >> 1));

                    if (GetMuxState() == 0)
                    {
                        if ((_regb[3] & 8) != 0)
                        { //==0 for cassette writes
                            _audioSample = (byte)((_regb[0] & 0xFC) >> 1); //0 to 127
                        }
                        else
                        {
                            _cassetteSample = (byte)(_regb[0] & 0xFC);
                        }
                    }
                }
                else
                {
                    _regbdd[port] = data;
                }

                return;

            case 2: //FF22
                if (ddb != 0)
                {
                    _regb[port] = (byte)(data & _regbdd[port]);

                    _singleBitSample = (byte)((_regb[port] & 2) << 6);

                    _modules.Graphics.SetGimeVdgMode2((byte)((_regb[2] & 248) >> 3));
                }
                else
                {
                    _regbdd[port] = data;
                }

                return;

            case 1:
                _regb[port] = (byte)(data & 0x3F);

                _modules.Cassette.Motor((byte)((data & 8) >> 3));

                return;

            case 3:
                _regb[port] = (byte)(data & 0x3F);

                return;
        }
    }

    private void MC6821_CaptureBit(byte sample)
    {
        byte data = 0;

        if ((long)(_hPrintFile) == -1)
        { //INVALID_HANDLE_VALUE
            return;
        }

        if ((_startWait & sample) != 0)
        { //Waiting for start bit
            return;
        }

        if (_startWait != 0)
        {
            _startWait = 0;

            return;
        }

        if (sample != 0)
        {
            data |= _bitMask;
        }

        _bitMask = (byte)(_bitMask << 1);

        if (_bitMask == 0)
        {
            _bitMask = 1;
            _startWait = 1;

            //WritePrint(data);

            if (_monState)
            {
                MC6821_WritePrintMon(data);
            }

            if (data == 0x0D && _addLf)
            {
                //data = 0x0A;

                //WritePrint(data);
            }
        }
    }

    //// ReSharper disable once UnusedParameter.Local
    //private void WritePrint(byte data)
    //{
    //    //ulong bytesMoved = 0;

    //    //TODO: Writing to a print file?
    //    //WriteFile(instance->hPrintFile, &data, 1, &bytesMoved, NULL);
    //}

    public byte GetMuxState()
    {
        return (byte)(((_rega[1] & 8) >> 3) + ((_rega[3] & 8) >> 2));
    }

    private void MC6821_WritePrintMon(byte data)
    {
        //WriteConsole(data);

        if (data == 0x0D)
        {
            data = 0x0A;

            //WriteConsole(data);
        }
    }

    //// ReSharper disable once UnusedParameter.Local
    //private un/safe void WriteConsole(byte* data)
    //{
    //    //ulong dummy = 0;

    //    //if (instance->hOut == IntPtr.Zero)
    //    {
    //        //AllocConsole();

    //        //instance->hOut = GetStdHandle(STD_OUTPUT_HANDLE);

    //        //SetConsoleTitle("Printer Monitor");
    //    }

    //    //TODO: Writing to a console?
    //    //WriteConsole(instance->hOut, data, 1, &dummy, 0);
    //}

    public byte DACState()
    {
        return (byte)(_regb[0] >> 2);
    }

    public int OpenPrintFile(string filename)
    {
        _hPrintFile = _kernel.CreateFile(filename, Define.GENERIC_READ | Define.GENERIC_WRITE, Define.CREATE_ALWAYS);

        return _hPrintFile == Define.INVALID_HANDLE_VALUE ? 0 : 1;
    }

    public void SetSerialParams(bool textMode)
    {
        _addLf = textMode;
    }

    public void GetDACSample(DACSample dacSample)
    {
        int pakSample = _modules.PAKInterface.PakAudioSample();

        dacSample.Left.Sample(pakSample >> 8, _audioSample, _singleBitSample);
        dacSample.Right.Sample(pakSample & 0xFF, _audioSample, _singleBitSample);
    }

    public void SetCassetteSample(byte sample)
    {
        _regb[0] &= 0xFE;

        if (sample > 0x7F)
        {
            _regb[0] |= 1;
        }
    }

    public byte GetCassetteSample()
    {
        return _cassetteSample;
    }

    public void ModuleReset()
    {
        Debug.WriteLine("MC6821.ModuleReset()");

        _addLf = false;
        _monState = false;

        _hPrintFile = HANDLE.Zero;

        _regadd.Initialize();
        _regbdd.Initialize();
        _rega.Initialize();
        _regb.Initialize();

        _audioSample = 0;
        _singleBitSample = 0;
        _cassetteSample = 0;

        _cartAutoStart = false;

        SetCartAutoStart();
    }

    public void ChipReset()
    {
        // Clear the PIA registers
        for (byte index = 0; index < 4; index++)
        {
            _rega[index] = 0;
            _regb[index] = 0;
            _regadd[index] = 0;
            _regbdd[index] = 0;
        }
    }
}
