using System;
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
        unsafe MC6821State* GetMC6821State();
        void MC6821_PiaReset();
        void MC6821_irq_fs(PhaseStates phase);
        void MC6821_irq_hs(PhaseStates phase);
        void MC6821_SetCartAutoStart(byte autoStart);
        void MC6821_ClosePrintFile();
        void MC6821_SetMonState(int state);
        void MC6821_SetSerialParams(byte textMode);
        int MC6821_OpenPrintFile(string filename);

        byte MC6821_pia0_read(byte port);
        byte MC6821_pia1_read(byte port);
        void MC6821_pia0_write(byte data, byte port);
        void MC6821_pia1_write(byte data, byte port);

        byte MC6821_GetMuxState();
        byte MC6821_DACState();
        uint MC6821_GetDACSample();
        void MC6821_SetCassetteSample(byte sample);
        byte MC6821_GetCasSample();
    }

    // ReSharper disable once InconsistentNaming
    public class MC6821 : IMC6821
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

        private byte _addLf;
        private int _monState = Define.FALSE;

        private HANDLE _hPrintFile;
        private HANDLE _hOut;

        public byte[] rega_dd = { 0, 0, 0, 0 };
        public byte[] regb_dd = { 0, 0, 0, 0 };
        public byte[] rega = { 0, 0, 0, 0 };
        public byte[] regb = { 0, 0, 0, 0 };

        private byte _aSample;
        private byte _sSample;
        private byte _cSample;

        public byte CartAutoStart;

        public MC6821(IModules modules, IKernel kernel)
        {
            _modules = modules;
            _kernel = kernel;
        }

        public unsafe MC6821State* GetMC6821State()
        {
            return Library.MC6821.GetMC6821State();
        }

        public void MC6821_SetCartAutoStart(byte autoStart)
        {
            CartAutoStart = autoStart;
        }

        public void MC6821_irq_hs(PhaseStates phase) //63.5 uS
        {
            switch (phase)
            {
                case PhaseStates.Falling:	//HS went High to low
                    if ((rega[1] & 2) != 0)
                    { //IRQ on low to High transition
                        return;
                    }

                    rega[1] = (byte)(rega[1] | 128);

                    if ((rega[1] & 1) != 0)
                    {
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                    }

                    break;

                case PhaseStates.Rising:	//HS went Low to High
                    if ((rega[1] & 2) == 0)
                    { //IRQ  High to low transition
                        return;
                    }

                    rega[1] = (byte)(rega[1] | 128);

                    if ((rega[1] & 1) != 0)
                    {
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                    }

                    break;

                case PhaseStates.Any:
                    rega[1] = (byte)(rega[1] | 128);

                    if ((rega[1] & 1) != 0)
                    {
                        _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                    }

                    break;
            }
        }

        public void MC6821_irq_fs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                if (mc6821State->CartInserted == 1 && CartAutoStart == 1)
                {
                    MC6821_AssertCart();
                }

                switch (phase)
                {
                    case PhaseStates.Falling:	//FS went High to low
                        if ((rega[3] & 2) == 0) //IRQ on High to low transition
                        {
                            rega[3] = (byte)(rega[3] | 128);
                        }

                        break;

                    case PhaseStates.Rising:	//FS went Low to High
                        if ((rega[3] & 2) != 0) //IRQ  Low to High transition
                        {
                            rega[3] = (byte)(rega[3] | 128);
                        }

                        break;
                }

                if ((rega[3] & 1) != 0)
                {
                    _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                }
            }
        }

        public void MC6821_PiaReset()
        {
            // Clear the PIA registers
            for (byte index = 0; index < 4; index++)
            {
                rega[index] = 0;
                regb[index] = 0;
                rega_dd[index] = 0;
                regb_dd[index] = 0;
            }
        }

        public void MC6821_AssertCart()
        {
            regb[3] = (byte)(regb[3] | 128);

            if ((regb[3] & 1) != 0)
            {
                _modules.CPU.CPUAssertInterrupt(CPUInterrupts.FIRQ, 0);
            }
            else
            {
                _modules.CPU.CPUDeAssertInterrupt(CPUInterrupts.FIRQ); //Kludge but working
            }
        }

        public void MC6821_ClosePrintFile()
        {
            _kernel.CloseHandle(_hPrintFile);

            _hPrintFile = Define.INVALID_HANDLE_VALUE;

            _kernel.FreeConsole();

            _hOut = IntPtr.Zero;
        }

        public void MC6821_SetMonState(int state)
        {
            if (_monState == Define.TRUE && state == Define.FALSE)
            {
                _kernel.FreeConsole();

                _hOut = IntPtr.Zero;
            }

            _monState = state;
        }

        public byte MC6821_pia0_read(byte port)
        {
            var dda = (byte)(rega[1] & 4);
            var ddb = (byte)(rega[3] & 4);

            switch (port)
            {
                case 1:
                    return (rega[port]);

                case 3:
                    return (rega[port]);

                case 0:
                    if (dda != 0)
                    {
                        rega[1] = (byte)(rega[1] & 63);

                        return _modules.Keyboard.KeyboardGetScan(rega[2]); //Read
                    }
                    else
                    {
                        return rega_dd[port];
                    }

                case 2: //WritePrint 
                    if (ddb != 0)
                    {
                        rega[3] = (byte)(rega[3] & 63);

                        return (byte)(rega[port] & rega_dd[port]);
                    }
                    else
                    {
                        return rega_dd[port];
                    }
            }

            return 0;
        }

        public byte MC6821_pia1_read(byte port)
        {
            port -= 0x20;

            var dda = (byte)(regb[1] & 4);
            var ddb = (byte)(regb[3] & 4);

            switch (port)
            {
                case 1:
                //	return 0;

                case 3:
                    return regb[port];

                case 2:
                    if (ddb != 0)
                    {
                        regb[3] = (byte)(regb[3] & 63);

                        return (byte)(regb[port] & regb_dd[port]);
                    }
                    else
                    {
                        return regb_dd[port];
                    }

                case 0:
                    if (dda != 0)
                    {
                        regb[1] = (byte)(regb[1] & 63); //Cass In
                        byte flag = regb[port];

                        return flag;
                    }
                    else
                    {
                        return regb_dd[port];
                    }
            }

            return 0;
        }

        public void MC6821_pia0_write(byte data, byte port)
        {
            var dda = (byte)(rega[1] & 4);
            var ddb = (byte)(rega[3] & 4);

            switch (port)
            {
                case 0:
                    if (dda != 0)
                    {
                        rega[port] = data;
                    }
                    else
                    {
                        rega_dd[port] = data;
                    }

                    return;

                case 2:
                    if (ddb != 0)
                    {
                        rega[port] = data;
                    }
                    else
                    {
                        rega_dd[port] = data;
                    }

                    return;

                case 1:
                    rega[port] = (byte)(data & 0x3F);

                    return;

                case 3:
                    rega[port] = (byte)(data & 0x3F);

                    return;
            }
        }

        public void MC6821_pia1_write(byte data, byte port)
        {
            port -= 0x20;

            var dda = (byte)(regb[1] & 4);
            var ddb = (byte)(regb[3] & 4);

            switch (port)
            {
                case 0:
                    if (dda != 0)
                    {
                        regb[port] = data;

                        MC6821_CaptureBit((byte)((regb[0] & 2) >> 1));

                        if (MC6821_GetMuxState() == 0)
                        {
                            if ((regb[3] & 8) != 0)
                            { //==0 for cassette writes
                                _aSample = (byte)((regb[0] & 0xFC) >> 1); //0 to 127
                            }
                            else
                            {
                                _cSample = (byte)(regb[0] & 0xFC);
                            }
                        }
                    }
                    else
                    {
                        regb_dd[port] = data;
                    }

                    return;

                case 2: //FF22
                    if (ddb != 0)
                    {
                        regb[port] = (byte)(data & regb_dd[port]);

                        _modules.Graphics.SetGimeVdgMode2((byte)((regb[2] & 248) >> 3));

                        _sSample = (byte)((regb[port] & 2) << 6);
                    }
                    else
                    {
                        regb_dd[port] = data;
                    }

                    return;

                case 1:
                    regb[port] = (byte)(data & 0x3F);

                    _modules.Cassette.Motor((byte)((data & 8) >> 3));

                    return;

                case 3:
                    regb[port] = (byte)(data & 0x3F);

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

        private void WritePrint(byte data)
        {
            //ulong bytesMoved = 0;

            //TODO: Writing to a print file?
            //WriteFile(instance->hPrintFile, &data, 1, &bytesMoved, NULL);
        }

        public byte MC6821_GetMuxState()
        {
            return (byte)(((rega[1] & 8) >> 3) + ((rega[3] & 8) >> 2));
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

        public byte MC6821_DACState()
        {
            return (byte)(regb[0] >> 2);
        }

        public int MC6821_OpenPrintFile(string filename)
        {
            _hPrintFile = Library.MC6821.MC6821_OpenPrintFile(filename);

            return _hPrintFile == Define.INVALID_HANDLE_VALUE ? 0 : 1;
        }

        public void MC6821_SetSerialParams(byte textMode)
        {
            _addLf = textMode;
        }

        int outLeft, outRight, lastLeft, lastRight;

        public uint MC6821_GetDACSample()
        {
            int pakSample = _modules.PAKInterface.PakAudioSample();

            var sampleLeft = (pakSample >> 8) + _aSample + _sSample;
            var sampleRight = (pakSample & 0xFF) + _aSample + _sSample;

            sampleLeft = sampleLeft << 6;   //Convert to 16 bit values
            sampleRight = sampleRight << 6; //For Max volume

            if (sampleLeft == lastLeft) //Simulate a slow high pass filter
            {
                if (outLeft != 0)
                {
                    outLeft--;
                }
            }
            else
            {
                outLeft = sampleLeft;
                lastLeft = sampleLeft;
            }

            if (sampleRight == lastRight)
            {
                if (outRight != 0)
                {
                    outRight--;
                }
            }
            else
            {
                outRight = sampleRight;
                lastRight = sampleRight;
            }

            return (uint)((outLeft << 16) + (outRight));
        }

        public void MC6821_SetCassetteSample(byte sample)
        {
            regb[0] &= 0xFE;

            if (sample > 0x7F)
            {
                regb[0] |= 1;
            }
        }

        public byte MC6821_GetCasSample()
        {
            return _cSample;
        }
    }
}
