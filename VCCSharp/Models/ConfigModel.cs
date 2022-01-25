using VCCSharp.Enums;

namespace VCCSharp.Models
{
    public class ConfigModel
    {
        //[Version]
        public string Release { get; set; } //## WRITE-ONLY ##//

        //[CPU]
        public byte CPUMultiplier { get; set; }
        public byte FrameSkip { get; set; }
        public byte SpeedThrottle { get; set; }
        public byte CpuType { get; set; }

        public ushort MaxOverclock { get; set; }

        //[Audio]
        public string SoundCardName { get; set; }
        public ushort AudioRate { get; set; }

        //[Video]
        public MonitorTypes MonitorType { get; set; }
        public byte PaletteType { get; set; }
        public byte ScanLines { get; set; }
        public bool ForceAspect { get; set; }
        public bool RememberSize { get; set; }

        public short WindowSizeX { get; set; }
        public short WindowSizeY { get; set; }

        //[Memory]
        public byte RamSize { get; set; }
        public string ExternalBasicImage { get; set; } //## READ-ONLY ##//

        //[Misc]
        public bool AutoStart { get; set; }
        public byte CartAutoStart { get; set; }
        public KeyboardLayouts KeyboardLayout { get; set; }

        //[Module]
        public string ModulePath { get; set; }

        //[DefaultPaths]
        public string CassPath { get; set; }
        public string PakPath { get; set; }
        public string FloppyPath { get; set; }
        public string CoCoRomPath { get; set; } //## READ-ONLY ##//
        public string SerialCaptureFilePath { get; set; }
 
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
