using System;

// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
namespace VCCSharp.Models
{
    public delegate byte PAKMEMREAD8(ushort address);
    public delegate void PAKMEMWRITE8(byte data, ushort address);
    public delegate byte PAKPORTREAD(byte port);
    public delegate void PAKPORTWRITE(byte port, byte data);
    public delegate ushort MODULEAUDIOSAMPLE();
    public delegate void CONFIGMODULE(byte menuItem);
    public delegate void DMAMEMPOINTERS(PAKMEMREAD8 read, PAKMEMWRITE8 write);
    public delegate void DYNAMICMENUCALLBACK(string menuName, int menuId, int type);
    public delegate void GETMODULENAME(byte[] buffer, string catNumber, DYNAMICMENUCALLBACK callback);
    public delegate void HEARTBEAT();
    public delegate void MODULERESET();
    public delegate void MODULESTATUS(byte[] statusLine);
    public delegate void SETCART(byte port);
    public delegate void PAKSETCART(SETCART callback);
    public delegate void SETINIPATH(string ini);
    public delegate void ASSERTINTERRUPT(byte irq, byte flag);
    public delegate void SETINTERRUPTCALLPOINTER(ASSERTINTERRUPT callback);

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class PakInterfaceDelegates
    {
        //unsigned char (*PakMemRead8)(unsigned short);
        //public PAKMEMREAD8* PakMemRead8;
        public IntPtr PakMemRead8;

        ////void (*PakMemWrite8)(unsigned char, unsigned short);
        //public PAKMEMWRITE8 PakMemWrite8;
        public IntPtr PakMemWrite8;
        
        ////unsigned char (*PakPortRead)(unsigned char);
        //public PAKPORTREAD PakPortRead;
        public IntPtr PakPortRead;

        ////void (*PakPortWrite)(unsigned char, unsigned char);
        //public PAKPORTWRITE PakPortWrite;
        public IntPtr PakPortWrite;

        ////unsigned short (*ModuleAudioSample)(void);
        //public MODULEAUDIOSAMPLE ModuleAudioSample;
        public IntPtr ModuleAudioSample;

        ////void (*ConfigModule)(unsigned char);
        //public CONFIGMODULE ConfigModule;
        public IntPtr ConfigModule;

        ////void (*DmaMemPointer) (MEMREAD8, MEMWRITE8);
        //public DMAMEMPOINTERS DmaMemPointers;
        public IntPtr DmaMemPointers;

        ////void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
        //public GETMODULENAME GetModuleName;
        public IntPtr GetModuleName;

        ////void (*HeartBeat)(void);
        //public HEARTBEAT HeartBeat;
        public IntPtr HeartBeat;

        ////void (*ModuleReset) (void);
        //public MODULERESET ModuleReset;
        public IntPtr ModuleReset;

        ////void (*ModuleStatus)(char*);
        //public MODULESTATUS ModuleStatus;
        public IntPtr ModuleStatus;

        ////void (*PakSetCart)(SETCART);
        //public PAKSETCART PakSetCart;
        public IntPtr PakSetCart;

        ////void (*SetIniPath) (char*);
        //public SETINIPATH SetIniPath;
        public IntPtr SetIniPath;

        ////void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
        //public SETINTERRUPTCALLPOINTER SetInterruptCallPointer;
        public IntPtr SetInterruptCallPointer;
    }
}
// ReSharper restore CommentTypo
// ReSharper restore InconsistentNaming
