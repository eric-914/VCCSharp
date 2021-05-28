using System.Runtime.InteropServices;

namespace VCCSharp.Models.Pak
{
    public struct PakInterfaceDelegates
    {
        #region PAKMEMREAD8

        //typedef unsigned char (*PAKMEMREAD8)(unsigned short);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte PAKMEMREAD8(ushort address);

        //unsigned char (*PakMemRead8)(unsigned short);
        public PAKMEMREAD8 PakMemRead8;

        #endregion

        #region PAKMEMWRITE8

        //typedef void (*PAKMEMWRITE8)(unsigned char, unsigned short);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PAKMEMWRITE8(byte data, ushort address);

        //void (*PakMemWrite8)(unsigned char, unsigned short);
        public PAKMEMWRITE8 PakMemWrite8;

        #endregion

        #region PAKPORTREAD

        //typedef unsigned char (*PAKPORTREAD)(unsigned char);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte PAKPORTREAD(byte port);

        //unsigned char (*PakPortRead)(unsigned char);
        public PAKPORTREAD PakPortRead;

        #endregion

        #region PAKPORTWRITE

        //typedef void (*PAKPORTWRITE)(unsigned char, unsigned char);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PAKPORTWRITE(byte port, byte data);

        //void (*PakPortWrite)(unsigned char, unsigned char);
        public PAKPORTWRITE PakPortWrite;

        #endregion

        #region MODULEAUDIOSAMPLE

        //typedef unsigned short (*MODULEAUDIOSAMPLE)(void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate ushort MODULEAUDIOSAMPLE();

        //unsigned short (*ModuleAudioSample)(void);
        public MODULEAUDIOSAMPLE ModuleAudioSample;

        #endregion

        #region CONFIGMODULE

        //typedef void (*CONFIGMODULE)(unsigned char);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CONFIGMODULE(byte menuItem);

        //void (*ConfigModule)(unsigned char);
        public CONFIGMODULE ConfigModule;

        #endregion

        #region DMAMEMPOINTERS

        //typedef void (*DMAMEMPOINTERS) (PAKMEMREAD8, PAKMEMWRITE8);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DMAMEMPOINTERS(PAKMEMREAD8 read, PAKMEMWRITE8 write);

        //void (*DmaMemPointer) (MEMREAD8, MEMWRITE8);
        public DMAMEMPOINTERS DmaMemPointers;

        #endregion

        #region GETMODULENAME

        //typedef void (*DYNAMICMENUCALLBACK)(char*, int, int);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void DYNAMICMENUCALLBACK(byte* menuName, int menuId, int type);

        //typedef void (*GETMODULENAME)(char*, char*, DYNAMICMENUCALLBACK);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void GETMODULENAME(byte* modName, byte* catNumber, DYNAMICMENUCALLBACK callback);

        //void (*GetModuleName)(char*, char*, DYNAMICMENUCALLBACK);
        public GETMODULENAME GetModuleName;

        #endregion

        #region HEARTBEAT

        //typedef void (*HEARTBEAT) (void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void HEARTBEAT();

        //void (*HeartBeat)(void);
        public HEARTBEAT HeartBeat;

        #endregion

        #region MODULERESET

        //typedef void (*MODULERESET)(void);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void MODULERESET();

        //void (*ModuleReset) (void);
        public MODULERESET ModuleReset;

        #endregion

        #region MODULESTATUS

        //typedef void (*MODULESTATUS)(char*);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void MODULESTATUS(byte* statusLine);

        //void (*ModuleStatus)(char*);
        public MODULESTATUS ModuleStatus;

        #endregion

        #region PAKSETCART

        //typedef void (*SETCART)(unsigned char);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SETCART(byte port);

        //typedef void (*PAKSETCART)(SETCART);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PAKSETCART(SETCART callback);

        //void (*PakSetCart)(SETCART);
        public PAKSETCART PakSetCart;

        #endregion

        #region SETINIPATH

        //typedef void (*SETINIPATH)(char*);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void SETINIPATH(byte* ini);

        //void (*SetIniPath) (char*);
        public SETINIPATH SetIniPath;

        #endregion

        #region SETINTERRUPTCALLPOINTER

        //typedef void (*ASSERTINTERRUPT) (unsigned char, unsigned char);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ASSERTINTERRUPT(byte irq, byte flag);

        //typedef void (*SETINTERRUPTCALLPOINTER) (ASSERTINTERRUPT);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SETINTERRUPTCALLPOINTER(ASSERTINTERRUPT callback);

        //void (*SetInterruptCallPointer) (ASSERTINTERRUPT);
        public SETINTERRUPTCALLPOINTER SetInterruptCallPointer;

        #endregion
    }
}
