using System;
using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IGraphics
    {
        unsafe GraphicsState* GetGraphicsState();
        unsafe GraphicsSurfaces* GetGraphicsSurfaces();
        unsafe GraphicsColors* GetGraphicsColors();

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
        bool CheckState(byte attributes);
    }

    public class Graphics : IGraphics
    {
        private readonly int[] red =
        {
            0, 14, 12, 21, 51, 86, 108, 118, 113, 92, 61, 21, 1, 5, 12, 13,
            50, 29, 49, 86, 119, 158, 179, 192, 186, 165, 133, 94, 23, 16, 23, 25,
            116, 74, 102, 142, 179, 219, 243, 252, 251, 230, 198, 155, 81, 61, 52, 57,
            253, 137, 161, 189, 215, 240, 253, 253, 251, 237, 214, 183, 134, 121, 116, 255
        };

        private readonly int[] green =
        {
            0, 78, 69, 53, 33, 4, 1, 1, 12, 24, 31, 35, 37, 51, 67, 77,
            50, 149, 141, 123, 103, 77, 55, 39, 35, 43, 53, 63, 100, 119, 137, 148,
            116, 212, 204, 186, 164, 137, 114, 97, 88, 89, 96, 109, 156, 179, 199, 211,
            253, 230, 221, 207, 192, 174, 158, 148, 143, 144, 150, 162, 196, 212, 225, 255
        };

        private readonly int[] blue =
        {
            0, 20, 18, 14, 10, 10, 12, 19, 76, 135, 178, 196, 148, 97, 29, 20,
            50, 38, 36, 32, 28, 25, 24, 78, 143, 207, 248, 249, 228, 174, 99, 46,
            116, 58, 52, 48, 44, 41, 68, 132, 202, 250, 250, 250, 251, 243, 163, 99,
            254, 104, 83, 77, 82, 105, 142, 188, 237, 251, 251, 251, 252, 240, 183, 255
        };

        private readonly IModules _modules;
        
        public Graphics(IModules modules)
        {
            _modules = modules;
        }

        public unsafe GraphicsState* GetGraphicsState()
        {
            return Library.Graphics.GetGraphicsState();
        }

        public unsafe GraphicsSurfaces* GetGraphicsSurfaces()
        {
            return Library.Graphics.GetGraphicsSurfaces();
        }

        public unsafe GraphicsColors* GetGraphicsColors()
        {
            return Library.Graphics.GetGraphicsColors();
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

        public void FlipArtifacts()
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                graphicsState->ColorInvert = graphicsState->ColorInvert == Define.FALSE ? Define.TRUE : Define.FALSE;
            }
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

        public void MakeCMPPalette(int paletteType)
        {
            Debug.WriteLine(paletteType == 1 ? "Loading new CMP palette." : "Loading old CMP palette.");

            float gamma = 1.4f;
            double r, g, b;

            for (byte index = 0; index <= 63; index++)
            {
                if (paletteType == 1)
                {
                    if (index > 39) { gamma = 1.1f; }

                    if (index > 55) { gamma = 1; }

                    r = red[index] * (double)gamma; if (r > 255) { r = 255; }
                    g = green[index] * (double)gamma; if (g > 255) { g = 255; }
                    b = blue[index] * (double)gamma; if (b > 255) { b = 255; }
                }
                else //Old palette //Stolen from M.E.S.S.
                {
                    switch (index)
                    {
                        case 0:
                            r = g = b = 0;
                            break;

                        case 16:
                            r = g = b = 47;
                            break;

                        case 32:
                            r = g = b = 120;
                            break;

                        case 48:
                        case 63:
                            r = g = b = 255;
                            break;

                        default:
                            double w = .4195456981879 * 1.01;
                            double contrast = 70;
                            double saturation = 92;
                            double brightness = -20;
                            brightness += (((double)index / 16) + (double)1) * contrast;
                            int offset = (index % 16) - 1 + (index / 16) * 15;
                            r = Math.Cos(w * (offset + 9.2)) * saturation + brightness;
                            g = Math.Cos(w * (offset + 14.2)) * saturation + brightness;
                            b = Math.Cos(w * (offset + 19.2)) * saturation + brightness;

                            if (r < 0)
                            {
                                r = 0;
                            }
                            else if (r > 255)
                            {
                                r = 255;
                            }

                            if (g < 0)
                            {
                                g = 0;
                            }
                            else if (g > 255)
                            {
                                g = 255;
                            }

                            if (b < 0)
                            {
                                b = 0;
                            }
                            else if (b > 255)
                            {
                                b = 255;
                            }

                            break;
                    }
                }

                SetPaletteLookup(index, (byte)r, (byte)g, (byte)b);
            }
        }

        public void SetPaletteType()
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                byte borderColor = graphicsState->CC3BorderColor;

                SetGimeBorderColor(0);
                MakeCMPPalette(_modules.Config.GetPaletteType());
                SetGimeBorderColor(borderColor);
            }
        }

        public void SetMonitorType(byte type)
        {
            unsafe
            {
                GraphicsState* graphicsState = GetGraphicsState();

                byte borderColor = graphicsState->CC3BorderColor;

                SetGimeBorderColor(0);

                graphicsState->MonType = (type & 1) == 0 ? Define.FALSE : Define.TRUE;

                for (byte palIndex = 0; palIndex < 16; palIndex++)
                {
                    SetMonitorTypePalettes(graphicsState->MonType, palIndex);
                }

                SetGimeBorderColor(borderColor);
            }
        }

        public void InvalidateBorder()
        {
            Library.Graphics.InvalidateBorder();
        }

        public void MakeRGBPalette()
        {
            for (byte index = 0; index < 64; index++)
            {
                Library.Graphics.MakeRGBPalette(index);
            }
        }

        public void SetMonitorTypePalettes(byte monType, byte palIndex)
        {
            Library.Graphics.SetMonitorTypePalettes(monType, palIndex);
        }

        public void SetGimeBorderColor(byte data)
        {
            Library.Graphics.SetGimeBorderColor(data);
        }

        public void SetPaletteLookup(byte index, byte r, byte g, byte b)
        {
            Library.Graphics.SetPaletteLookup(index, r, g, b);
        }

        public bool CheckState(byte attributes)
        {
            return Library.Graphics.CheckState(attributes) != Define.FALSE;
        }
    }
}
