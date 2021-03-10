using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IGraphics
    {
        void ResetGraphicsState();
        void MakeRGBPalette();
        void MakeCMPPalette(int paletteType);
        void SetBlinkState(byte state);
        void SetBorderChange(byte data);
        void SetVidMask(uint mask);
        void SetPaletteType();
        unsafe byte SetScanLines(EmuState* emuState, byte lines);
        byte SetMonitorType(byte type);
    }

    public class Graphics : IGraphics
    {
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
            Library.Graphics.SetVidMask(mask);
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
    }
}
