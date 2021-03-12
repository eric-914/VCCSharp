using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConfigModel
    {
        //[Version]
        public unsafe fixed byte Release[Define.MAX_LOADSTRING]; //## WRITE-ONLY ##//

        //[CPU]
        public byte CPUMultiplier;
        public byte FrameSkip;
        public byte SpeedThrottle;
        public byte CpuType;

        public ushort MaxOverclock;

        //[Audio]
        public unsafe fixed byte SoundCardName[Define.MAX_LOADSTRING];
        public ushort AudioRate;

        //[Video]
        public byte MonitorType;
        public byte PaletteType;
        public byte ScanLines;
        public byte ForceAspect;
        public ushort RememberSize;

        public short WindowSizeX;
        public short WindowSizeY;

        //[Memory]
        public byte RamSize;
        public unsafe fixed byte ExternalBasicImage[Define.MAX_PATH]; //## READ-ONLY ##//

        //[Misc]
        public byte AutoStart;
        public byte CartAutoStart;
        public byte KeyMapIndex;

        //[Module]
        public unsafe fixed byte ModulePath[Define.MAX_PATH];

        //[LeftJoyStick]  //struct {} JoyStick
        public unsafe JoystickModel* Left;

        //[RightJoyStick]
        public unsafe JoystickModel* Right;

        //[DefaultPaths]
        public unsafe fixed byte CassPath[Define.MAX_PATH];
        public unsafe fixed byte PakPath[Define.MAX_PATH];
        public unsafe fixed byte FloppyPath[Define.MAX_PATH];
        public unsafe fixed byte CoCoRomPath[Define.MAX_PATH]; //## READ-ONLY ##//
        public unsafe fixed byte SerialCaptureFilePath[Define.MAX_PATH];
 
        //[FD-502]  //### MODULE SPECIFIC ###//
        //DiskRom=1
        //RomPath=
        //Persist=1
        //Disk#0=
        //Disk#1=
        //Disk#2=
        //Disk#3=
        //ClkEnable=1
        //TurboDisk=1

        //[MPI]     //### MODULE SPECIFIC ###//
        //SWPOSITION=3
        //PesistPaks=1
        //SLOT1=
        //SLOT2=
        //SLOT3=
        //SLOT4=C:\CoCo\Mega-Bug (1982) (26-3076) (Tandy).ccc
        //"MPIPath"   //TODO: Originally in [DefaultPaths]

        //[HardDisk]  //### MODULE SPECIFIC ###// 
        //"HardDiskPath"  //TODO: Originally in [DefaultPaths]

        //[SuperIDE]  //### MODULE SPECIFIC ###//
        //"SuperIDEPath"  //TODO: Originally in [DefaultPaths]
    }
}
