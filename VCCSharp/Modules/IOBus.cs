using VCCSharp.IoC;
using VCCSharp.Modules.TCC1014;

namespace VCCSharp.Modules;

// ReSharper disable once InconsistentNaming
public interface IIOBus : IModule
{
    byte PortRead(ushort address);
    void PortWrite(byte data, ushort address);
}

// ReSharper disable once InconsistentNaming
public class IOBus : IIOBus
{
    private readonly IModules _modules;

    public IOBus(IModules modules)
    {
        _modules = modules;
    }

    public byte PortRead(ushort address)
    {
        //0xFF00 ≤ address ≤ 0xFFFF

        byte port = (byte)(address & 0xFF);
        var target = Ports.Handler(port);

        switch (target)
        {
            case PortHandlers.PIA0:
                return _modules.MC6821.Pia0_Read(port);
            case PortHandlers.PIA1:
                return _modules.MC6821.Pia1_Read(port);
            case PortHandlers.GIME:
                return _modules.TCC1014.GimeRead(port);
            case PortHandlers.SAM:
                return _modules.TCC1014.SAMRead(port);
            case PortHandlers.VECT:
                return _modules.TCC1014.VectorRead(port);
            default:
                return _modules.PAKInterface.PakPortRead(port);
        }

        #region OLD
        //switch (port)
        //{
        //    #region PIA0
        //    case Ports.PIA_0_0:
        //    case Ports.PIA_0_1:
        //    case Ports.PIA_0_2:
        //    case Ports.PIA_0_3:
        //        return _modules.MC6821.Pia0_Read(port); //MC6821 P.I.A  Keyboard access $FF00-$FF03
        //    #endregion

        //    #region PIA1
        //    case Ports.PIA_1_0:
        //    case Ports.PIA_1_1:
        //    case Ports.PIA_1_2:
        //    case Ports.PIA_1_3:
        //        return _modules.MC6821.Pia1_Read(port); //MC6821 P.I.A	Sound and VDG Control
        //    #endregion

        //    #region GIME
        //    //TCC1014 G.I.M.E. (IC6)

        //    //Chip Control Registers
        //    case Ports.INITO:
        //    case Ports.INIT1:
        //    case Ports.IRQENR:
        //    case Ports.FIRQENR:
        //    case Ports.TIMER_MS:
        //    case Ports.TIMER_LS:
        //    case Ports.RESERVED_96:
        //    case Ports.RESERVED_97:
        //    case Ports.VideoModeRegister:
        //    case Ports.VideoResolutionRegister:
        //    case Ports.BorderRegister:
        //    case Ports.RESERVED_9B:
        //    case Ports.VerticalScrollRegister:
        //    case Ports.VerticalOffset1Register:
        //    case Ports.VerticalOffset0Register:
        //    case Ports.HorizontalOffsetRegister:

        //    //Memory Management Unit (MMU)
        //    //$FFAO - $FFAF, 6 bits (Write only)
        //    case 0xA0:
        //    case 0xA1:
        //    case 0xA2:
        //    case 0xA3:
        //    case 0xA4:
        //    case 0xA5:
        //    case 0xA6:
        //    case 0xA7:
        //    case 0xA8:
        //    case 0xA9:
        //    case 0xAA:
        //    case 0xAB:
        //    case 0xAC:
        //    case 0xAD:
        //    case 0xAE:
        //    case 0xAF:

        //    // COLOR PALETTE
        //    // $FFBO - $FFBF: 16 addresses, 6 bits each
        //    case 0xB0:  // (Green/12)
        //    case 0xB1:  // (Yellow/36)
        //    case 0xB2:  // (Blue/09)
        //    case 0xB3:  // (Red/24)
        //    case 0xB4:  // (Buff/3F)
        //    case 0xB5:  // (Cyan/10)
        //    case 0xB6:  // (Magenta/2D)
        //    case 0xB7:  // (Orange/26)
        //    case 0xB8:  // (Black/00)
        //    case 0xB9:  // (Green/12)
        //    case 0xBA:  // (Black/00)
        //    case 0xBB:  // (Buff/3F)
        //    case 0xBC:  // (Black/00)
        //    case 0xBD:  // (Green/12)
        //    case 0xBE:  // (Black/00)
        //    case 0xBF:  // (Orange/26)
        //        return _modules.TCC1014.GimeRead(port);
        //    #endregion

        //    #region SAM
        //    // SAM CONTROL REGISTERS
        //    // $FFCO - $FFDF
        //    case 0xC0: // V0 - Display Model Control - Clear
        //    case 0xC1: // V0 - Display Model Control - Set
        //    case 0xC2: // V1 - Display Model Control - Clear
        //    case 0xC3: // V1 - Display Model Control - Set
        //    case 0xC4: // V2 - Display Model Control - Clear
        //    case 0xC5: // V2 - Display Model Control - Set

        //    case 0xC6: // F0 - Display Offset - Clear
        //    case 0xC7: // F0 - Display Offset - Set
        //    case 0xC8: // F1 - Display Offset - Clear
        //    case 0xC9: // F1 - Display Offset - Set
        //    case 0xCA: // F2 - Display Offset - Clear
        //    case 0xCB: // F2 - Display Offset - Set
        //    case 0xCC: // F3 - Display Offset - Clear
        //    case 0xCD: // F3 - Display Offset - Set
        //    case 0xCE: // F4 - Display Offset - Clear
        //    case 0xCF: // F4 - Display Offset - Set
        //    case 0xD0: // F5 - Display Offset - Clear
        //    case 0xD1: // F5 - Display Offset - Set
        //    case 0xD2: // F6 - Display Offset - Clear
        //    case 0xD3: // F6 - Display Offset - Set
        //    case 0xD4: // P1 - Page #1 - Clear
        //    case 0xD5: // P1 - Page #1 - Set
        //    case 0xD6: // R0 - CPU Rate - Clear
        //    case 0xD7: // R0 - CPU Rate - Set
        //    case 0xD8: // R1 - CPU Rate - Clear
        //    case 0xD9: // R1 - CPU Rate - Set
        //    case 0xDA: // M0 - Memory Size - Clear
        //    case 0xDB: // M0 - Memory Size - Set
        //    case 0xDC: // M1 - Memory Size - Clear
        //    case 0xDD: // M1 - Memory Size - Set
        //    case 0xDE: // TY - Map Type - ROM disable - Clear
        //    case 0xDF: // TY - Map Type - ROM disable - Set
        //        return _modules.TCC1014.SAMRead(port);  //MC6883 S.A.M. address range $FFC0-$FFDF
        //    #endregion

        //    // $FFE0 - $FFEF -- Not Used

        //    #region SAM
        //    case 0xF0:  // Reserved
        //    case 0xF1:  // Reserved
        //    case 0xF2:  // SWI3 vector MS
        //    case 0xF3:  // SWI3 vector LS
        //    case 0xF4:  // SWI2 vector MS
        //    case 0xF5:  // SWI2 vector LS
        //    case 0xF6:  // FIRQ vector MS
        //    case 0xF7:  // FIRQ vector LS
        //    case 0xF8:  // IRQ vector MS
        //    case 0xF9:  // IRQ vector LS
        //    case 0xFA:  // SWI1 vector MS
        //    case 0xFB:  // SWI1 vector LS
        //    case 0xFC:  // NMI vector MS
        //    case 0xFD:  // NMI vector LS
        //    case 0xFE:  // Reset vector MS
        //    case 0xFF:  // Reset vector LS
        //        return _modules.TCC1014.SAMRead(port);  // SAM controls IRQ Vectors at $FFF0 - $FFFF	
        //    #endregion

        //    default:
        //        return _modules.PAKInterface.PakPortRead(port);
        //}
        #endregion
    }

    public void PortWrite(byte data, ushort address)
    {
        byte port = (byte)(address & 0xFF);
        var target = Ports.Handler(port);

        switch (target)
        {
            case PortHandlers.PIA0:
                _modules.MC6821.Pia0_Write(data, port);
                break;
            case PortHandlers.PIA1:
                _modules.MC6821.Pia1_Write(data, port);
                break;
            case PortHandlers.GIME:
                _modules.TCC1014.GimeWrite(port, data);
                break;
            case PortHandlers.SAM:
                _modules.TCC1014.SAMWrite(data, port);
                break;
            default:
                _modules.PAKInterface.PakPortWrite(port, data);
                break;
        }

        #region OLD
        //switch (port)
        //{
        //    case Ports.PIA_0_0:
        //    case Ports.PIA_0_1:
        //    case Ports.PIA_0_2:
        //    case Ports.PIA_0_3:
        //        _modules.MC6821.Pia0_Write(data, port);	//MC6821 P.I.A  Keyboard access $FF00-$FF03
        //        break;

        //    case Ports.PIA_1_0:
        //    case Ports.PIA_1_1:
        //    case Ports.PIA_1_2:
        //    case Ports.PIA_1_3:
        //        _modules.MC6821.Pia1_Write(data, port);	//MC6821 P.I.A	Sound and VDG Control 
        //        break;

        //    case 0xC0:
        //    case 0xC1:
        //    case 0xC2:
        //    case 0xC3:
        //    case 0xC4:
        //    case 0xC5:
        //    case 0xC6:
        //    case 0xC7:
        //    case 0xC8:
        //    case 0xC9:
        //    case 0xCA:
        //    case 0xCB:
        //    case 0xCC:
        //    case 0xCD:
        //    case 0xCE:
        //    case 0xCF:
        //    case 0xD0:
        //    case 0xD1:
        //    case 0xD2:
        //    case 0xD3:
        //    case 0xD4:
        //    case 0xD5:
        //    case 0xD6:
        //    case 0xD7:
        //    case 0xD8:
        //    case 0xD9:
        //    case 0xDA:
        //    case 0xDB:
        //    case 0xDC:
        //    case 0xDD:
        //    case 0xDE:
        //    case 0xDF:
        //        _modules.TCC1014.SAMWrite(data, port);	//MC6883 S.A.M. address range $FFC0-$FFDF
        //        break;

        //    case 0x90:
        //    case 0x91:
        //    case 0x92:
        //    case 0x93:
        //    case 0x94:
        //    case 0x95:
        //    case 0x96:
        //    case 0x97:
        //    case 0x98:
        //    case 0x99:
        //    case 0x9A:
        //    case 0x9B:
        //    case 0x9C:
        //    case 0x9D:
        //    case 0x9E:
        //    case 0x9F:
        //    case 0xA0:
        //    case 0xA1:
        //    case 0xA2:
        //    case 0xA3:
        //    case 0xA4:
        //    case 0xA5:
        //    case 0xA6:
        //    case 0xA7:
        //    case 0xA8:
        //    case 0xA9:
        //    case 0xAA:
        //    case 0xAB:
        //    case 0xAC:
        //    case 0xAD:
        //    case 0xAE:
        //    case 0xAF:
        //    case 0xB0:
        //    case 0xB1:
        //    case 0xB2:
        //    case 0xB3:
        //    case 0xB4:
        //    case 0xB5:
        //    case 0xB6:
        //    case 0xB7:
        //    case 0xB8:
        //    case 0xB9:
        //    case 0xBA:
        //    case 0xBB:
        //    case 0xBC:
        //    case 0xBD:
        //    case 0xBE:
        //    case 0xBF:
        //        _modules.TCC1014.GimeWrite(port, data);
        //        break;

        //    default:
        //        _modules.PAKInterface.PakPortWrite(port, data);
        //        break;
        //}
        #endregion
    }

    public void ModuleReset()
    {
        //--Anything to reset?
    }
}
