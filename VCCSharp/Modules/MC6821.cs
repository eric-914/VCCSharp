using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    // ReSharper disable once InconsistentNaming
    public interface IMC6821
    {
        void PiaReset();
        void irq_fs(PhaseStates phase);
        void irq_hs(PhaseStates phase);
        void SetCartAutoStart(byte autoStart);
        void ClosePrintFile();
        void SetMonState(int state);
        void SetSerialParams(byte textMode);
        int OpenPrintFile(string filename);

        byte pia0_read(byte port);
        byte pia1_read(byte port);
        void pia0_write(byte data, byte port);
        void pia1_write(byte data, byte port);

        byte GetMuxState();
        byte DACState();
        uint GetDACSample();
        void SetCassetteSample(byte sample);
        byte GetCassetteSample();
    }

    // ReSharper disable once InconsistentNaming
    public class MC6821 : IMC6821
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        private byte _addLf;
        private int _monState = Define.FALSE;

        private HANDLE _hPrintFile;
        //private HANDLE _hOut;

        // ReSharper disable IdentifierTypo
        private readonly byte[] _regadd = { 0, 0, 0, 0 };
        private readonly byte[] _regbdd = { 0, 0, 0, 0 };
        private readonly byte[] _rega = { 0, 0, 0, 0 };
        private readonly byte[] _regb = { 0, 0, 0, 0 };
        // ReSharper restore IdentifierTypo

        private byte _aSample;
        private byte _sSample;
        private byte _cSample;

        private int _outLeft, _outRight, _lastLeft, _lastRight;

        public byte CartAutoStart;

        public MC6821(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public void SetCartAutoStart(byte autoStart)
        {
            CartAutoStart = autoStart;
        }

        public void irq_hs(PhaseStates phase) //63.5 uS
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
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
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
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                    }

                    break;

                case PhaseStates.Any:
                    _rega[1] = (byte)(_rega[1] | 128);

                    if ((_rega[1] & 1) != 0)
                    {
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                    }

                    break;
            }
        }

        public void irq_fs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
        {
            if (_modules.PAKInterface.CartInserted == 1 && CartAutoStart == 1)
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
                _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
            }
        }

        public void PiaReset()
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

        public void MC6821_AssertCart()
        {
            _regb[3] = (byte)(_regb[3] | 128);

            if ((_regb[3] & 1) != 0)
            {
                _modules.CPU.CPUAssertInterrupt(CPUInterrupts.FIRQ, 0);
            }
            else
            {
                _modules.CPU.CPUDeAssertInterrupt(CPUInterrupts.FIRQ); //Kludge but working
            }
        }

        public void ClosePrintFile()
        {
            _kernel.CloseHandle(_hPrintFile);

            _hPrintFile = Define.INVALID_HANDLE_VALUE;

            _kernel.FreeConsole();

            //_hOut = IntPtr.Zero;
        }

        public void SetMonState(int state)
        {
            if (_monState == Define.TRUE && state == Define.FALSE)
            {
                _kernel.FreeConsole();

                //_hOut = IntPtr.Zero;
            }

            _monState = state;
        }

        public byte pia0_read(byte port)
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

        public byte pia1_read(byte port)
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

        public void pia0_write(byte data, byte port)
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

        public void pia1_write(byte data, byte port)
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
                                _aSample = (byte)((_regb[0] & 0xFC) >> 1); //0 to 127
                            }
                            else
                            {
                                _cSample = (byte)(_regb[0] & 0xFC);
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

                        _modules.Graphics.SetGimeVdgMode2((byte)((_regb[2] & 248) >> 3));

                        _sSample = (byte)((_regb[port] & 2) << 6);
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

        private byte _bitMask = 1, _startWait = 1;

        public void MC6821_CaptureBit(byte sample)
        {
            unsafe
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

                    WritePrint(data);

                    if (_monState != 0)
                    {
                        MC6821_WritePrintMon(&data);
                    }

                    if ((data == 0x0D) && (_addLf != 0))
                    {
                        data = 0x0A;

                        WritePrint(data);
                    }

                    data = 0;
                }
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private void WritePrint(byte data)
        {
            //ulong bytesMoved = 0;

            //TODO: Writing to a print file?
            //WriteFile(instance->hPrintFile, &data, 1, &bytesMoved, NULL);
        }

        public byte GetMuxState()
        {
            return (byte)(((_rega[1] & 8) >> 3) + ((_rega[3] & 8) >> 2));
        }

        public unsafe void MC6821_WritePrintMon(byte* data)
        {
            WriteConsole(data);

            if (data[0] == 0x0D)
            {
                data[0] = 0x0A;

                WriteConsole(data);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private unsafe void WriteConsole(byte* data)
        {
            //ulong dummy = 0;

            //if (instance->hOut == IntPtr.Zero)
            {
                //AllocConsole();

                //instance->hOut = GetStdHandle(STD_OUTPUT_HANDLE);

                //SetConsoleTitle("Printer Monitor");
            }

            //TODO: Writing to a console?
            //WriteConsole(instance->hOut, data, 1, &dummy, 0);
        }

        public byte DACState()
        {
            return (byte)(_regb[0] >> 2);
        }

        public int OpenPrintFile(string filename)
        {
            _hPrintFile = _modules.FileOperations.FileCreateFile(filename, Define.GENERIC_READ | Define.GENERIC_WRITE);

            return _hPrintFile == Define.INVALID_HANDLE_VALUE ? 0 : 1;
        }

        public void SetSerialParams(byte textMode)
        {
            _addLf = textMode;
        }

        public uint GetDACSample()
        {
            int pakSample = _modules.PAKInterface.PakAudioSample();

            var sampleLeft = (pakSample >> 8) + _aSample + _sSample;
            var sampleRight = (pakSample & 0xFF) + _aSample + _sSample;

            sampleLeft = sampleLeft << 6;   //Convert to 16 bit values
            sampleRight = sampleRight << 6; //For Max volume

            if (sampleLeft == _lastLeft) //Simulate a slow high pass filter
            {
                if (_outLeft != 0)
                {
                    _outLeft--;
                }
            }
            else
            {
                _outLeft = sampleLeft;
                _lastLeft = sampleLeft;
            }

            if (sampleRight == _lastRight)
            {
                if (_outRight != 0)
                {
                    _outRight--;
                }
            }
            else
            {
                _outRight = sampleRight;
                _lastRight = sampleRight;
            }

            return (uint)((_outLeft << 16) + (_outRight));
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
            return _cSample;
        }
    }
}
