using System;
using System.Text;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;
using Point = System.Drawing.Point;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        unsafe DirectDrawState* GetDirectDrawState();
        bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        void ClearScreen();
        void FullScreenToggle();
        unsafe bool CreateDirectDrawWindow(EmuState* emuState);
        unsafe void SetStatusBarText(string textBuffer, EmuState* emuState);
        unsafe float Static(EmuState* emuState);
        unsafe void DoCls(EmuState* emuState);
        unsafe byte LockScreen(EmuState* emuState);
        unsafe void UnlockScreen(EmuState* emuState);
        void SetAspect(byte forceAspect);
        unsafe void DisplayFlip(EmuState* emuState);
    }

    public class DirectDraw : IDirectDraw
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;

        private static int _textX = 0, _textY = 0;
        private static byte _counter = 0, _counter1 = 32, _phase = 1;

        public DirectDraw(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        public unsafe DirectDrawState* GetDirectDrawState()
        {
            return Library.DirectDraw.GetDirectDrawState();
        }

        public void FullScreenToggle()
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                _modules.Audio.PauseAudio(Define.TRUE);

                if (!CreateDirectDrawWindow(emuState))
                {
                    MessageBox.Show("Can't rebuild primary Window", "Error");

                    Environment.Exit(0);
                }

                _modules.Graphics.InvalidateBorder();
                _modules.Callbacks.RefreshDynamicMenu(emuState);

                //TODO: Guess it wants to close other windows/dialogs
                emuState->ConfigDialog = Zero;

                _modules.Audio.PauseAudio(Define.FALSE);
            }
        }

        public void SetAspect(byte forceAspect)
        {
            unsafe
            {
                DirectDrawState* instance = GetDirectDrawState();

                instance->ForceAspect = forceAspect;
            }
        }

        public unsafe void DoCls(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (LockScreen(emuState) == Define.TRUE)
            {
                return;
            }

            switch ((BitDepthStates)(emuState->BitDepth))
            {
                case BitDepthStates.BIT_8:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface8[x + (y * emuState->SurfacePitch)] = (byte)(instance->Color | 128);
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface16[x + (y * emuState->SurfacePitch)] = instance->Color;
                        }
                    }
                    break;

                case BitDepthStates.BIT_24:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = (byte)((instance->Color & 0xFF0000) >> 16);
                            graphicsSurfaces->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = (byte)((instance->Color & 0x00FF00) >> 8);
                            graphicsSurfaces->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = (byte)(instance->Color & 0xFF);
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch)] = instance->Color;
                        }
                    }
                    break;
            }

            UnlockScreen(emuState);
        }

        public void ClearScreen()
        {
            unsafe
            {
                GetDirectDrawState()->Color = 0;
            }
        }

        public unsafe float Static(EmuState* emuState)
        {
            Static(emuState, _modules.Graphics.GetGraphicsSurfaces());

            return _modules.Throttle.CalculateFPS();
        }

        private unsafe void Static(EmuState* emuState, GraphicsSurfaces* graphicsSurfaces)
        {
            var random = new Random();

            LockScreen(emuState);

            if (graphicsSurfaces->pSurface32 == null)
            {
                return; //TODO: Seems bad to exit w/out unlocking first
            }

            switch ((BitDepthStates)(emuState->BitDepth))
            {
                case BitDepthStates.BIT_8:
                    byte[] greyScales = { 128, 135, 184, 191 };

                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 160; x++)
                        {
                            byte temp = (byte)(random.Next() & 3);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                            graphicsSurfaces->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 320; x++)
                        {
                            byte temp = (byte)(random.Next() & 31);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                            graphicsSurfaces->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                        }
                    }
                    break;

                case BitDepthStates.BIT_24: //--TODO: Don't think this was ever tested
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);
                            graphicsSurfaces->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = temp;
                            graphicsSurfaces->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = temp;
                            graphicsSurfaces->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = temp;
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);

                            graphicsSurfaces->pSurface32[x + (y * emuState->SurfacePitch)] = (uint)(temp | (temp << 8) | (temp << 16));
                        }
                    }
                    break;
            }

            byte color = (byte)(_counter1 << 2);

            ShowStaticMessage((ushort)_textX, (ushort)_textY, (uint)(color << 16 | color << 8 | color));

            _counter++;
            _counter1 += _phase;

            if ((_counter1 == 60) || (_counter1 == 20))
            {
                _phase = (byte)(-_phase);
            }

            _counter %= 60; //about 1 seconds

            if (_counter == 0)
            {
                _textX = (ushort)(random.Next() % 580);
                _textY = (ushort)(random.Next() % 470);
            }

            UnlockScreen(emuState);
        }

        public unsafe void UnlockScreen(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.TRUE && instance->InfoBand == Define.TRUE)
            {
                WriteStatusText(Converter.ToString(instance->StatusText));
            }

            UnlockSurface();

            DisplayFlip(emuState);
        }

        public void ShowStaticMessage(ushort x, ushort y, uint color)
        {
            unsafe
            {
                const string message = " Signal Missing! Press F9";
                void* hdc;

                GetSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0);
                _modules.GDI.GDISetTextColor(hdc, color);
                _modules.GDI.GDIWriteTextOut(hdc, x, y, message);

                ReleaseSurfaceDC(hdc);
            }
        }

        public void WriteStatusText(string statusText)
        {
            unsafe
            {
                void* hdc;

                var sb = new StringBuilder(statusText.PadRight(134 - statusText.Length));

                GetSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0); //RGB(0, 0, 0)
                _modules.GDI.GDISetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
                _modules.GDI.GDITextOut(hdc, 0, 0, sb.ToString(), 132);

                ReleaseSurfaceDC(hdc);
            }
        }

        public unsafe void SetStatusBarText(string text, EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.FALSE)
            {
                SetStatusBarText(text);
            }
            else
            {
                Converter.ToByteArray(text, instance->StatusText);
            }
        }

        private void SetStatusBarText(string text)
        {
            Library.DirectDraw.SetStatusBarText(text);
        }

        public unsafe void DisplayFlip(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.TRUE)
            {	// if we're windowed do the blit, else just Flip
                SurfaceFlip();
            }
            else
            {
                var p = new Point(0, 0);
                RECT rcSrc;  // source blit rectangle
                RECT rcDest; // destination blit rectangle

                // The ClientToScreen function converts the client-area coordinates of a specified point to screen coordinates.
                // in other word the client rectangle of the main windows 0, 0 (upper-left corner) 
                // in a screen x,y coords which is put back into p  
                _user32.ClientToScreen(emuState->WindowHandle, &p);  // find out where on the primary surface our window lives

                // get the actual client rectangle, which is always 0,0 - w,h
                _user32.GetClientRect(emuState->WindowHandle, &rcDest);

                // The OffsetRect function moves the specified rectangle by the specified offsets
                // add the delta screen point we got above, which gives us the client rect in screen coordinates.
                _user32.OffsetRect(&rcDest, (int)p.X, (int)p.Y);

                // our destination rectangle is going to be 
                _user32.SetRect(&rcSrc, 0, 0, (short)instance->WindowSize.X, (short)instance->WindowSize.Y);

                //if (instance->Resizeable)
                //if (true) //--Currently, this is fixed at always resizable
                //{

                rcDest.bottom -= instance->StatusBarHeight;

                if (instance->ForceAspect == Define.TRUE) // Adjust the Aspect Ratio if window is resized
                {
                    float srcWidth = (float)instance->WindowSize.X;
                    float srcHeight = (float)instance->WindowSize.Y;
                    float srcRatio = srcWidth / srcHeight;

                    // change this to use the existing rcDest and the calc, w = right-left & h = bottom-top, 
                    //                         because rcDest has already been converted to screen cords, right?   
                    RECT rcClient;

                    _user32.GetClientRect(emuState->WindowHandle, &rcClient);  // x,y is always 0,0 so right, bottom is w,h

                    rcClient.bottom -= instance->StatusBarHeight;

                    float clientWidth = (float)rcClient.right;
                    float clientHeight = (float)rcClient.bottom;
                    float clientRatio = clientWidth / clientHeight;

                    float dstWidth = 0, dstHeight = 0;

                    if (clientRatio > srcRatio)
                    {
                        dstWidth = srcWidth * clientHeight / srcHeight;
                        dstHeight = clientHeight;
                    }
                    else
                    {
                        dstWidth = clientWidth;
                        dstHeight = srcHeight * clientWidth / srcWidth;
                    }
                    
                    float dstX = (clientWidth - dstWidth) / 2;
                    float dstY = (clientHeight - dstHeight) / 2;

                    Point pDstLeftTop = new Point { X = (int)dstX, Y = (int)dstY };

                    _user32.ClientToScreen(emuState->WindowHandle, &pDstLeftTop);

                    Point pDstRightBottom = new Point { X = (int)(dstX + dstWidth), Y = (int)(dstY + dstHeight) };

                    _user32.ClientToScreen(emuState->WindowHandle, &pDstRightBottom);

                    _user32.SetRect(&rcDest, (short)pDstLeftTop.X, (short)pDstLeftTop.Y, (short)pDstRightBottom.X, (short)pDstRightBottom.Y);
                }

                //}
                //else
                //{
                //    // this does not seem ideal, it lets you begin to resize and immediately resizes it back ... causing a lot of flicker.
                //    rcDest.right = rcDest.left + (int)instance->WindowSize.X;
                //    rcDest.bottom = rcDest.top + (int)instance->WindowSize.Y;

                //    RECT defaultRect = GetWindowDefaultSize();

                //    _user32.GetWindowRect(emuState->WindowHandle, &rect);
                //    _user32.MoveWindow(emuState->WindowHandle, rect.left, rect.top, defaultRect.right - defaultRect.left, defaultRect.bottom - defaultRect.top, 1);
                //}

                if (!HasBackSurface())
                {
                    MessageBox.Show("Odd", "Error"); // yes, odd error indeed!! (??) especially since we go ahead and use it below!
                }

                SurfaceBlt(&rcDest, &rcSrc);
            }

            //--Store the updated WindowSizeX/Y for configuration, later.
            RECT windowSize;

            _user32.GetClientRect(emuState->WindowHandle, &windowSize);

            emuState->WindowSize.X = windowSize.right;
            emuState->WindowSize.Y = windowSize.bottom - instance->StatusBarHeight;
        }

        public unsafe byte LockScreen(EmuState* emuState)
        {
            DDSURFACEDESC* ddsd = DDSDCreate();  // A structure to describe the surfaces we want

            CheckSurfaces();

            // Lock entire surface, wait if it is busy, return surface memory pointer
            int hr = LockSurface(ddsd);

            if (hr < 0)
            {
                return(1);
            }

            uint rgbBitCount = DDSDRGBBitCount(ddsd);
            uint pitch = DDSDPitch(ddsd);

            switch (rgbBitCount)
            {
                case 8:
                    emuState->SurfacePitch = pitch;
                    emuState->BitDepth = (byte)BitDepthStates.BIT_8;
                    break;

                case 15:
                case 16:
                    emuState->SurfacePitch = pitch / 2;
                    emuState->BitDepth = (byte)BitDepthStates.BIT_16;
                    break;

                case 24:
                    MessageBox.Show("24 Bit color is currently unsupported", "Ok");

                    Environment.Exit(0);

                    //emuState->SurfacePitch = pitch;
                    //emuState->BitDepth = (byte)BitDepthStates.BIT_24;
                    break;

                case 32:
                    emuState->SurfacePitch = pitch / 4;
                    emuState->BitDepth = (byte)BitDepthStates.BIT_32;
                    break;

                default:
                    MessageBox.Show("Unsupported Color Depth!", "Error");
                    return 1;
            }

            if (!DDSDHasSurface(ddsd)) {
                MessageBox.Show("Returning NULL!!", "ok");
            }

            SetSurfaces(ddsd);

            return 0;
        }

        public int UnlockSurface()
        {
            return Library.DirectDraw.UnlockSurface();
        }

        //--TODO: I don't know what HDC is.
        public unsafe void GetSurfaceDC(void** pHdc)
        {
            Library.DirectDraw.GetSurfaceDC(pHdc);
        }

        //--TODO: I don't know what HDC is.
        public unsafe void ReleaseSurfaceDC(void* hdc)
        {
            Library.DirectDraw.ReleaseSurfaceDC(hdc);
        }

        public int SurfaceFlip()
        {
            return Library.DirectDraw.SurfaceFlip();
        }

        public RECT GetWindowDefaultSize()
        {
            return Library.DirectDraw.GetWindowDefaultSize();
        }

        public unsafe int SurfaceBlt(RECT* rcDest, RECT* rcSrc)
        {
            return Library.DirectDraw.SurfaceBlt(rcDest, rcSrc);
        }

        public bool HasBackSurface()
        {
            return Library.DirectDraw.HasBackSurface() != Define.FALSE;
        }

        public unsafe DDSURFACEDESC* DDSDCreate()
        {
            return Library.DirectDraw.DDSDCreate();
        }

        public unsafe uint DDSDRGBBitCount(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDRGBBitCount(ddsd);
        }

        public unsafe uint DDSDPitch(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDPitch(ddsd);
        }

        public unsafe bool DDSDHasSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDHasSurface(ddsd) == Define.TRUE;
        }

        public void CheckSurfaces()
        {
            Library.DirectDraw.CheckSurfaces();;
        }

        public unsafe int LockSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.LockSurface(ddsd);
        }

        public unsafe void SetSurfaces(DDSURFACEDESC* ddsd)
        {
            Library.DirectDraw.SetSurfaces(ddsd);
        }

        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public unsafe bool CreateDirectDrawWindow(EmuState* emuState)
        {
            return Library.DirectDraw.CreateDirectDrawWindow(emuState) == Define.TRUE;
        }
    }
}
