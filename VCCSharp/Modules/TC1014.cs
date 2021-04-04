using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

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
        byte MemRead8(ushort address);
        ushort MemRead16(ushort addr);
        void MemWrite16(ushort data, ushort addr);
        ushort GetMem(int address);
        void SetMapType(byte type);
        void SetRomMap(byte data);
        unsafe void DrawBottomBorder16(EmuState* emuState);
        unsafe void DrawBottomBorder24(EmuState* emuState);
        unsafe void DrawBottomBorder32(EmuState* emuState);
        unsafe void DrawBottomBorder8(EmuState* emuState);
        unsafe void DrawTopBorder16(EmuState* emuState);
        unsafe void DrawTopBorder24(EmuState* emuState);
        unsafe void DrawTopBorder32(EmuState* emuState);
        unsafe void DrawTopBorder8(EmuState* emuState);
        unsafe void UpdateScreen16(EmuState* emuState);
        unsafe void UpdateScreen24(EmuState* emuState);
        unsafe void UpdateScreen32(EmuState* emuState);
        unsafe void UpdateScreen8(EmuState* emuState);
    }

    public class TC1014 : ITC1014
    {
        #region CC2 Font

        private static byte[] ntsc_round_fontdata8x12 =
        {
            0x00, 0x00, 0x38, 0x44, 0x04, 0x34, 0x4C, 0x4C, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x28, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x78, 0x24, 0x24, 0x38, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x40, 0x40, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x78, 0x24, 0x24, 0x24, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x40, 0x40, 0x4C, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x04, 0x04, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x48, 0x50, 0x60, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x6C, 0x54, 0x54, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x64, 0x54, 0x4C, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x78, 0x44, 0x44, 0x78, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x44, 0x44, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x78, 0x44, 0x44, 0x78, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x40, 0x38, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x28, 0x28, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x44, 0x54, 0x6C, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x28, 0x10, 0x28, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x28, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x7C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x20, 0x20, 0x20, 0x20, 0x20, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x40, 0x20, 0x10, 0x08, 0x04, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x38, 0x54, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x10, 0x20, 0x7C, 0x20, 0x10, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x28, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x28, 0x28, 0x7C, 0x28, 0x7C, 0x28, 0x28, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x3C, 0x50, 0x38, 0x14, 0x78, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x60, 0x64, 0x08, 0x10, 0x20, 0x4C, 0x0C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x50, 0x50, 0x20, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x10, 0x20, 0x20, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x10, 0x08, 0x08, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x10, 0x54, 0x38, 0x38, 0x54, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x10, 0x10, 0x7C, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x20, 0x40, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x04, 0x08, 0x10, 0x20, 0x40, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x4C, 0x54, 0x64, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x30, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x04, 0x38, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x04, 0x08, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x18, 0x28, 0x48, 0x7C, 0x08, 0x08, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x40, 0x78, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x40, 0x40, 0x78, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x40, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x44, 0x38, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x44, 0x3C, 0x04, 0x04, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x10, 0x20, 0x40, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x10, 0x08, 0x04, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x04, 0x08, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00,

            /* Block Graphics (Semigraphics 4 Graphics ) */
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F,
            0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0,
            0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F,
            0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0,
            0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,

            /* Lower case */
            0x00, 0x00, 0x10, 0x28, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x40, 0x58, 0x64, 0x44, 0x64, 0x58, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x38, 0x44, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x04, 0x04, 0x34, 0x4C, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x14, 0x10, 0x38, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x34, 0x4C, 0x44, 0x4C, 0x34, 0x04, 0x38, 0x00,
            0x00, 0x00, 0x40, 0x40, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x00, 0x30, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x04, 0x00, 0x04, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x40, 0x48, 0x50, 0x60, 0x50, 0x48, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x30, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x78, 0x54, 0x54, 0x54, 0x54, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x78, 0x44, 0x44, 0x44, 0x78, 0x40, 0x40, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x3C, 0x44, 0x44, 0x44, 0x3C, 0x04, 0x04, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x58, 0x64, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x3C, 0x40, 0x38, 0x04, 0x78, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x20, 0x70, 0x20, 0x20, 0x24, 0x18, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x44, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x44, 0x44, 0x44, 0x28, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x44, 0x54, 0x54, 0x28, 0x28, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x44, 0x28, 0x10, 0x28, 0x44, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x44, 0x44, 0x44, 0x3C, 0x04, 0x38, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x7C, 0x08, 0x10, 0x20, 0x7C, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x10, 0x10, 0x20, 0x10, 0x10, 0x08, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x10, 0x10, 0x00, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x10, 0x10, 0x08, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x20, 0x54, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00,
        };

        #endregion

        #region CC3 Font

        private static byte[] cc3Fontdata8x12 =
        {
            /* Junk padding fix me chrs 0-31 */
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,

            /* Normal Text chrs 32-127 */
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // space
            0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // !
            0x28, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // "
            0x28, 0x28, 0x7C, 0x28, 0x7C, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, // #
            0x10, 0x3C, 0x50, 0x38, 0x14, 0x78, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // $
            0x60, 0x64, 0x08, 0x10, 0x20, 0x4C, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, // %
            0x20, 0x50, 0x50, 0x20, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // &
            0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // '
            0x08, 0x10, 0x20, 0x20, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, // (
            0x20, 0x10, 0x08, 0x08, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, // )
            0x00, 0x10, 0x54, 0x38, 0x38, 0x54, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // *
            0x00, 0x10, 0x10, 0x7C, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // +
            0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, // ,
            0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // -
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // .
            0x00, 0x04, 0x08, 0x10, 0x20, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // /
            0x38, 0x44, 0x4C, 0x54, 0x64, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 0
            0x10, 0x30, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 1
            0x38, 0x44, 0x04, 0x38, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // 2
            0x38, 0x44, 0x04, 0x08, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 3
            0x08, 0x18, 0x28, 0x48, 0x7C, 0x08, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, // 4
            0x7C, 0x40, 0x78, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 5
            0x38, 0x40, 0x40, 0x78, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 6
            0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, // 7
            0x38, 0x44, 0x44, 0x38, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 8
            0x38, 0x44, 0x44, 0x38, 0x04, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 9
            0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // :
            0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, // ;
            0x08, 0x10, 0x20, 0x40, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, // <
            0x00, 0x00, 0x7C, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // =
            0x20, 0x10, 0x08, 0x04, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, // >
            0x38, 0x44, 0x04, 0x08, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // ?
            0x38, 0x44, 0x04, 0x34, 0x4C, 0x4C, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // @
            0x10, 0x28, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // A
            0x78, 0x24, 0x24, 0x38, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, // B
            0x38, 0x44, 0x40, 0x40, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // C
            0x78, 0x24, 0x24, 0x24, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, // D
            0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // E
            0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, // F
            0x38, 0x44, 0x40, 0x40, 0x4C, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // G
            0x44, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // H
            0x38, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // I
            0x04, 0x04, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // J
            0x44, 0x48, 0x50, 0x60, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // K
            0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // L
            0x44, 0x6C, 0x54, 0x54, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // M
            0x44, 0x44, 0x64, 0x54, 0x4C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // N
            0x38, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // O
            0x78, 0x44, 0x44, 0x78, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, // P
            0x38, 0x44, 0x44, 0x44, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // Q
            0x78, 0x44, 0x44, 0x78, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // R
            0x38, 0x44, 0x40, 0x38, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // S
            0x7C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // T
            0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // U
            0x44, 0x44, 0x44, 0x28, 0x28, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // V
            0x44, 0x44, 0x44, 0x44, 0x54, 0x6C, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // W
            0x44, 0x44, 0x28, 0x10, 0x28, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // X
            0x44, 0x44, 0x28, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // Y
            0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // Z
            0x38, 0x20, 0x20, 0x20, 0x20, 0x20, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // [
            0x00, 0x40, 0x20, 0x10, 0x08, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // \ 
            0x38, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // ]
            0x10, 0x38, 0x54, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // Up Arrow
            0x00, 0x10, 0x20, 0x7C, 0x20, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // Left Arrow
            0x10, 0x28, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // ^
            0x00, 0x00, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // a
            0x40, 0x40, 0x58, 0x64, 0x44, 0x64, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, // b
            0x00, 0x00, 0x38, 0x44, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // c
            0x04, 0x04, 0x34, 0x4C, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // d
            0x00, 0x00, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // e
            0x08, 0x14, 0x10, 0x38, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // f
            0x00, 0x00, 0x34, 0x4C, 0x4C, 0x34, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00, // g
            0x40, 0x40, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // h
            0x00, 0x10, 0x00, 0x30, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // i
            0x00, 0x04, 0x00, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, // j
            0x40, 0x40, 0x48, 0x50, 0x60, 0x50, 0x48, 0x00, 0x00, 0x00, 0x00, 0x00, // k
            0x30, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // l
            0x00, 0x00, 0x68, 0x54, 0x54, 0x54, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, // m
            0x00, 0x00, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // n
            0x00, 0x00, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x00, 0x00, 0x78, 0x44, 0x44, 0x78, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, // p
            0x00, 0x00, 0x3C, 0x44, 0x44, 0x3C, 0x04, 0x04, 0x00, 0x00, 0x00, 0x00, // q
            0x00, 0x00, 0x58, 0x64, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, // r
            0x00, 0x00, 0x3C, 0x40, 0x38, 0x04, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, // s
            0x20, 0x20, 0x70, 0x20, 0x20, 0x24, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, // t
            0x00, 0x00, 0x44, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // u
            0x00, 0x00, 0x44, 0x44, 0x44, 0x28, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // v
            0x00, 0x00, 0x44, 0x54, 0x54, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, // w
            0x00, 0x00, 0x44, 0x28, 0x10, 0x28, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // x
            0x00, 0x00, 0x44, 0x44, 0x44, 0x3C, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00, // y
            0x00, 0x00, 0x7C, 0x08, 0x10, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // z
            0x08, 0x10, 0x10, 0x20, 0x10, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, // {
            0x10, 0x10, 0x10, 0x00, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, // |
            0x20, 0x10, 0x10, 0x08, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, // }
            0x20, 0x54, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // ~
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // _

            /* Junk padding fix me chrs 128-159 */
            0x38, 0x44, 0x40, 0x40, 0x40, 0x44, 0x38, 0x10, 0x08, 0x00, 0x00, 0x00, // C
            0x44, 0x00, 0x44, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // u
            0x08, 0x10, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // e
            0x10, 0x28, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // a
            0x28, 0x00, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // a
            0x20, 0x10, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // a
            0x10, 0x00, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // a
            0x00, 0x00, 0x38, 0x44, 0x40, 0x44, 0x38, 0x10, 0x00, 0x00, 0x00, 0x00, // c
            0x10, 0x28, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // e
            0x28, 0x00, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // e
            0x20, 0x10, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // e
            0x28, 0x00, 0x30, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // l
            0x10, 0x28, 0x00, 0x30, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // i
            0x00, 0x38, 0x44, 0x78, 0x44, 0x44, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, // B
            0x24, 0x10, 0x28, 0x44, 0x7C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // A
            0x10, 0x10, 0x28, 0x44, 0x7C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, // A
            0x08, 0x10, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x00, 0x00, 0x68, 0x14, 0x3C, 0x50, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x3C, 0x50, 0x50, 0x7A, 0x50, 0x50, 0x5C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x28, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x28, 0x00, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x00, 0x00, 0x38, 0x4C, 0x54, 0x64, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x10, 0x28, 0x00, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // u
            0x20, 0x10, 0x44, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, // u
            0x38, 0x4C, 0x54, 0x54, 0x54, 0x64, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // 0
            0x44, 0x38, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // o
            0x28, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // U
            0x38, 0x40, 0x38, 0x44, 0x38, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, // s
            0x08, 0x14, 0x10, 0x38, 0x10, 0x50, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, // f
            0x10, 0x10, 0x7C, 0x10, 0x10, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, // +
            0x10, 0x28, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // '
            0x08, 0x14, 0x10, 0x38, 0x10, 0x10, 0x20, 0x40, 0x00, 0x00, 0x00, 0x00, // f

            /* Normal Text chrs 160-255 */
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x28, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x28, 0x28, 0x7C, 0x28, 0x7C, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x3C, 0x50, 0x38, 0x14, 0x78, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x60, 0x64, 0x08, 0x10, 0x20, 0x4C, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x50, 0x50, 0x20, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x08, 0x10, 0x20, 0x20, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x10, 0x08, 0x08, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x10, 0x54, 0x38, 0x38, 0x54, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x10, 0x10, 0x7C, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x04, 0x08, 0x10, 0x20, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x4C, 0x54, 0x64, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x30, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x04, 0x38, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x04, 0x08, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x08, 0x18, 0x28, 0x48, 0x7C, 0x08, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x40, 0x78, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x40, 0x40, 0x78, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x44, 0x38, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x44, 0x38, 0x04, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00,
            0x08, 0x10, 0x20, 0x40, 0x20, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x10, 0x08, 0x04, 0x08, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x04, 0x08, 0x10, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x04, 0x34, 0x4C, 0x4C, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x28, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x78, 0x24, 0x24, 0x38, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x40, 0x40, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x78, 0x24, 0x24, 0x24, 0x24, 0x24, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x40, 0x40, 0x70, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x40, 0x40, 0x4C, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x44, 0x7C, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x48, 0x50, 0x60, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x6C, 0x54, 0x54, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x64, 0x54, 0x4C, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x78, 0x44, 0x44, 0x78, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x44, 0x44, 0x54, 0x48, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x78, 0x44, 0x44, 0x78, 0x50, 0x48, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x44, 0x40, 0x38, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x44, 0x28, 0x28, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x44, 0x44, 0x54, 0x6C, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x28, 0x10, 0x28, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x44, 0x44, 0x28, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x7C, 0x04, 0x08, 0x10, 0x20, 0x40, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x20, 0x20, 0x20, 0x20, 0x20, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x40, 0x20, 0x10, 0x08, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x38, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x38, 0x54, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x10, 0x20, 0x7C, 0x20, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x28, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x04, 0x3C, 0x44, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x40, 0x58, 0x64, 0x44, 0x64, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x40, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x04, 0x04, 0x34, 0x4C, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x7C, 0x40, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x08, 0x14, 0x10, 0x38, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x34, 0x4C, 0x4C, 0x34, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x40, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x10, 0x00, 0x30, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x04, 0x00, 0x04, 0x04, 0x04, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x40, 0x48, 0x50, 0x60, 0x50, 0x48, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x30, 0x10, 0x10, 0x10, 0x10, 0x10, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x68, 0x54, 0x54, 0x54, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x58, 0x64, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x44, 0x44, 0x44, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x78, 0x44, 0x44, 0x78, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x3C, 0x44, 0x44, 0x3C, 0x04, 0x04, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x58, 0x64, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x3C, 0x40, 0x38, 0x04, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x20, 0x70, 0x20, 0x20, 0x24, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x4C, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x28, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x54, 0x54, 0x28, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x28, 0x10, 0x28, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x44, 0x44, 0x44, 0x3C, 0x04, 0x38, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x7C, 0x08, 0x10, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x08, 0x10, 0x10, 0x20, 0x10, 0x10, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x10, 0x10, 0x10, 0x00, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x10, 0x10, 0x08, 0x10, 0x10, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x54, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        #endregion

        private readonly IModules _modules;

        public TC1014(IModules modules)
        {
            _modules = modules;
        }

        public unsafe TC1014MmuState* GetTC1014MmuState()
        {
            return Library.TC1014.GetTC1014MmuState();
        }

        public unsafe TC1014RegistersState* GetTC1014RegistersState()
        {
            return Library.TC1014.GetTC1014RegistersState();
        }

        public void MC6883Reset()
        {
            Library.TC1014.MC6883Reset();
        }

        //TODO: Used by MmuInit()
        public void CopyRom()
        {
            const string ROM = "coco3.rom";

            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();

                //--Try loading from Vcc.ini >> CoCoRomPath
                string cocoRomPath = Converter.ToString(configState->Model->CoCoRomPath);

                string path = Path.Combine(Converter.ToString(configState->Model->CoCoRomPath), ROM);

                if (LoadInternalRom(path) == Define.TRUE)
                {
                    Debug.WriteLine($"Found {ROM} in CoCoRomPath");
                    return;
                }

                //--Try loading from Vcc.inin >> ExternalBasicImage
                string externalBasicImage = _modules.Config.ExternalBasicImage();

                if (!string.IsNullOrEmpty(externalBasicImage) && LoadInternalRom(externalBasicImage) == Define.TRUE)
                {
                    Debug.WriteLine($"Found {ROM} in ExternalBasicImage");
                    return;
                }

                //--Try loading from current executable folder
                string exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());
                string exeFile = Path.Combine(exePath, ROM);

                if (LoadInternalRom(exeFile) != Define.FALSE)
                {
                    Debug.WriteLine($"Found {ROM} in executable folder");
                    return;
                }

                //--Give up...
                string message = @$"
Could not locate {ROM} in any of these locations:
* Vcc.ini >> CoCoRomPath=""{cocoRomPath}""
* Vcc.ini >> ExternalBasicImage=""{externalBasicImage}""
* In the same folder as the executable: ""{exePath}""
";

                MessageBox.Show(message, "Error");

                Environment.Exit(0);
            }
        }

        public void MmuReset()
        {
            unsafe
            {
                TC1014MmuState* instance = GetTC1014MmuState();

                instance->MmuTask = 0;
                instance->MmuEnabled = 0;
                instance->RamVectors = 0;
                instance->MmuState = 0;
                instance->RomMap = 0;
                instance->MapType = 0;
                instance->MmuPrefix = 0;

                ushort[,] MmuRegisters = new ushort[4, 8];

                for (ushort index1 = 0; index1 < 8; index1++)
                {
                    for (ushort index2 = 0; index2 < 4; index2++)
                    {
                        MmuRegisters[index2, index1] = (ushort)(index1 + instance->StateSwitch[instance->CurrentRamConfig]);
                    }
                }

                for (int index = 0; index < 32; index++)
                {
                    instance->MmuRegisters[index] = MmuRegisters[index >> 3, index & 7];
                }

                for (int index = 0; index < 1024; index++)
                {
                    byte* offset = instance->Memory + (index & instance->RamMask[instance->CurrentRamConfig]) * 0x2000;
                    instance->MemPages[index] = (long)offset;
                    instance->MemPageOffsets[index] = 1;
                }

                SetRomMap(0);
                SetMapType(0);
            }
        }

        /*****************************************************************************************
        * MmuInit Initialize and allocate memory for RAM Internal and External ROM Images.        *
        * Copy Rom Images to buffer space and reset GIME MMU registers to 0                      *
        * Returns NULL if any of the above fail.                                                 *
        *****************************************************************************************/
        public byte MmuInit(byte ramSizeOption)
        {
            unsafe
            {
                TC1014MmuState* mmuState = GetTC1014MmuState();

                uint ramSize = mmuState->MemConfig[ramSizeOption];

                mmuState->CurrentRamConfig = ramSizeOption;

                FreeMemory(mmuState->Memory);

                mmuState->Memory = AllocateMemory(ramSize);

                if (mmuState->Memory == null)
                {
                    return 0;
                }

                //--Well, this explains the vertical bands when you start a graphics mode in BASIC w/out PCLS
                for (uint index = 0; index < ramSize; index++)
                {
                    mmuState->Memory[index] = (byte)((index & 1) == 0 ? 0 : 0xFF);
                }

                _modules.Graphics.SetVidMask(mmuState->VidMask[mmuState->CurrentRamConfig]);

                FreeMemory(mmuState->InternalRomBuffer);
                mmuState->InternalRomBuffer = AllocateMemory(0x8001); //--TODO: Weird that the extra byte is needed here

                if (mmuState->InternalRomBuffer == null)
                {
                    return 0;
                }

                //memset(mmuState->InternalRomBuffer, 0xFF, 0x8000);
                for (uint index = 0; index <= 0x8000; index++)
                {
                    mmuState->InternalRomBuffer[index] = 0xFF;
                }

                CopyRom();
                MmuReset();

                return 1;
            }
        }

        public void GimeAssertHorzInterrupt()
        {
            Library.TC1014.GimeAssertHorzInterrupt();
        }

        public void GimeAssertTimerInterrupt()
        {
            Library.TC1014.GimeAssertTimerInterrupt();
        }

        public ushort LoadInternalRom(string filename)
        {
            Debug.WriteLine($"LoadInternalRom: {filename}");

            if (!File.Exists(filename)) return 0;

            byte[] bytes = File.ReadAllBytes(filename);

            unsafe
            {
                TC1014MmuState* instance = GetTC1014MmuState();

                for (ushort index = 0; index < bytes.Length; index++)
                {
                    instance->InternalRomBuffer[index] = bytes[index];
                }
            }

            return (ushort)bytes.Length;
        }

        public void GimeAssertVertInterrupt()
        {
            unsafe
            {
                TC1014RegistersState* registersState = GetTC1014RegistersState();

                if (((registersState->GimeRegisters[0x93] & 8) != 0) && (registersState->EnhancedFIRQFlag == 1))
                {
                    _modules.CPU.CPUAssertInterrupt(CPUInterrupts.FIRQ, 0); //FIRQ

                    registersState->LastFirq |= 8;
                }
                else if (((registersState->GimeRegisters[0x92] & 8) != 0) && (registersState->EnhancedIRQFlag == 1))
                {
                    _modules.CPU.CPUAssertInterrupt(CPUInterrupts.IRQ, 0); //IRQ moon patrol demo using this

                    registersState->LastIrq |= 8;
                }
            }
        }

        public unsafe void FreeMemory(byte* target)
        {
            if (target != null)
            {
                Marshal.FreeHGlobal((IntPtr)target);
            }
        }

        public unsafe byte* AllocateMemory(uint size)
        {
            return (byte*)Marshal.AllocHGlobal((int)size); //malloc(size);
        }

        public byte MemRead8(ushort address)
        {
            return Library.TC1014.MemRead8(address);
        }

        public void MemWrite8(byte data, ushort address)
        {
            Library.TC1014.MemWrite8(data, address);
        }

        public ushort MemRead16(ushort addr)
        {
            return Library.TC1014.MemRead16(addr);
        }

        public void MemWrite16(ushort data, ushort addr)
        {
            Library.TC1014.MemWrite16(data, addr);
        }

        //--I think this is just a hack to access memory directly for the 40/80 char-wide screen-scrapes
        public ushort GetMem(int address)
        {
            unsafe
            {
                TC1014MmuState* instance = GetTC1014MmuState();

                return instance->Memory[address];
            }
        }

        public void SetMapType(byte type)
        {
            Library.TC1014.SetMapType(type);
        }

        public void SetRomMap(byte data)
        {
            Library.TC1014.SetRomMap(data);
        }

        public unsafe void DrawTopBorder8(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface8[x + ((emuState->LineCounter * 2) * emuState->SurfacePitch)] = (byte)(gs->BorderColor8 | 128);

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface8[x + ((emuState->LineCounter * 2 + 1) * emuState->SurfacePitch)] = (byte)(gs->BorderColor8 | 128);
                }
            }
        }

        public unsafe void DrawTopBorder16(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface16[x + ((emuState->LineCounter * 2) * emuState->SurfacePitch)] = gs->BorderColor16;

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface16[x + ((emuState->LineCounter * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor16;
                }
            }
        }

        public unsafe void DrawTopBorder24(EmuState* emuState)
        {
            //--Not implemented
        }

        public unsafe void DrawTopBorder32(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface32[x + ((emuState->LineCounter * 2) * emuState->SurfacePitch)] = gs->BorderColor32;

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface32[x + ((emuState->LineCounter * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor32;
                }
            }
        }

        public unsafe void DrawBottomBorder8(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface8[x + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = (byte)(gs->BorderColor8 | 128);

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface8[x + emuState->SurfacePitch + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = (byte)(gs->BorderColor8 | 128);
                }
            }
        }

        public unsafe void DrawBottomBorder16(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface16[x + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = gs->BorderColor16;

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface16[x + emuState->SurfacePitch + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = gs->BorderColor16;
                }
            }
        }

        public unsafe void DrawBottomBorder24(EmuState* emuState)
        {
            //--Not implemented
        }

        public unsafe void DrawBottomBorder32(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (gs->BorderChange == 0)
            {
                return;
            }

            for (ushort x = 0; x < emuState->WindowSize.X; x++)
            {
                graphicsSurfaces->pSurface32[x + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = gs->BorderColor32;

                if (emuState->ScanLines == Define.FALSE)
                {
                    graphicsSurfaces->pSurface32[x + emuState->SurfacePitch + (2 * (emuState->LineCounter + gs->LinesperScreen + gs->VertCenter) * emuState->SurfacePitch)] = gs->BorderColor32;
                }
            }
        }

        public unsafe void UpdateScreen8(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if ((gs->HorzCenter != 0) && (gs->BorderChange > 0))
            {
                for (ushort x = 0; x < gs->HorzCenter; x++)
                {
                    graphicsSurfaces->pSurface8[x + (((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch)] = gs->BorderColor8;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        graphicsSurfaces->pSurface8[x + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor8;
                    }

                    graphicsSurfaces->pSurface8[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch)] = gs->BorderColor8;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        graphicsSurfaces->pSurface8[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor8;
                    }
                }
            }

            if (gs->LinesperRow < 13)
            {
                gs->TagY++;
            }

            if (emuState->LineCounter == Define.FALSE)
            {
                gs->StartofVidram = gs->NewStartofVidram;
                gs->TagY = (ushort)(emuState->LineCounter);
            }

            uint start = (uint)(gs->StartofVidram + (gs->TagY / gs->LinesperRow) * (gs->VPitch * gs->ExtendedText));
            uint yStride = (uint)((((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch) + (gs->HorzCenter) - 1);

            SwitchMasterMode8(emuState, gs->MasterMode, start, yStride);
        }

        public unsafe void UpdateScreen16(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if ((gs->HorzCenter != 0) && (gs->BorderChange > 0))
            {
                for (ushort x = 0; x < gs->HorzCenter; x++)
                {
                    graphicsSurfaces->pSurface16[x + (((emuState->LineCounter + gs->VertCenter) * 2) * (emuState->SurfacePitch))] = gs->BorderColor16;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        graphicsSurfaces->pSurface16[x + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * (emuState->SurfacePitch))] = gs->BorderColor16;
                    }

                    graphicsSurfaces->pSurface16[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2) * (emuState->SurfacePitch))] = gs->BorderColor16;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        graphicsSurfaces->pSurface16[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * (emuState->SurfacePitch))] = gs->BorderColor16;
                    }
                }
            }

            if (gs->LinesperRow < 13)
            {
                gs->TagY++;
            }

            if (emuState->LineCounter == Define.FALSE)
            {
                gs->StartofVidram = gs->NewStartofVidram;
                gs->TagY = (ushort)(emuState->LineCounter);
            }

            uint start = (uint)(gs->StartofVidram + (gs->TagY / gs->LinesperRow) * (gs->VPitch * gs->ExtendedText));
            uint yStride = (uint)((((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch) + (gs->HorzCenter * 1) - 1);

            SwitchMasterMode16(emuState, gs->MasterMode, start, yStride);
        }

        public unsafe void UpdateScreen24(EmuState* emuState)
        {
            //--Not implemented
        }

        public unsafe void UpdateScreen32(EmuState* emuState)
        {
            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            uint* szSurface32 = graphicsSurfaces->pSurface32;

            ushort y = (ushort)emuState->LineCounter;
            long Xpitch = emuState->SurfacePitch;

            if ((gs->HorzCenter != 0) && (gs->BorderChange > 0))
            {
                for (ushort x = 0; x < gs->HorzCenter; x++)
                {
                    szSurface32[x + (((y + gs->VertCenter) * 2) * Xpitch)] = gs->BorderColor32;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        szSurface32[x + (((y + gs->VertCenter) * 2 + 1) * Xpitch)] = gs->BorderColor32;
                    }

                    szSurface32[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((y + gs->VertCenter) * 2) * Xpitch)] = gs->BorderColor32;

                    if (emuState->ScanLines == Define.FALSE)
                    {
                        szSurface32[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((y + gs->VertCenter) * 2 + 1) * Xpitch)] = gs->BorderColor32;
                    }
                }
            }

            if (gs->LinesperRow < 13)
            {
                gs->TagY++;
            }

            if (y == Define.FALSE)
            {
                gs->StartofVidram = gs->NewStartofVidram;
                gs->TagY = y;
            }

            uint start = (uint)(gs->StartofVidram + (gs->TagY / gs->LinesperRow) * (gs->VPitch * gs->ExtendedText));
            uint yStride = (uint)((((y + gs->VertCenter) * 2) * Xpitch) + (gs->HorzCenter * 1) - 1);

            SwitchMasterMode32(emuState, gs->MasterMode, start, yStride);
        }

        public unsafe void SwitchMasterMode8(EmuState* emuState, byte masterMode, uint start, uint yStride)
        {
            throw new NotImplementedException("No plans to implement");
        }

        public unsafe void SwitchMasterMode16(EmuState* emuState, byte masterMode, uint start, uint yStride)
        {
            throw new NotImplementedException("No plans to implement");
        }

        public unsafe void SwitchMasterMode32(EmuState* emuState, byte masterMode, uint start, uint yStride)
        {
            byte pixel = 0;
            byte character = 0, attributes = 0;
            uint[] textPalette = { 0, 0 };
            ushort widePixel = 0;
            byte pix = 0, bit = 0, phase = 0;
            byte carry1 = 1, carry2 = 0;
            byte color = 0;

            GraphicsState* gs = _modules.Graphics.GetGraphicsState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();
            GraphicsColors* graphicsColors = _modules.Graphics.GetGraphicsColors();
            TC1014MmuState* mmu = GetTC1014MmuState();

            byte* ramBuffer = mmu->Memory;
            ushort* wRamBuffer = (ushort*)ramBuffer;

            uint* szSurface32 = graphicsSurfaces->pSurface32;

            uint Xpitch = (uint)(emuState->SurfacePitch);
            ushort y = (ushort)(emuState->LineCounter);

            switch (masterMode) // (GraphicsMode <<7) | (CompatMode<<6)  | ((Bpp & 3)<<4) | (Stretch & 15);
            {
                #region case 0  //Width 80

                case 0: //Width 80
                    attributes = 0;

                    if ((gs->HorzOffsetReg & 128) != 0)
                    {
                        start = (uint)(gs->StartofVidram + (gs->TagY / gs->LinesperRow) * (gs->VPitch)); //Fix for Horizontal Offset Register in text mode.
                    }

                    for (ushort beam = 0; beam < gs->BytesperRow * gs->ExtendedText; beam += gs->ExtendedText)
                    {
                        character = ramBuffer[start + (byte)(beam + gs->Hoffset)];
                        pixel = cc3Fontdata8x12[character * 12 + (y % gs->LinesperRow)];

                        if (gs->ExtendedText == 2)
                        {
                            attributes = ramBuffer[start + (byte)(beam + gs->Hoffset) + 1];

                            if (((attributes & 64) != 0) && (y % gs->LinesperRow == (gs->LinesperRow - 1)))
                            {   //UnderLine
                                pixel = 255;
                            }

                            if (_modules.Graphics.CheckState(attributes))
                            {
                                pixel = 0;
                            }
                        }

                        textPalette[1] = graphicsColors->Palette32Bit[8 + ((attributes & 56) >> 3)];
                        textPalette[0] = graphicsColors->Palette32Bit[attributes & 7];
                        szSurface32[yStride += 1] = textPalette[pixel >> 7];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                        szSurface32[yStride += 1] = textPalette[pixel & 1];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (8);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = textPalette[pixel >> 7];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                            szSurface32[yStride += 1] = textPalette[pixel & 1];
                            yStride -= Xpitch;
                        }
                    }

                    break;
                #endregion

                #region case 1-2  //Width 40

                case 1:
                case 2: //Width 40
                    attributes = 0;

                    for (ushort beam = 0; beam < gs->BytesperRow * gs->ExtendedText; beam += gs->ExtendedText)
                    {
                        character = ramBuffer[start + (byte)(beam + gs->Hoffset)];
                        pixel = cc3Fontdata8x12[character * 12 + (y % gs->LinesperRow)];

                        if (gs->ExtendedText == 2)
                        {
                            attributes = ramBuffer[start + (byte)(beam + gs->Hoffset) + 1];

                            if (((attributes & 64) != 0) && (y % gs->LinesperRow == (gs->LinesperRow - 1)))
                            {   //UnderLine
                                pixel = 255;
                            }

                            if (_modules.Graphics.CheckState(attributes))
                            {
                                pixel = 0;
                            }
                        }

                        textPalette[1] = graphicsColors->Palette32Bit[8 + ((attributes & 56) >> 3)];
                        textPalette[0] = graphicsColors->Palette32Bit[attributes & 7];
                        szSurface32[yStride += 1] = textPalette[pixel >> 7];
                        szSurface32[yStride += 1] = textPalette[pixel >> 7];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                        szSurface32[yStride += 1] = textPalette[(pixel & 1)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = textPalette[pixel >> 7];
                            szSurface32[yStride += 1] = textPalette[pixel >> 7];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                            szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                            yStride -= Xpitch;
                        }
                    }

                    break;
                #endregion

                #region case 3-63

                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                    return; //TODO: Why?

                //for (ushort beam = 0; beam < gs->BytesperRow * gs->ExtendedText; beam += gs->ExtendedText)
                //{
                //    character = ramBuffer[start + (byte)(beam + gs->Hoffset)];

                //    if (gs->ExtendedText == 2)
                //    {
                //        attributes = ramBuffer[start + (byte)(beam + gs->Hoffset) + 1];
                //    }
                //    else
                //    {
                //        attributes = 0;
                //    }

                //    pixel = cc3Fontdata8x12[(character & 127) * 8 + (y % 8)];

                //    if (((attributes & 64) != 0) && (y % 8 == 7))
                //    {   //UnderLine
                //        pixel = 255;
                //    }

                //    if (_modules.Graphics.CheckState(attributes))
                //    {
                //        pixel = 0;
                //    }

                //    textPalette[1] = graphicsColors->Palette32Bit[8 + ((attributes & 56) >> 3)];
                //    textPalette[0] = graphicsColors->Palette32Bit[attributes & 7];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 128) / 128];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 64) / 64];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 32) / 32];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 16) / 16];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 8) / 8];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 4) / 4];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 2) / 2];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                //}
                //break;

                #endregion

                #region case 64-127 //Low Res Text

                //******************************************************************** Low Res Text
                case 64:        //Low Res text GraphicsMode=0 CompatMode=1 Ignore Bpp and Stretch
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                    for (ushort beam = 0; beam < gs->BytesperRow; beam++)
                    {
                        character = ramBuffer[start + (byte)(beam + gs->Hoffset)];

                        switch ((character & 192) >> 6)
                        {
                            case 0:
                                character &= 63;
                                textPalette[0] = graphicsColors->Palette32Bit[gs->TextBGPalette];
                                textPalette[1] = graphicsColors->Palette32Bit[gs->TextFGPalette];

                                if ((gs->LowerCase != 0) && (character < 32))
                                    pixel = ntsc_round_fontdata8x12[(character + 80) * 12 + (y % 12)];
                                else
                                    pixel = (byte)~ntsc_round_fontdata8x12[(character) * 12 + (y % 12)];
                                break;

                            case 1:
                                character &= 63;
                                textPalette[0] = graphicsColors->Palette32Bit[gs->TextBGPalette];
                                textPalette[1] = graphicsColors->Palette32Bit[gs->TextFGPalette];
                                pixel = ntsc_round_fontdata8x12[(character) * 12 + (y % 12)];
                                break;

                            case 2:
                            case 3:
                                textPalette[1] = graphicsColors->Palette32Bit[(character & 112) >> 4];
                                textPalette[0] = graphicsColors->Palette32Bit[8];
                                character = (byte)(64 + (character & 0xF));
                                pixel = ntsc_round_fontdata8x12[(character) * 12 + (y % 12)];
                                break;
                        }

                        szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                        szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                        szSurface32[yStride += 1] = textPalette[(pixel & 1)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                            szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                            szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                            yStride -= Xpitch;
                        }
                    }

                    break;

                #endregion

                #region case 128 + 0 //Bpp=0 Sr=0 1BPP Stretch=1

                case 128 + 0:
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (1-2) //Bpp=0 Sr=1 1BPP Stretch=2

                case 128 + 1: //Bpp=0 Sr=1 1BPP Stretch=2
                case 128 + 2:   //Bpp=0 Sr=2 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (32);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (3-6) //Bpp=0 Sr=3 1BPP Stretch=4

                case 128 + 3: //Bpp=0 Sr=3 1BPP Stretch=4
                case 128 + 4: //Bpp=0 Sr=4
                case 128 + 5: //Bpp=0 Sr=5
                case 128 + 6: //Bpp=0 Sr=6
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (64);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (7-14) //Bpp=0 Sr=7 1BPP Stretch=8 

                case 128 + 7: //Bpp=0 Sr=7 1BPP Stretch=8 
                case 128 + 8: //Bpp=0 Sr=8
                case 128 + 9: //Bpp=0 Sr=9
                case 128 + 10: //Bpp=0 Sr=10
                case 128 + 11: //Bpp=0 Sr=11
                case 128 + 12: //Bpp=0 Sr=12
                case 128 + 13: //Bpp=0 Sr=13
                case 128 + 14: //Bpp=0 Sr=14
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (128);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (15-16) //Bpp=0 Sr=15 1BPP Stretch=16

                case 128 + 15: //Bpp=0 Sr=15 1BPP Stretch=16
                case 128 + 16: //BPP=1 Sr=0  2BPP Stretch=1
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (8);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (17-18) //Bpp=1 Sr=1  2BPP Stretch=2

                case 128 + 17: //Bpp=1 Sr=1  2BPP Stretch=2
                case 128 + 18: //Bpp=1 Sr=2
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }

                    break;

                #endregion

                #region case 128 + (19-22) //Bpp=1 Sr=3  2BPP Stretch=4

                case 128 + 19: //Bpp=1 Sr=3  2BPP Stretch=4
                case 128 + 20: //Bpp=1 Sr=4
                case 128 + 21: //Bpp=1 Sr=5
                case 128 + 22: //Bpp=1 Sr=6
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (32);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (23-30) //Bpp=1 Sr=7  2BPP Stretch=8

                case 128 + 23: //Bpp=1 Sr=7  2BPP Stretch=8
                case 128 + 24: //Bpp=1 Sr=8
                case 128 + 25: //Bpp=1 Sr=9 
                case 128 + 26: //Bpp=1 Sr=10 
                case 128 + 27: //Bpp=1 Sr=11 
                case 128 + 28: //Bpp=1 Sr=12 
                case 128 + 29: //Bpp=1 Sr=13 
                case 128 + 30: //Bpp=1 Sr=14
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (64);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 31 //Bpp=1 Sr=15 2BPP Stretch=16 

                case 128 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (128);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 32 //Bpp=2 Sr=0 4BPP Stretch=1

                case 128 + 32: //Bpp=2 Sr=0 4BPP Stretch=1
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //4bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (4);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (33-34) //Bpp=2 Sr=1 4BPP Stretch=2 

                case 128 + 33: //Bpp=2 Sr=1 4BPP Stretch=2 
                case 128 + 34: //Bpp=2 Sr=2
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //4bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (8);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (35-38) //Bpp=2 Sr=3 4BPP Stretch=4

                case 128 + 35: //Bpp=2 Sr=3 4BPP Stretch=4
                case 128 + 36: //Bpp=2 Sr=4 
                case 128 + 37: //Bpp=2 Sr=5 
                case 128 + 38: //Bpp=2 Sr=6 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //4bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 12 + (39-46) //Bpp=2 Sr=7 4BPP Stretch=8

                case 128 + 39: //Bpp=2 Sr=7 4BPP Stretch=8
                case 128 + 40: //Bpp=2 Sr=8 
                case 128 + 41: //Bpp=2 Sr=9 
                case 128 + 42: //Bpp=2 Sr=10 
                case 128 + 43: //Bpp=2 Sr=11 
                case 128 + 44: //Bpp=2 Sr=12 
                case 128 + 45: //Bpp=2 Sr=13 
                case 128 + 46: //Bpp=2 Sr=14 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //4bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (32);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 47: //Bpp=2 Sr=15 4BPP Stretch=16

                case 128 + 47: //Bpp=2 Sr=15 4BPP Stretch=16
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //4bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (64);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (48-63) //Bpp=3 Sr=0  Unsupported 

                case 128 + 48: //Bpp=3 Sr=0  Unsupported 
                case 128 + 49: //Bpp=3 Sr=1 
                case 128 + 50: //Bpp=3 Sr=2 
                case 128 + 51: //Bpp=3 Sr=3 
                case 128 + 52: //Bpp=3 Sr=4 
                case 128 + 53: //Bpp=3 Sr=5 
                case 128 + 54: //Bpp=3 Sr=6 
                case 128 + 55: //Bpp=3 Sr=7 
                case 128 + 56: //Bpp=3 Sr=8 
                case 128 + 57: //Bpp=3 Sr=9 
                case 128 + 58: //Bpp=3 Sr=10 
                case 128 + 59: //Bpp=3 Sr=11 
                case 128 + 60: //Bpp=3 Sr=12 
                case 128 + 61: //Bpp=3 Sr=13 
                case 128 + 62: //Bpp=3 Sr=14 
                case 128 + 63: //Bpp=3 Sr=15 
                    return;

                #endregion

                #region case 192 + 0: //Bpp=0 Sr=0 1BPP Stretch=1

                case 192 + 0: //Bpp=0 Sr=0 1BPP Stretch=1
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (1-2): //Bpp=0 Sr=1 1BPP Stretch=2

                case 192 + 1: //Bpp=0 Sr=1 1BPP Stretch=2
                case 192 + 2:   //Bpp=0 Sr=2 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];

                        if (gs->MonType == 0)
                        { //Pcolor
                            for (byte xbit = 8; xbit > 0; xbit--)
                            {
                                bit = (byte)(xbit - 1); //TODO: work-around that a byte is always positive
                                pix = (byte)(1 & (widePixel >> bit));
                                phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

                                switch (phase)
                                {
                                    case 0:
                                    case 4:
                                    case 6:
                                        color = 0;
                                        break;

                                    case 1:
                                    case 5:
                                        color = (byte)((bit & 1) + 1);
                                        break;

                                    case 2:
                                        break;

                                    case 3:
                                        color = 3;
                                        szSurface32[yStride - 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];

                                        if (emuState->ScanLines == Define.FALSE)
                                        {
                                            szSurface32[yStride + Xpitch - 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];
                                        }

                                        szSurface32[yStride] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];

                                        if (emuState->ScanLines == Define.FALSE)
                                        {
                                            szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];
                                        }

                                        break;

                                    case 7:
                                        color = 3;
                                        break;
                                }

                                szSurface32[yStride += 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];

                                if (emuState->ScanLines == Define.FALSE)
                                {
                                    szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];
                                }

                                szSurface32[yStride += 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];

                                if (emuState->ScanLines == Define.FALSE)
                                {
                                    szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];
                                }

                                carry2 = carry1;
                                carry1 = pix;
                            }

                            for (bit = 15; bit >= 8; bit--)
                            {
                                pix = (byte)(1 & (widePixel >> bit));
                                phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

                                switch (phase)
                                {
                                    case 0:
                                    case 4:
                                    case 6:
                                        color = 0;
                                        break;

                                    case 1:
                                    case 5:
                                        color = (byte)((bit & 1) + 1);
                                        break;

                                    case 2:
                                        break;

                                    case 3:
                                        color = 3;
                                        szSurface32[yStride - 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];

                                        if (emuState->ScanLines == Define.FALSE)
                                        {
                                            szSurface32[yStride + Xpitch - 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];
                                        }

                                        szSurface32[yStride] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];

                                        if (emuState->ScanLines == Define.FALSE)
                                        {
                                            szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + 3];
                                        }

                                        break;

                                    case 7:
                                        color = 3;
                                        break;
                                }

                                szSurface32[yStride += 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];

                                if (emuState->ScanLines == Define.FALSE)
                                {
                                    szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];
                                }

                                szSurface32[yStride += 1] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];

                                if (emuState->ScanLines == Define.FALSE)
                                {
                                    szSurface32[yStride + Xpitch] = graphicsColors->Afacts32[gs->ColorInvert * 4 + color];
                                }

                                carry2 = carry1;
                                carry1 = pix;
                            }
                        }
                        else
                        {
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];

                            if (emuState->ScanLines == Define.FALSE)
                            {
                                yStride -= (32);
                                yStride += Xpitch;
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                                szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                                yStride -= Xpitch;
                            }
                        }
                    }
                    break;

                #endregion

                #region case 192 + (3-6): //Bpp=0 Sr=3 1BPP Stretch=4

                case 192 + 3: //Bpp=0 Sr=3 1BPP Stretch=4
                case 192 + 4: //Bpp=0 Sr=4
                case 192 + 5: //Bpp=0 Sr=5
                case 192 + 6: //Bpp=0 Sr=6
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (64);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (7-14): //Bpp=0 Sr=7 1BPP Stretch=8 

                case 192 + 7: //Bpp=0 Sr=7 1BPP Stretch=8 
                case 192 + 8: //Bpp=0 Sr=8
                case 192 + 9: //Bpp=0 Sr=9
                case 192 + 10: //Bpp=0 Sr=10
                case 192 + 11: //Bpp=0 Sr=11
                case 192 + 12: //Bpp=0 Sr=12
                case 192 + 13: //Bpp=0 Sr=13
                case 192 + 14: //Bpp=0 Sr=14
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //1bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (128);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (15-16): //Bpp=0 Sr=15 1BPP Stretch=16

                case 192 + 15: //Bpp=0 Sr=15 1BPP Stretch=16
                case 192 + 16: //BPP=1 Sr=0  2BPP Stretch=1
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (8);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (17-18): //Bpp=1 Sr=1  2BPP Stretch=2

                case 192 + 17: //Bpp=1 Sr=1  2BPP Stretch=2
                case 192 + 18: //Bpp=1 Sr=2
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (16);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + (19-22): //Bpp=1 Sr=3  2BPP Stretch=4

                case 192 + 19: //Bpp=1 Sr=3  2BPP Stretch=4
                case 192 + 20: //Bpp=1 Sr=4
                case 192 + 21: //Bpp=1 Sr=5
                case 192 + 22: //Bpp=1 Sr=6
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (32);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (23-30): //Bpp=1 Sr=7  2BPP Stretch=8

                case 192 + 23: //Bpp=1 Sr=7  2BPP Stretch=8
                case 192 + 24: //Bpp=1 Sr=8
                case 192 + 25: //Bpp=1 Sr=9 
                case 192 + 26: //Bpp=1 Sr=10 
                case 192 + 27: //Bpp=1 Sr=11 
                case 192 + 28: //Bpp=1 Sr=12 
                case 192 + 29: //Bpp=1 Sr=13 
                case 192 + 30: //Bpp=1 Sr=14
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (64);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 

                case 192 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 
                    for (ushort beam = 0; beam < gs->BytesperRow; beam += 2) //2bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(gs->VidMask & (start + (byte)(gs->Hoffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];

                        if (emuState->ScanLines == Define.FALSE)
                        {
                            yStride -= (128);
                            yStride += Xpitch;
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors->Palette32Bit[gs->PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= Xpitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + (32-63): //Bpp=2 Sr=0 4BPP Stretch=1 Unsupport with Compat set

                case 192 + 32: //Bpp=2 Sr=0 4BPP Stretch=1 Unsupport with Compat set
                case 192 + 33: //Bpp=2 Sr=1 4BPP Stretch=2 
                case 192 + 34: //Bpp=2 Sr=2
                case 192 + 35: //Bpp=2 Sr=3 4BPP Stretch=4
                case 192 + 36: //Bpp=2 Sr=4 
                case 192 + 37: //Bpp=2 Sr=5 
                case 192 + 38: //Bpp=2 Sr=6 
                case 192 + 39: //Bpp=2 Sr=7 4BPP Stretch=8
                case 192 + 40: //Bpp=2 Sr=8 
                case 192 + 41: //Bpp=2 Sr=9 
                case 192 + 42: //Bpp=2 Sr=10 
                case 192 + 43: //Bpp=2 Sr=11 
                case 192 + 44: //Bpp=2 Sr=12 
                case 192 + 45: //Bpp=2 Sr=13 
                case 192 + 46: //Bpp=2 Sr=14 
                case 192 + 47: //Bpp=2 Sr=15 4BPP Stretch=16
                case 192 + 48: //Bpp=3 Sr=0  Unsupported 
                case 192 + 49: //Bpp=3 Sr=1 
                case 192 + 50: //Bpp=3 Sr=2 
                case 192 + 51: //Bpp=3 Sr=3 
                case 192 + 52: //Bpp=3 Sr=4 
                case 192 + 53: //Bpp=3 Sr=5 
                case 192 + 54: //Bpp=3 Sr=6 
                case 192 + 55: //Bpp=3 Sr=7 
                case 192 + 56: //Bpp=3 Sr=8 
                case 192 + 57: //Bpp=3 Sr=9 
                case 192 + 58: //Bpp=3 Sr=10 
                case 192 + 59: //Bpp=3 Sr=11 
                case 192 + 60: //Bpp=3 Sr=12 
                case 192 + 61: //Bpp=3 Sr=13 
                case 192 + 62: //Bpp=3 Sr=14 
                case 192 + 63: //Bpp=3 Sr=15 
                    return;


                #endregion
            }
        }
    }
}
