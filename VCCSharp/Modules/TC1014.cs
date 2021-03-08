﻿using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface ITC1014
    {
        void MC6883Reset();
        void CopyRom();
        void MmuReset();
        byte MmuInit(byte ramSizeOption);
        void MemWrite8(byte data, ushort address);
        void GimeAssertVertInterrupt();
        void GimeAssertHorzInterrupt();
        void GimeAssertTimerInterrupt();
    }

    public class TC1014 : ITC1014
    {
        private readonly IModules _modules;

        public TC1014(IModules modules)
        {
            _modules = modules;
        }

        public void MC6883Reset()
        {
            Library.TC1014.MC6883Reset();
        }

        public void CopyRom()
        {
            Library.TC1014.CopyRom();
        }

        public void MmuReset()
        {
            Library.TC1014.MmuReset();
        }

        public byte MmuInit(byte ramSizeOption)
        {
            return Library.TC1014.MmuInit(ramSizeOption);
        }

        public void MemWrite8(byte data, ushort address)
        {
            Library.TC1014.MemWrite8(data, address);
        }

        public void GimeAssertVertInterrupt()
        {
            Library.TC1014.GimeAssertVertInterrupt();
        }

        public void GimeAssertHorzInterrupt()
        {
            Library.TC1014.GimeAssertHorzInterrupt();
        }

        public void GimeAssertTimerInterrupt()
        {
            Library.TC1014.GimeAssertTimerInterrupt();
        }
    }
}
