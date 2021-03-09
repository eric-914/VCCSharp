using VCCSharp.Libraries;

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
    }
}
