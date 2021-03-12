﻿using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IGraphics
    {
        unsafe GraphicsState* GetGraphicsState();
        void ResetGraphicsState();
        void MakeRGBPalette();
        void MakeCMPPalette(int paletteType);
        void SetBlinkState(byte state);
        void SetBorderChange(byte data);
        void SetVidMask(uint mask);
        void SetPaletteType();
        unsafe byte SetScanLines(EmuState* emuState, byte lines);
        byte SetMonitorType(byte type);
        void FlipArtifacts();
        void InvalidateBorder();
    }

    public class Graphics : IGraphics
    {
        public unsafe GraphicsState* GetGraphicsState()
        {
            return Library.Graphics.GetGraphicsState();
        }

        public void ResetGraphicsState()
        {
            Library.Graphics.ResetGraphicsState();
        }

        public void MakeRGBPalette()
        {
            Library.Graphics.MakeRGBPalette();
        }

        public void MakeCMPPalette(int paletteType)
        {
            Library.Graphics.MakeCMPPalette(paletteType);
        }

        public void SetBlinkState(byte state)
        {
            Library.Graphics.SetBlinkState(state);
        }

        public void SetBorderChange(byte data)
        {
            Library.Graphics.SetBorderChange(data);
        }

        public void SetVidMask(uint mask)
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                graphicsState->VidMask = mask;
            }
        }

        public void SetPaletteType()
        {
            Library.Graphics.SetPaletteType();
        }

        public unsafe byte SetScanLines(EmuState* emuState, byte lines)
        {
            return Library.Graphics.SetScanLines(emuState, lines);
        }

        public byte SetMonitorType(byte type)
        {
            return Library.Graphics.SetMonitorType(type);
        }

        public void FlipArtifacts()
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                graphicsState->ColorInvert = graphicsState->ColorInvert == Define.FALSE ? Define.TRUE : Define.FALSE;
            }
        }

        public void InvalidateBorder()
        {
            Library.Graphics.InvalidateBorder();
        }
    }
}
