using System;
using System.Runtime.InteropServices;

// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
namespace VCCSharp.Models
{
    #region PAKMEMREAD8

    //typedef unsigned char (*PAKMEMREAD8)(unsigned short);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate byte PAKMEMREAD8(ushort address);
    
    #endregion

    #region PAKMEMWRITE8

    //typedef void (*PAKMEMWRITE8)(unsigned char, unsigned short);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PAKMEMWRITE8(byte data, ushort address);

    #endregion

    #region PAKPORTREAD

    //typedef unsigned char (*PAKPORTREAD)(unsigned char);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate byte PAKPORTREAD(byte port);

    #endregion

    #region PAKPORTWRITE

    //typedef void (*PAKPORTWRITE)(unsigned char, unsigned char);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PAKPORTWRITE(byte port, byte data);

    #endregion

    #region MODULEAUDIOSAMPLE

    //typedef unsigned short (*MODULEAUDIOSAMPLE)(void);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate ushort MODULEAUDIOSAMPLE();

    #endregion

    #region CONFIGMODULE

    //typedef void (*CONFIGMODULE)(unsigned char);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CONFIGMODULE(byte menuItem);

    #endregion

    #region DMAMEMPOINTERS

    //typedef void (*DMAMEMPOINTERS) (PAKMEMREAD8, PAKMEMWRITE8);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DMAMEMPOINTERS(PAKMEMREAD8 read, PAKMEMWRITE8 write);

    #endregion

    #region GETMODULENAME

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DYNAMICMENUCALLBACK(string menuName, int menuId, int type);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void GETMODULENAME(byte* buffer, string catNumber, DYNAMICMENUCALLBACK callback);

    #endregion

    #region HEARTBEAT

    //typedef void (*HEARTBEAT) (void);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void HEARTBEAT();

    #endregion

    #region MODULERESET

    //typedef void (*MODULERESET)(void);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void MODULERESET();

    #endregion

    #region MODULESTATUS

    //typedef void (*MODULESTATUS)(char*);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void MODULESTATUS(byte* statusLine);

    #endregion

    #region PAKSETCART

    //typedef void (*SETCART)(unsigned char);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SETCART(byte port);

    //typedef void (*PAKSETCART)(SETCART);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PAKSETCART(SETCART callback);

    #endregion

    #region SETINIPATH

    //typedef void (*SETINIPATH)(char*);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SETINIPATH(string ini);

    #endregion

    #region SETINTERRUPTCALLPOINTER

    //typedef void (*ASSERTINTERRUPT) (unsigned char, unsigned char);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ASSERTINTERRUPT(byte irq, byte flag);

    //typedef void (*SETINTERRUPTCALLPOINTER) (ASSERTINTERRUPT);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SETINTERRUPTCALLPOINTER(ASSERTINTERRUPT callback);

    #endregion

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
