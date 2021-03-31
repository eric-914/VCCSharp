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

        public void MemWrite8(byte data, ushort address)
        {
            Library.TC1014.MemWrite8(data, address);
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

            if (gs->BorderChange == 0) {
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

            if ((gs->HorzCenter != 0) && (gs->BorderChange > 0)) {
                for (ushort x = 0; x < gs->HorzCenter; x++)
                {
                    graphicsSurfaces->pSurface8[x + (((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch)] = gs->BorderColor8;

                    if (emuState->ScanLines == Define.FALSE) {
                        graphicsSurfaces->pSurface8[x + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor8;
                    }

                    graphicsSurfaces->pSurface8[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2) * emuState->SurfacePitch)] = gs->BorderColor8;

                    if (emuState->ScanLines == Define.FALSE) {
                        graphicsSurfaces->pSurface8[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * emuState->SurfacePitch)] = gs->BorderColor8;
                    }
                }
            }

            if (gs->LinesperRow < 13) {
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

                    if (emuState->ScanLines == Define.FALSE) {
                        graphicsSurfaces->pSurface16[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((emuState->LineCounter + gs->VertCenter) * 2 + 1) * (emuState->SurfacePitch))] = gs->BorderColor16;
                    }
                }
            }

            if (gs->LinesperRow < 13) {
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

            if ((gs->HorzCenter != 0) && (gs->BorderChange > 0)) {
                for (ushort x = 0; x < gs->HorzCenter; x++)
                {
                    szSurface32[x + (((y + gs->VertCenter) * 2) * Xpitch)] = gs->BorderColor32;

                    if (emuState->ScanLines == Define.FALSE) {
                        szSurface32[x + (((y + gs->VertCenter) * 2 + 1) * Xpitch)] = gs->BorderColor32;
                    }

                    szSurface32[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((y + gs->VertCenter) * 2) * Xpitch)] = gs->BorderColor32;

                    if (emuState->ScanLines == Define.FALSE) {
                        szSurface32[x + (gs->PixelsperLine * (gs->Stretch + 1)) + gs->HorzCenter + (((y + gs->VertCenter) * 2 + 1) * Xpitch)] = gs->BorderColor32;
                    }
                }
            }

            if (gs->LinesperRow < 13) {
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
            Library.TC1014.SwitchMasterMode8(emuState, masterMode, start, yStride);
        }

        public unsafe void SwitchMasterMode16(EmuState* emuState, byte masterMode, uint start, uint yStride)
        {
            Library.TC1014.SwitchMasterMode16(emuState, masterMode, start, yStride);
        }

        public unsafe void SwitchMasterMode32(EmuState* emuState, byte masterMode, uint start, uint yStride)
        {
            Library.TC1014.SwitchMasterMode32(emuState, masterMode, start, yStride);
        }
    }
}
