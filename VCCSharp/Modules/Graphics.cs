using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IGraphics
    {
        void ResetGraphicsState();
        void MakeRGBPalette();
        void MakeCMPPalette(int paletteType);
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
    }
}
