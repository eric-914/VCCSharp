using VCCSharp.Enums;
using VCCSharp.IoC;
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
        void SetMonitorType(byte type);
        void FlipArtifacts();
        void InvalidateBorder();
    }

    public class Graphics : IGraphics
    {
        private readonly IModules _modules;

        public Graphics(IModules modules)
        {
            _modules = modules;
        }

        public unsafe GraphicsState* GetGraphicsState()
        {
            return Library.Graphics.GetGraphicsState();
        }

        public void ResetGraphicsState()
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                graphicsState->CC3Vmode = 0;
                graphicsState->CC3Vres = 0;
                graphicsState->StartofVidram = 0;
                graphicsState->NewStartofVidram = 0;
                graphicsState->GraphicsMode = 0;
                graphicsState->LowerCase = 0;
                graphicsState->InvertAll = 0;
                graphicsState->ExtendedText = 1;
                graphicsState->HorzOffsetReg = 0;
                graphicsState->TagY = 0;
                graphicsState->DistoOffset = 0;
                graphicsState->BorderChange = 3;
                graphicsState->CC2Offset = 0;
                graphicsState->Hoffset = 0;
                graphicsState->VerticalOffsetRegister = 0;
            }
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

        public void SetMonitorType(byte type)
        {
            Library.Graphics.SetMonitorType(type);
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

        public void MakeRGBPalette()
        {
            Library.Graphics.MakeRGBPalette();
        }

        public void MakeCMPPalette(int paletteType)
        {
            Library.Graphics.MakeCMPPalette(paletteType);
        }

        //TODO: ScanLines never really worked right to begin with...
        public unsafe void SetScanLines(EmuState* emuState, byte lines)
        {
            GraphicsState* graphicsState = GetGraphicsState();

            emuState->ScanLines = lines;
            emuState->ResetPending = (byte)ResetPendingStates.Cls;

            _modules.DirectDraw.ClearScreen();

            graphicsState->BorderChange = 3;
        }
    }
}
