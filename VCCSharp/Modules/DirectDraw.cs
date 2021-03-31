using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;
using Point = System.Drawing.Point;
using HWND = System.IntPtr;

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
                            graphicsSurfaces->pSurface16[x + (y * emuState->SurfacePitch)] = (ushort)instance->Color;
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

            UnlockDDBackSurface();

            DisplayFlip(emuState);
        }

        public void ShowStaticMessage(ushort x, ushort y, uint color)
        {
            unsafe
            {
                const string message = " Signal Missing! Press F9";
                void* hdc;

                GetDDBackSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0);
                _modules.GDI.GDISetTextColor(hdc, color);
                _modules.GDI.GDIWriteTextOut(hdc, x, y, message);

                ReleaseDDBackSurfaceDC(hdc);
            }
        }

        public void WriteStatusText(string statusText)
        {
            unsafe
            {
                void* hdc;

                var sb = new StringBuilder(statusText.PadRight(134 - statusText.Length));

                GetDDBackSurfaceDC(&hdc);

                _modules.GDI.GDISetBkColor(hdc, 0); //RGB(0, 0, 0)
                _modules.GDI.GDISetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
                _modules.GDI.GDITextOut(hdc, 0, 0, sb.ToString(), 132);

                ReleaseDDBackSurfaceDC(hdc);
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
            unsafe
            {
                DirectDrawState* instance = GetDirectDrawState();

                byte[] buffer = Converter.ToByteArray(text);

                fixed (byte* p = buffer)
                {
                    _user32.SendMessageA(instance->hWndStatusBar, Define.WM_SETTEXT, (ulong)text.Length, (long)p);
                };

                _user32.SendMessageA(instance->hWndStatusBar, Define.WM_SIZE, 0, 0);
            }
        }

        public unsafe void DisplayFlip(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (emuState->FullScreen == Define.TRUE)
            {	// if we're windowed do the blit, else just Flip
                DDSurfaceFlip();
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

                rcDest.bottom -= (int)instance->StatusBarHeight;

                if (instance->ForceAspect == Define.TRUE) // Adjust the Aspect Ratio if window is resized
                {
                    float srcWidth = (float)instance->WindowSize.X;
                    float srcHeight = (float)instance->WindowSize.Y;
                    float srcRatio = srcWidth / srcHeight;

                    // change this to use the existing rcDest and the calc, w = right-left & h = bottom-top, 
                    //                         because rcDest has already been converted to screen cords, right?   
                    RECT rcClient;

                    _user32.GetClientRect(emuState->WindowHandle, &rcClient);  // x,y is always 0,0 so right, bottom is w,h

                    rcClient.bottom -= (int)instance->StatusBarHeight;

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

                    //RECT defaultRect = _defaultSize;

                //    _user32.GetWindowRect(emuState->WindowHandle, &rect);
                //    _user32.MoveWindow(emuState->WindowHandle, rect.left, rect.top, defaultRect.right - defaultRect.left, defaultRect.bottom - defaultRect.top, 1);
                //}

                if (!HasDDBackSurface())
                {
                    MessageBox.Show("Odd", "Error"); // yes, odd error indeed!! (??) especially since we go ahead and use it below!
                }

                DDSurfaceBlt(&rcDest, &rcSrc);
            }

            //--Store the updated WindowSizeX/Y for configuration, later.
            RECT windowSize;

            _user32.GetClientRect(emuState->WindowHandle, &windowSize);

            emuState->WindowSize.X = windowSize.right;
            emuState->WindowSize.Y = (int)(windowSize.bottom - instance->StatusBarHeight);
        }

        public unsafe byte LockScreen(EmuState* emuState)
        {
            DDSURFACEDESC* ddsd = DDSDCreate();  // A structure to describe the surfaces we want

            CheckSurfaces();

            // Lock entire surface, wait if it is busy, return surface memory pointer
            int hr = LockSurface(ddsd);

            if (hr < 0)
            {
                return (1);
            }

            uint rgbBitCount = DDSDGetRGBBitCount(ddsd);
            uint pitch = DDSDGetPitch(ddsd);

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

            if (!DDSDHasSurface(ddsd))
            {
                MessageBox.Show("Returning NULL!!", "ok");
            }

            SetSurfaces(ddsd);

            return 0;
        }

        public unsafe bool CreateDirectDrawWindow(EmuState* emuState)
        {
            DirectDrawState* instance = GetDirectDrawState();

            if (_modules.Config.GetRememberSize())
            {
                Point pp = _modules.Config.GetIniWindowSize();

                instance->WindowSize.X = pp.X;
                instance->WindowSize.Y = pp.Y;
            }
            else
            {
                instance->WindowSize.X = 640;
                instance->WindowSize.Y = 480;
            }

            if (emuState->WindowHandle != null) //If its go a value it must be a mode switch
            {
                DDRelease();

                _user32.DestroyWindow(emuState->WindowHandle);

                DDUnregisterClass();
            }

            if (!CreateDirectDrawWindow(emuState->Resources, emuState->FullScreen))
            {
                return false;
            }

            DDSURFACEDESC* ddsd = DDSDCreate();

            switch (emuState->FullScreen)
            {
                case 0: //Windowed Mode
                    if (!CreateDirectDrawWindowedMode(emuState, ddsd))
                    {
                        return false;
                    }
                    break;

                case 1:	//Full Screen Mode
                    if (!CreateDirectDrawWindowFullScreen(emuState, ddsd))
                    {
                        return false;
                    }
                    break;
            }

            emuState->WindowSize.X = instance->WindowSize.X;
            emuState->WindowSize.Y = instance->WindowSize.Y;

            return true;
        }

        public unsafe void SetSurfaces(DDSURFACEDESC* ddsd)
        {
            GraphicsSurfaces* graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            void* surface = DDSDGetSurface(ddsd);

            graphicsSurfaces->pSurface8 = (byte*)surface;
            graphicsSurfaces->pSurface16 = (ushort*)surface;
            graphicsSurfaces->pSurface32 = (uint*)surface;
        }

        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE hResources)
        {
            unsafe
            {
                DirectDrawState* instance = GetDirectDrawState();

                instance->hInstance = hInstance;

                Converter.ToByteArray(_modules.Resource.ResourceAppTitle(hResources), instance->TitleBarText);
                Converter.ToByteArray(_modules.Resource.ResourceAppTitle(hResources), instance->AppNameText);

                return true;
            }
        }

        public unsafe bool CreateDirectDrawWindowedMode(EmuState* emuState, DDSURFACEDESC* ddsd)
        {
            DirectDrawState* instance = GetDirectDrawState();

            long hr;

            RECT rc = new RECT { top = 0, left = 0, right = instance->WindowSize.X, bottom = instance->WindowSize.Y };

            // Calculates the required size of the window rectangle, based on the desired client-rectangle size
            // The window rectangle can then be passed to the CreateWindow function to create a window whose client area is the desired size.
            _user32.AdjustWindowRect(&rc, Define.WS_OVERLAPPEDWINDOW, Define.TRUE);

            int width = rc.right - rc.left;
            int height = rc.bottom - rc.top;

            // We create the Main window 
            emuState->WindowHandle = _user32.CreateWindowExA(0, instance->AppNameText, instance->TitleBarText,
                Define.WS_OVERLAPPEDWINDOW, Define.CW_USEDEFAULT, 0, width, height,
                Zero, null, instance->hInstance, null);

            if (emuState->WindowHandle == null)
            {	// Can't create window
                return false;
            }

            instance->hWndStatusBar = _user32.CreateWindowExA(0, Define.STATUSCLASSNAME, "Ready",
                Define.SBARS_SIZEGRIP | Define.WS_CHILD | Define.WS_VISIBLE, 0, 0, 0, 0,
                emuState->WindowHandle, null, instance->hInstance, null);

            if (instance->hWndStatusBar == null)
            {	// Can't create Status bar
                return false;
            }

            var rStatBar = new RECT();

            // Retrieves the dimensions of the bounding rectangle of the specified window
            // The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
            _user32.GetWindowRect(instance->hWndStatusBar, &rStatBar); // Get the size of the Status bar

            instance->StatusBarHeight = (uint)(rStatBar.bottom - rStatBar.top); // Calculate its height

            _user32.GetWindowRect(emuState->WindowHandle, &rStatBar);

            width = rStatBar.right - rStatBar.left;
            height = rStatBar.bottom + (int)(instance->StatusBarHeight) - rStatBar.top;

            // using MoveWindow to resize 
            _user32.MoveWindow(emuState->WindowHandle, rStatBar.left, rStatBar.top, width, height, 1);

            RECT size;

            _user32.SendMessageA(instance->hWndStatusBar, Define.WM_SIZE, 0, 0); // Redraw Status bar in new position

            _user32.GetWindowRect(emuState->WindowHandle, &size);	// And save the Final size of the Window 

            _user32.ShowWindow(emuState->WindowHandle, Define.SW_SHOWDEFAULT);
            _user32.UpdateWindow(emuState->WindowHandle);

            hr = DDCreate();

            if (hr < 0) return false;

            // Initialize the DirectDraw object
            hr = DDSetCooperativeLevel(emuState->WindowHandle, Define.DDSCL_NORMAL); // Set DDSCL_NORMAL to use windowed mode

            if (hr < 0) return false;

            DDSDSetdwFlags(ddsd, Define.DDSD_CAPS);
            DDSDSetdwCaps(ddsd, Define.DDSCAPS_PRIMARYSURFACE);

            // Create our Primary Surface
            hr = DDCreateSurface(ddsd);

            if (hr < 0) return false;

            DDSDSetdwFlags(ddsd, Define.DDSD_WIDTH | Define.DDSD_HEIGHT | Define.DDSD_CAPS);

            // Make our off-screen surface 
            DDSDSetdwWidth(ddsd, (uint)instance->WindowSize.X);
            DDSDSetdwHeight(ddsd, (uint)instance->WindowSize.Y);

            DDSDSetdwCaps(ddsd, Define.DDSCAPS_VIDEOMEMORY); // Try to create back buffer in video RAM
            hr = DDCreateBackSurface(ddsd);

            if (hr < 0)
            { // If not enough Video Ram 			
                DDSDSetdwCaps(ddsd, Define.DDSCAPS_SYSTEMMEMORY);			// Try to create back buffer in System RAM
                hr = DDCreateBackSurface(ddsd);

                if (hr < 0)
                {
                    return false; //Giving Up
                }

                MessageBox.Show("Creating Back Buffer in System Ram!\nThis will be slower", "Performance Warning");
            }

            hr = DDGetDisplayMode(ddsd);

            if (hr < 0) return false;

            hr = DDCreateClipper(); // Create the clipper using the DirectDraw object

            if (hr < 0) return false;

            hr = DDClipperSetHWnd(emuState->WindowHandle);	// Assign your window's HWND to the clipper

            if (hr < 0) return false;

            hr = DDSurfaceSetClipper(); // Attach the clipper to the primary surface

            if (hr < 0) return false;

            hr = LockSurface(ddsd);

            if (hr < 0) return false;

            hr = UnlockDDBackSurface();

            return hr >= 0;
        }

        public unsafe bool CreateDirectDrawWindowFullScreen(EmuState* emuState, DDSURFACEDESC* ddsd)
        {
            DirectDrawState* instance = GetDirectDrawState();

            DDSDSetPitch(ddsd, 0);
            DDSDSetRGBBitCount(ddsd, 0);

            emuState->WindowHandle = _user32.CreateWindowExA(0, instance->AppNameText, null, Define.WS_POPUP | Define.WS_VISIBLE,
                0, 0, instance->WindowSize.X, instance->WindowSize.Y, Zero, null, instance->hInstance, null);

            if (emuState->WindowHandle == null) return false;

            RECT size;

            _user32.GetWindowRect(emuState->WindowHandle, &size);

            _user32.ShowWindow(emuState->WindowHandle, Define.SW_SHOWMAXIMIZED);
            _user32.UpdateWindow(emuState->WindowHandle);

            long hr = DDCreate();		// Initialize DirectDraw

            if (hr < 0) return false;

            hr = DDSetCooperativeLevel(emuState->WindowHandle, Define.DDSCL_EXCLUSIVE | Define.DDSCL_FULLSCREEN | Define.DDSCL_NOWINDOWCHANGES);

            if (hr < 0) return false;

            hr = DDSetDisplayMode((uint)instance->WindowSize.X, (uint)instance->WindowSize.Y, 32);	// Set 640x480x32 Bit full-screen mode

            if (hr < 0) return false;

            DDSDSetdwFlags(ddsd, Define.DDSD_CAPS | Define.DDSD_BACKBUFFERCOUNT);
            DDSDSetdwCaps(ddsd, Define.DDSCAPS_PRIMARYSURFACE | Define.DDSCAPS_COMPLEX | Define.DDSCAPS_FLIP);
            DDSDSetdwBackBufferCount(ddsd, 1);

            hr = DDCreateSurface(ddsd);

            if (hr < 0) return false;

            DDSDSetdwCaps(ddsd, Define.DDSCAPS_BACKBUFFER);

            DDSCAPS ddsCaps = DDSDGetddsCaps(ddsd);

            DDSurfaceGetAttachedSurface(&ddsCaps);

            hr = DDGetDisplayMode(ddsd);

            if (hr < 0) return false;

            CreateFullScreenPalette();

            return true;
        }

        

        private bool CreateDirectDrawWindow(HINSTANCE resources, byte fullscreen)
        {
            //IntPtr WndProcCallback(HWND hWnd, uint msg, ulong wParam, long lParam)
            //{
            //    _modules.Events.ProcessMessage(hWnd, msg, wParam, lParam);

            //    return _user32.DefWindowProcA(hWnd, msg, wParam, lParam);
            //}

            //return Library.DirectDraw.CreateDirectDrawWindow(resources, fullscreen, WndProcCallback) != Define.FALSE;
            return Library.DirectDraw.CreateDirectDrawWindow(resources, fullscreen) != Define.FALSE;
        }

        public void DDRelease()
        {
            Library.DirectDraw.DDRelease();
        }

        public void DDUnregisterClass()
        {
            Library.DirectDraw.DDUnregisterClass();
        }

        public unsafe void* DDSDGetSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetSurface(ddsd);
        }

        public int DDSurfaceSetClipper()
        {
            return Library.DirectDraw.DDSurfaceSetClipper();
        }

        public int DDClipperSetHWnd(HWND hWnd)
        {
            return Library.DirectDraw.DDClipperSetHWnd(hWnd);
        }

        public int DDCreateClipper()
        {
            return Library.DirectDraw.DDCreateClipper();
        }

        public unsafe int DDGetDisplayMode(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDGetDisplayMode(ddsd);
        }

        public unsafe int DDCreateBackSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDCreateBackSurface(ddsd);
        }

        public unsafe void DDSDSetdwCaps(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwCaps(ddsd, value);
        }

        public unsafe void DDSDSetdwWidth(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwWidth(ddsd, value);
        }

        public unsafe void DDSDSetdwHeight(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwHeight(ddsd, value);
        }

        public unsafe void DDSDSetdwFlags(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwFlags(ddsd, value);
        }

        public unsafe int DDCreateSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDCreateSurface(ddsd);
        }

        public int DDSetCooperativeLevel(HWND hWnd, uint value)
        {
            return Library.DirectDraw.DDSetCooperativeLevel(hWnd, value);
        }

        public int DDCreate()
        {
            return Library.DirectDraw.DDCreate();
        }

        public int UnlockDDBackSurface()
        {
            return Library.DirectDraw.UnlockDDBackSurface();
        }

        //--TODO: I don't know what HDC is.
        public unsafe void GetDDBackSurfaceDC(void** pHdc)
        {
            Library.DirectDraw.GetDDBackSurfaceDC(pHdc);
        }

        //--TODO: I don't know what HDC is.
        public unsafe void ReleaseDDBackSurfaceDC(void* hdc)
        {
            Library.DirectDraw.ReleaseDDBackSurfaceDC(hdc);
        }

        public int DDSurfaceFlip()
        {
            return Library.DirectDraw.DDSurfaceFlip();
        }

        public unsafe int DDSurfaceBlt(RECT* rcDest, RECT* rcSrc)
        {
            return Library.DirectDraw.DDSurfaceBlt(rcDest, rcSrc);
        }

        public bool HasDDBackSurface()
        {
            return Library.DirectDraw.HasDDBackSurface() != Define.FALSE;
        }

        public unsafe DDSURFACEDESC* DDSDCreate()
        {
            return Library.DirectDraw.DDSDCreate();
        }

        public unsafe uint DDSDGetRGBBitCount(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetRGBBitCount(ddsd);
        }

        public unsafe void DDSDSetRGBBitCount(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetRGBBitCount(ddsd, value);
        }

        public unsafe uint DDSDGetPitch(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetPitch(ddsd);
        }

        public unsafe void DDSDSetPitch(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetPitch(ddsd, value);
        }

        public unsafe bool DDSDHasSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDHasSurface(ddsd) == Define.TRUE;
        }

        // Checks if the memory associated with surfaces is lost and restores if necessary.
        public void CheckSurfaces()
        {
            if (HasDDSurface())
            {	// Check the primary surface
                if (DDSurfaceIsLost())
                {
                    DDSurfaceRestore();
                }
            }

            if (HasDDBackSurface())
            {	// Check the back buffer
                if (DDBackSurfaceIsLost())
                {
                    DDBackSurfaceRestore();
                }
            }
        }

        public unsafe int LockSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.LockDDBackSurface(ddsd);
        }

        public int DDSetDisplayMode(uint x, uint y, uint depth)
        {
            return Library.DirectDraw.DDSetDisplayMode(x, y, depth);
        }

        public unsafe void DDSDSetdwBackBufferCount(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwBackBufferCount(ddsd, value);
        }

        public unsafe DDSCAPS DDSDGetddsCaps(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetddsCaps(ddsd);
        }

        public unsafe void DDSurfaceGetAttachedSurface(DDSCAPS* ddsCaps)
        {
            Library.DirectDraw.DDSurfaceGetAttachedSurface(ddsCaps);
        }

        public void CreateFullScreenPalette()
        {
            byte[] ColorValues = { 0, 85, 170, 255 };

            PALETTEENTRY[] pal = new PALETTEENTRY[256];

            for (int i = 0; i <= 63; i++)
            {
                pal[i + 128].peBlue = ColorValues[(i & 8) >> 2 | (i & 1)];
                pal[i + 128].peGreen = ColorValues[(i & 16) >> 3 | (i & 2) >> 1];
                pal[i + 128].peRed = ColorValues[(i & 32) >> 4 | (i & 4) >> 2];
                pal[i + 128].peFlags = Define.PC_RESERVED | Define.PC_NOCOLLAPSE;
            }
            
            uint caps = Define.DDPCAPS_8BIT | Define.DDPCAPS_ALLOW256;

            unsafe
            {
                fixed (PALETTEENTRY* p = pal)
                {
                    IDirectDrawPalette* ddPalette = DDCreatePalette(caps, p);
                    
                    DDSurfaceSetPalette(ddPalette); // Set palette for Primary surface
                }
            }
        }

        public bool HasDDSurface()
        {
            return Library.DirectDraw.HasDDSurface() == Define.TRUE;
        }

        public bool DDSurfaceIsLost()
        {
            return Library.DirectDraw.DDSurfaceIsLost() == Define.TRUE;
        }

        public bool DDBackSurfaceIsLost()
        {
            return Library.DirectDraw.DDBackSurfaceIsLost() == Define.TRUE;
        }

        public void DDSurfaceRestore()
        {
            Library.DirectDraw.DDSurfaceRestore();
        }

        public void DDBackSurfaceRestore()
        {
            Library.DirectDraw.DDBackSurfaceRestore();
        }

        public unsafe IDirectDrawPalette* DDCreatePalette(uint caps, PALETTEENTRY* pal)
        {
            return Library.DirectDraw.DDCreatePalette(caps, pal);
        }

        public unsafe void DDSurfaceSetPalette(IDirectDrawPalette* ddPalette)
        {
            Library.DirectDraw.DDSurfaceSetPalette(ddPalette);
        }
    }
}
