using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

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
    }

    // ReSharper disable once InconsistentNaming
    public class MC6821 : IMC6821
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;

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
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                mc6821State->CartAutoStart = autoStart;
            }
        }

        public void MC6821_irq_hs(PhaseStates phase) //63.5 uS
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                switch (phase)
                {
                    case PhaseStates.Falling:	//HS went High to low
                        if ((mc6821State->rega[1] & 2) != 0)
                        { //IRQ on low to High transition
                            return;
                        }

                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;

                    case PhaseStates.Rising:	//HS went Low to High
                        if ((mc6821State->rega[1] & 2) == 0)
                        { //IRQ  High to low transition
                            return;
                        }

                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;

                    case PhaseStates.Any:
                        mc6821State->rega[1] = (byte)(mc6821State->rega[1] | 128);

                        if ((mc6821State->rega[1] & 1) != 0)
                        {
                            _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                        }

                        break;
                }
            }
        }

        public void MC6821_irq_fs(PhaseStates phase) //60HZ Vertical sync pulse 16.667 mS
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                if (mc6821State->CartInserted == 1 && mc6821State->CartAutoStart == 1)
                {
                    MC6821_AssertCart();
                }

                switch (phase)
                {
                    case PhaseStates.Falling:	//FS went High to low
                        if ((mc6821State->rega[3] & 2) == 0) //IRQ on High to low transition
                        {
                            mc6821State->rega[3] = (byte)(mc6821State->rega[3] | 128);
                        }

                        break;

                    case PhaseStates.Rising:	//FS went Low to High
                        if ((mc6821State->rega[3] & 2) != 0) //IRQ  Low to High transition
                        {
                            mc6821State->rega[3] = (byte)(mc6821State->rega[3] | 128);
                        }

                        break;
                }

                if ((mc6821State->rega[3] & 1) != 0)
                {
                    _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 1);
                }
            }
        }

        public void MC6821_PiaReset()
        {
            unsafe
            {
                MC6821State* mc6821State = GetMC6821State();

                // Clear the PIA registers
                for (byte index = 0; index < 4; index++)
                {
                    mc6821State->rega[index] = 0;
                    mc6821State->regb[index] = 0;
                    mc6821State->rega_dd[index] = 0;
                    mc6821State->regb_dd[index] = 0;
                }
            }
        }

        public void MC6821_AssertCart()
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                instance->regb[3] = (byte)(instance->regb[3] | 128);

                if ((instance->regb[3] & 1) != 0)
                {
                    _modules.CPU.CPUAssertInterrupt(CPUInterrupts.FIRQ, 0);
                }
                else
                {
                    _modules.CPU.CPUDeAssertInterrupt(CPUInterrupts.FIRQ); //Kludge but working
                }
            }
        }

        public void MC6821_ClosePrintFile()
        {
            Library.MC6821.MC6821_ClosePrintFile();
        }

        public void MC6821_SetMonState(int state)
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                if (instance->MonState == Define.TRUE && state == Define.FALSE)
                {
                    _kernel.FreeConsole();

                    instance->hOut = IntPtr.Zero;
                }

                instance->MonState = state;
            }
        }

        public void MC6821_SetSerialParams(byte textMode)
        {
            Library.MC6821.MC6821_SetSerialParams(textMode);
        }

        public int MC6821_OpenPrintFile(string filename)
        {
            return Library.MC6821.MC6821_OpenPrintFile(filename);
        }

        public byte MC6821_pia0_read(byte port)
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                var dda = (byte)(instance->rega[1] & 4);
                var ddb = (byte)(instance->rega[3] & 4);

                switch (port)
                {
                    case 1:
                        return (instance->rega[port]);

                    case 3:
                        return (instance->rega[port]);

                    case 0:
                        if (dda != 0)
                        {
                            instance->rega[1] = (byte)(instance->rega[1] & 63);

                            return _modules.Keyboard.vccKeyboardGetScan(instance->rega[2]); //Read
                        }
                        else
                        {
                            return instance->rega_dd[port];
                        }

                    case 2: //Write 
                        if (ddb != 0)
                        {
                            instance->rega[3] = (byte)(instance->rega[3] & 63);

                            return (byte)(instance->rega[port] & instance->rega_dd[port]);
                        }
                        else
                        {
                            return instance->rega_dd[port];
                        }
                }
            }

            return 0;
        }

        public byte MC6821_pia1_read(byte port)
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                port -= 0x20;

                var dda = (byte)(instance->regb[1] & 4);
                var ddb = (byte)(instance->regb[3] & 4);

                switch (port)
                {
                    case 1:
                    //	return 0;

                    case 3:
                        return instance->regb[port];

                    case 2:
                        if (ddb != 0)
                        {
                            instance->regb[3] = (byte)(instance->regb[3] & 63);

                            return (byte)(instance->regb[port] & instance->regb_dd[port]);
                        }
                        else
                        {
                            return instance->regb_dd[port];
                        }

                    case 0:
                        if (dda != 0)
                        {
                            instance->regb[1] = (byte)(instance->regb[1] & 63); //Cass In
                            byte flag = instance->regb[port];

                            return flag;
                        }
                        else
                        {
                            return instance->regb_dd[port];
                        }
                }
            }

            return 0;
        }

        public void MC6821_pia0_write(byte data, byte port)
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                var dda = (byte)(instance->rega[1] & 4);
                var ddb = (byte)(instance->rega[3] & 4);

                switch (port)
                {
                    case 0:
                        if (dda != 0)
                        {
                            instance->rega[port] = data;
                        }
                        else
                        {
                            instance->rega_dd[port] = data;
                        }

                        return;

                    case 2:
                        if (ddb != 0)
                        {
                            instance->rega[port] = data;
                        }
                        else
                        {
                            instance->rega_dd[port] = data;
                        }

                        return;

                    case 1:
                        instance->rega[port] = (byte)(data & 0x3F);

                        return;

                    case 3:
                        instance->rega[port] = (byte)(data & 0x3F);

                        return;
                }
            }
        }

        public void MC6821_pia1_write(byte data, byte port)
        {
            unsafe
            {
                MC6821State* instance = GetMC6821State();

                port -= 0x20;

                var dda = (byte)(instance->regb[1] & 4);
                var ddb = (byte)(instance->regb[3] & 4);

                switch (port)
                {
                    case 0:
                        if (dda != 0)
                        {
                            instance->regb[port] = data;

                            MC6821_CaptureBit((byte)((instance->regb[0] & 2) >> 1));

                            if (MC6821_GetMuxState() == 0)
                            {
                                if ((instance->regb[3] & 8) != 0)
                                { //==0 for cassette writes
                                    instance->Asample = (byte)((instance->regb[0] & 0xFC) >> 1); //0 to 127
                                }
                                else
                                {
                                    instance->Csample = (byte)(instance->regb[0] & 0xFC);
                                }
                            }
                        }
                        else
                        {
                            instance->regb_dd[port] = data;
                        }

                        return;

                    case 2: //FF22
                        if (ddb != 0)
                        {
                            instance->regb[port] = (byte)(data & instance->regb_dd[port]);

                            _modules.Graphics.SetGimeVdgMode2((byte)((instance->regb[2] & 248) >> 3));

                            instance->Ssample = (byte)((instance->regb[port] & 2) << 6);
                        }
                        else
                        {
                            instance->regb_dd[port] = data;
                        }

                        return;

                    case 1:
                        instance->regb[port] = (byte)(data & 0x3F);

                        _modules.Cassette.Motor((byte)((data & 8) >> 3));

                        return;

                    case 3:
                        instance->regb[port] = (byte)(data & 0x3F);

                        return;
                }

            }
        }

        public void MC6821_CaptureBit(byte sample)
        {
            Library.MC6821.MC6821_CaptureBit(sample);
        }

        public byte MC6821_GetMuxState()
        {
            return Library.MC6821.MC6821_GetMuxState();
        }
    }
}
