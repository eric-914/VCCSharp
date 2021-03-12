using VCCSharp.Libraries;
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
        void SetBorderChange();
        void SetVidMask(uint mask);
        void SetPaletteType();
        unsafe void SetScanLines(EmuState* emuState, byte lines);
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
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                graphicsState->BlinkState = state;
            }
        }

        public void SetBorderChange()
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                if (graphicsState->BorderChange > 0)
                {
                    graphicsState->BorderChange--;
                }

            }
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

        public unsafe void SetScanLines(EmuState* emuState, byte lines)
        {
            Library.Graphics.SetScanLines(emuState, lines);
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
