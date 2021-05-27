namespace VCCSharp.Models
{
    public class ConfigModel
    {
        //[Version]
        public string Release; //## WRITE-ONLY ##//

        //[CPU]
        public byte CPUMultiplier;
        public byte FrameSkip;
        public byte SpeedThrottle;
        public byte CpuType;

        public ushort MaxOverclock;

        //[Audio]
        public string SoundCardName;
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
        public string ExternalBasicImage; //## READ-ONLY ##//

        //[Misc]
        public byte AutoStart;
        public byte CartAutoStart;
        public byte KeyMapIndex;

        //[Module]
        public string ModulePath;

        //[DefaultPaths]
        public string CassPath;
        public string PakPath;
        public string FloppyPath;
        public string CoCoRomPath; //## READ-ONLY ##//
        public string SerialCaptureFilePath;
 
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
