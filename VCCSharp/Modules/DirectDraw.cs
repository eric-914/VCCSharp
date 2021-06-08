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
using HICON = System.IntPtr;
using HCURSOR = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        void InitDirectDraw();
        void ClearScreen();
        void FullScreenToggle();
        bool CreateDirectDrawWindow();
        void SetStatusBarText(string textBuffer);
        float Static();
        void DoCls();
        byte LockScreen();
        void UnlockScreen();
        void SetAspect(byte forceAspect);
        byte InfoBand { get; set; }
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate HINSTANCE WndProcTemplate(HWND hWnd, uint msg, long wParam, long lParam);

    public class DirectDraw : IDirectDraw
    {
        private static WndProcTemplate _wndProcTemplate;

        private readonly IModules _modules;
        private readonly IUser32 _user32;

        private static int _textX, _textY;
        private static byte _counter, _counter1 = 32, _phase = 1;

        private readonly HINSTANCE _hInstance = Zero;
        private Point _windowSize;

        private byte _forceAspect;

        private uint _color;

        public byte InfoBand { get; set; }

        public string AppNameText;
        public string TitleBarText;
        public string StatusText;

        public DirectDraw(IModules modules, IUser32 user32)
        {
            _modules = modules;
            _user32 = user32;
        }

        static DirectDraw()
        {
            _textX = 0;
        }

        public HINSTANCE WndProc(HWND hWnd, uint msg, long wParam, long lParam)
        {
            _modules.Events.ProcessMessage(hWnd, msg, wParam, lParam);

            return Library.DirectDraw.WndProc(hWnd, msg, wParam, lParam);
        }

        private bool CreateDirectDrawWindow(HINSTANCE resources, bool fullscreen)
        {
            uint style = Define.CS_HREDRAW | Define.CS_VREDRAW;

            IGDI gdi = _modules.GDI;

            HICON hIcon = gdi.GetIcon(resources);

            //--Convert WinProc to void*
            _wndProcTemplate = WndProc;

            unsafe
            {
                HCURSOR hCursor = gdi.GetCursor(fullscreen);
                void* hBrush = gdi.GetBrush();

                void* wndProc = (void*)Marshal.GetFunctionPointerForDelegate(_wndProcTemplate);

                //And Rebuilt it from scratch
                return Library.DirectDraw.DDRegisterClass(_hInstance, wndProc, AppNameText, null, style, hIcon, hCursor, hBrush) != Define.FALSE;
            }
        }

        public void FullScreenToggle()
        {
            _modules.Audio.PauseAudio(Define.TRUE);

            if (!CreateDirectDrawWindow())
            {
                MessageBox.Show("Can't rebuild primary Window", "Error");

                Environment.Exit(0);
            }

            _modules.Graphics.InvalidateBorder();

            _modules.Audio.PauseAudio(Define.FALSE);
        }

        public void SetAspect(byte forceAspect)
        {
            _forceAspect = forceAspect;
        }

        public unsafe void DoCls()
        {
            GraphicsSurfaces graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            if (LockScreen() == Define.TRUE)
            {
                return;
            }

            switch ((BitDepthStates)(_modules.Emu.BitDepth))
            {
                case BitDepthStates.BIT_8:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces.pSurface8[x + (y * _modules.Emu.SurfacePitch)] = (byte)(_color | 128);
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces.pSurface16[x + (y * _modules.Emu.SurfacePitch)] = (ushort)_color;
                        }
                    }
                    break;

                case BitDepthStates.BIT_24:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces.pSurface8[(x * 3) + (y * _modules.Emu.SurfacePitch)] = (byte)((_color & 0xFF0000) >> 16);
                            graphicsSurfaces.pSurface8[(x * 3) + 1 + (y * _modules.Emu.SurfacePitch)] = (byte)((_color & 0x00FF00) >> 8);
                            graphicsSurfaces.pSurface8[(x * 3) + 2 + (y * _modules.Emu.SurfacePitch)] = (byte)(_color & 0xFF);
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            graphicsSurfaces.pSurface32[x + (y * _modules.Emu.SurfacePitch)] = _color;
                        }
                    }
                    break;
            }

            UnlockScreen();
        }

        public void ClearScreen()
        {
            _color = 0;
        }

        public float Static()
        {
            Static(_modules.Graphics.GetGraphicsSurfaces());

            return _modules.Throttle.CalculateFPS();
        }

        private unsafe void Static(GraphicsSurfaces graphicsSurfaces)
        {
            var random = new Random();

            LockScreen();

            if (graphicsSurfaces.pSurface32 == null)
            {
                return; //TODO: Seems bad to exit w/out unlocking first
            }

            switch ((BitDepthStates)(_modules.Emu.BitDepth))
            {
                case BitDepthStates.BIT_8:
                    byte[] greyScales = { 128, 135, 184, 191 };

                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 160; x++)
                        {
                            byte temp = (byte)(random.Next() & 3);

                            graphicsSurfaces.pSurface32[x + (y * _modules.Emu.SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                            graphicsSurfaces.pSurface32[x + ((y + 1) * _modules.Emu.SurfacePitch >> 2)] = (uint)(greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24));
                        }
                    }
                    break;

                case BitDepthStates.BIT_16:
                    for (int y = 0; y < 480; y += 2)
                    {
                        for (int x = 0; x < 320; x++)
                        {
                            byte temp = (byte)(random.Next() & 31);

                            graphicsSurfaces.pSurface32[x + (y * _modules.Emu.SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                            graphicsSurfaces.pSurface32[x + ((y + 1) * _modules.Emu.SurfacePitch >> 1)] = (uint)(temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27));
                        }
                    }
                    break;

                case BitDepthStates.BIT_24: //--TODO: Don't think this was ever tested
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);
                            graphicsSurfaces.pSurface8[(x * 3) + (y * _modules.Emu.SurfacePitch)] = temp;
                            graphicsSurfaces.pSurface8[(x * 3) + 1 + (y * _modules.Emu.SurfacePitch)] = temp;
                            graphicsSurfaces.pSurface8[(x * 3) + 2 + (y * _modules.Emu.SurfacePitch)] = temp;
                        }
                    }
                    break;

                case BitDepthStates.BIT_32:
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            byte temp = (byte)(random.Next() & 255);

                            graphicsSurfaces.pSurface32[x + (y * _modules.Emu.SurfacePitch)] = (uint)(temp | (temp << 8) | (temp << 16));
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

            UnlockScreen();
        }

        public void UnlockScreen()
        {
            if (_modules.Emu.FullScreen && InfoBand == Define.TRUE)
            {
                WriteStatusText(StatusText);
            }

            UnlockBackSurface();

            DisplayFlip();
        }

        public void ShowStaticMessage(ushort x, ushort y, uint color)
        {
            unsafe
            {
                const string message = " Signal Missing! Press F9";
                void* hdc;

                GetBackSurface(&hdc);

                _modules.GDI.SetBkColor(hdc, 0);
                _modules.GDI.SetTextColor(hdc, color);
                _modules.GDI.WriteTextOut(hdc, x, y, message);

                ReleaseBackSurface(hdc);
            }
        }

        public void WriteStatusText(string statusText)
        {
            unsafe
            {
                void* hdc;

                var sb = new StringBuilder(statusText.PadRight(134 - statusText.Length));

                GetBackSurface(&hdc);

                _modules.GDI.SetBkColor(hdc, 0); //RGB(0, 0, 0)
                _modules.GDI.SetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
                _modules.GDI.TextOut(hdc, 0, 0, sb.ToString(), 132);

                ReleaseBackSurface(hdc);
            }
        }

        public void SetStatusBarText(string text)
        {
            StatusText = text;
        }

        private unsafe void DisplayFlip()
        {
            if (_modules.Emu.FullScreen)
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
                _user32.ClientToScreen(_modules.Emu.WindowHandle, &p);  // find out where on the primary surface our window lives

                // get the actual client rectangle, which is always 0,0 - w,h
                _user32.GetClientRect(_modules.Emu.WindowHandle, &rcDest);

                // The OffsetRect function moves the specified rectangle by the specified offsets
                // add the delta screen point we got above, which gives us the client rect in screen coordinates.
                _user32.OffsetRect(&rcDest, p.X, p.Y);

                // our destination rectangle is going to be 
                _user32.SetRect(&rcSrc, 0, 0, (short)_windowSize.X, (short)_windowSize.Y);

                //if (instance->Resizeable)
                //if (true) //--Currently, this is fixed at always resizable
                //{

                //rcDest.bottom -= (int)_statusBarHeight;

                if (_forceAspect == Define.TRUE) // Adjust the Aspect Ratio if window is resized
                {
                    float srcWidth = _windowSize.X;
                    float srcHeight = _windowSize.Y;
                    float srcRatio = srcWidth / srcHeight;

                    // change this to use the existing rcDest and the calc, w = right-left & h = bottom-top, 
                    //                         because rcDest has already been converted to screen cords, right?   
                    RECT rcClient;

                    _user32.GetClientRect(_modules.Emu.WindowHandle, &rcClient);  // x,y is always 0,0 so right, bottom is w,h

                    //rcClient.bottom -= (int)_statusBarHeight;

                    float clientWidth = rcClient.right;
                    float clientHeight = rcClient.bottom;
                    float clientRatio = clientWidth / clientHeight;

                    float dstWidth, dstHeight;

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

                    _user32.ClientToScreen(_modules.Emu.WindowHandle, &pDstLeftTop);

                    Point pDstRightBottom = new Point { X = (int)(dstX + dstWidth), Y = (int)(dstY + dstHeight) };

                    _user32.ClientToScreen(_modules.Emu.WindowHandle, &pDstRightBottom);

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

                if (!HasBackSurface())
                {
                    MessageBox.Show("Odd", "Error"); // yes, odd error indeed!! (??) especially since we go ahead and use it below!
                }

                SurfaceBlt(&rcDest, &rcSrc);
            }

            //--Store the updated WindowSizeX/Y for configuration, later.
            RECT windowSize;

            _user32.GetClientRect(_modules.Emu.WindowHandle, &windowSize);

            _modules.Emu.WindowSize = new System.Windows.Point(windowSize.right, windowSize.bottom);
        }

        public unsafe byte LockScreen()
        {
            DDSURFACEDESC* ddsd = CreateSurface();  // A structure to describe the surfaces we want

            CheckSurfaces();

            // Lock entire surface, wait if it is busy, return surface memory pointer
            int hr = LockSurface(ddsd);

            if (hr < 0)
            {
                return (1);
            }

            uint rgbBitCount = GetRgbBitCount(ddsd);
            uint pitch = GetPitch(ddsd);

            switch (rgbBitCount)
            {
                case 8:
                    _modules.Emu.SurfacePitch = pitch;
                    _modules.Emu.BitDepth = (byte)BitDepthStates.BIT_8;
                    break;

                case 15:
                case 16:
                    _modules.Emu.SurfacePitch = pitch / 2;
                    _modules.Emu.BitDepth = (byte)BitDepthStates.BIT_16;
                    break;

                case 24:
                    MessageBox.Show("24 Bit color is currently unsupported", "Ok");

                    Environment.Exit(0);

                    //_modules.Emu.SurfacePitch = pitch;
                    //_modules.Emu.BitDepth = (byte)BitDepthStates.BIT_24;
                    break;

                case 32:
                    _modules.Emu.SurfacePitch = pitch / 4;
                    _modules.Emu.BitDepth = (byte)BitDepthStates.BIT_32;
                    break;

                default:
                    MessageBox.Show("Unsupported Color Depth!", "Error");
                    return 1;
            }

            if (!HasSurface(ddsd))
            {
                MessageBox.Show("Returning NULL!!", "ok");
            }

            SetSurfaces(ddsd);

            return 0;
        }

        public unsafe bool CreateDirectDrawWindow()
        {
            if (_modules.Config.GetRememberSize())
            {
                Point pp = _modules.Config.GetIniWindowSize();

                _windowSize.X = pp.X;
                _windowSize.Y = pp.Y;
            }
            else
            {
                _windowSize.X = 640;
                _windowSize.Y = 480;
            }

            if (_modules.Emu.WindowHandle != Zero) //If its go a value it must be a mode switch
            {
                Release();

                _user32.DestroyWindow(_modules.Emu.WindowHandle);

                UnregisterClass();
            }

            if (!CreateDirectDrawWindow(_modules.Emu.Resources, _modules.Emu.FullScreen))
            {
                return false;
            }

            DDSURFACEDESC* ddsd = CreateSurface();

            switch (_modules.Emu.FullScreen)
            {
                case false: //Windowed Mode
                    if (!CreateDirectDrawWindowedMode(ddsd))
                    {
                        return false;
                    }
                    break;

                case true:	//Full Screen Mode
                    if (!CreateDirectDrawWindowFullScreen(ddsd))
                    {
                        return false;
                    }
                    break;
            }

            _modules.Emu.WindowSize = new System.Windows.Point(_windowSize.X, _windowSize.Y);

            return true;
        }

        private unsafe void SetSurfaces(DDSURFACEDESC* ddsd)
        {
            _modules.Graphics.SetGraphicsSurfaces(GetSurface(ddsd));

            //GraphicsSurfaces graphicsSurfaces = _modules.Graphics.GetGraphicsSurfaces();

            //graphicsSurfaces.pSurface = GetSurface(ddsd);

            //graphicsSurfaces.pSurface8 = (byte*)surface;
            //graphicsSurfaces.pSurface16 = (ushort*)surface;
            //graphicsSurfaces.pSurface32 = (uint*)surface;
        }

        public void InitDirectDraw()
        {
            TitleBarText = _modules.Config.AppTitle;
            AppNameText = _modules.Config.AppTitle;
        }

        private unsafe bool CreateDirectDrawWindowedMode(DDSURFACEDESC* ddsd)
        {
            RECT rc = new RECT { top = 0, left = 0, right = _windowSize.X, bottom = _windowSize.Y };

            // Calculates the required size of the window rectangle, based on the desired client-rectangle size
            // The window rectangle can then be passed to the CreateWindow function to create a window whose client area is the desired size.
            _user32.AdjustWindowRect(&rc, Define.WS_OVERLAPPEDWINDOW, Define.TRUE);

            int width = rc.right - rc.left;
            int height = rc.bottom - rc.top;

            // We create the Main window 
            CreateWindowExA(Define.CW_USEDEFAULT, 0, width, height, Define.WS_OVERLAPPEDWINDOW);

            if (_modules.Emu.WindowHandle == Zero)
            {
                // Can't create window
                return false;
            }

            var rStatBar = new RECT();

            width = rStatBar.right - rStatBar.left;
            height = rStatBar.bottom - rStatBar.top;

            // using MoveWindow to resize 
            _user32.MoveWindow(_modules.Emu.WindowHandle, rStatBar.left, rStatBar.top, width, height, 1);

            RECT size;

            //_user32.SendMessageA(_hWndStatusBar, Define.WM_SIZE, 0, 0); // Redraw Status bar in new position

            _user32.GetWindowRect(_modules.Emu.WindowHandle, &size);	// And save the Final size of the Window 

            _user32.ShowWindow(_modules.Emu.WindowHandle, Define.SW_SHOWDEFAULT);
            _user32.UpdateWindow(_modules.Emu.WindowHandle);

            long hr = Create();

            if (hr < 0) return false;

            // Initialize the DirectDraw object
            hr = SetCooperativeLevel(_modules.Emu.WindowHandle, Define.DDSCL_NORMAL); // Set to use windowed mode

            if (hr < 0) return false;

            SetSurfaceFlags(ddsd, Define.DDSD_CAPS);
            SetSurfaceCapabilities(ddsd, Define.DDSCAPS_PRIMARYSURFACE);

            // Create our Primary Surface
            hr = CreateSurface(ddsd);

            if (hr < 0) return false;

            SetSurfaceFlags(ddsd, Define.DDSD_WIDTH | Define.DDSD_HEIGHT | Define.DDSD_CAPS);

            // Make our off-screen surface 
            SetWidth(ddsd, (uint)_windowSize.X);
            SetHeight(ddsd, (uint)_windowSize.Y);

            SetSurfaceCapabilities(ddsd, Define.DDSCAPS_VIDEOMEMORY); // Try to create back buffer in video RAM
            hr = CreateBackSurface(ddsd);

            if (hr < 0)
            { // If not enough Video Ram 			
                SetSurfaceCapabilities(ddsd, Define.DDSCAPS_SYSTEMMEMORY);			// Try to create back buffer in System RAM
                hr = CreateBackSurface(ddsd);

                if (hr < 0)
                {
                    return false; //Giving Up
                }

                MessageBox.Show("Creating Back Buffer in System Ram!\nThis will be slower", "Performance Warning");
            }

            hr = GetDisplayMode(ddsd);

            if (hr < 0) return false;

            hr = CreateClipper(); // Create the clipper using the DirectDraw object

            if (hr < 0) return false;

            hr = SetClipper(_modules.Emu.WindowHandle);	// Assign your window's HWND to the clipper

            if (hr < 0) return false;

            hr = SetSurfaceClipper(); // Attach the clipper to the primary surface

            if (hr < 0) return false;

            hr = LockSurface(ddsd);

            if (hr < 0) return false;

            hr = UnlockBackSurface();

            return hr >= 0;
        }


        private unsafe bool CreateDirectDrawWindowFullScreen(DDSURFACEDESC* ddsd)
        {
            SetPitch(ddsd, 0);
            SetRgbBitCount(ddsd, 0);

            CreateWindowExA(_windowSize.X, _windowSize.Y, 0, 0, Define.WS_POPUP | Define.WS_VISIBLE);

            if (_modules.Emu.WindowHandle == Zero)
            {
                return false;
            }

            RECT size;

            _user32.GetWindowRect(_modules.Emu.WindowHandle, &size);

            _user32.ShowWindow(_modules.Emu.WindowHandle, Define.SW_SHOWMAXIMIZED);
            _user32.UpdateWindow(_modules.Emu.WindowHandle);

            long hr = Create();		// Initialize DirectDraw

            if (hr < 0) return false;

            hr = SetCooperativeLevel(_modules.Emu.WindowHandle, Define.DDSCL_EXCLUSIVE | Define.DDSCL_FULLSCREEN | Define.DDSCL_NOWINDOWCHANGES);

            if (hr < 0) return false;

            hr = SetDisplayMode((uint)_windowSize.X, (uint)_windowSize.Y, 32);	// Set 640x480x32 Bit full-screen mode

            if (hr < 0) return false;

            SetSurfaceFlags(ddsd, Define.DDSD_CAPS | Define.DDSD_BACKBUFFERCOUNT);
            SetSurfaceCapabilities(ddsd, Define.DDSCAPS_PRIMARYSURFACE | Define.DDSCAPS_COMPLEX | Define.DDSCAPS_FLIP);
            SetBackBufferCount(ddsd, 1);

            hr = CreateSurface(ddsd);

            if (hr < 0) return false;

            SetSurfaceCapabilities(ddsd, Define.DDSCAPS_BACKBUFFER);

            DDSCAPS ddsCaps = GetSurfaceCapabilities(ddsd);

            SurfaceGetAttachedSurface(&ddsCaps);

            hr = GetDisplayMode(ddsd);

            if (hr < 0) return false;

            CreateFullScreenPalette();

            return true;
        }

        private unsafe void CreateWindowExA(int x, int y, int width, int height, uint style)
        {
            _modules.Emu.WindowHandle = _user32.CreateWindowExA(0, AppNameText, TitleBarText,
                style, x, y, width, height,
                Zero, null, _hInstance, null);
        }

        // Checks if the memory associated with surfaces is lost and restores if necessary.
        private static void CheckSurfaces()
        {
            if (HasSurface())
            {	// Check the primary surface
                if (SurfaceIsLost())
                {
                    SurfaceRestore();
                }
            }

            if (HasBackSurface())
            {	// Check the back buffer
                if (BackSurfaceIsLost())
                {
                    BackSurfaceRestore();
                }
            }
        }

        private static unsafe int LockSurface(DDSURFACEDESC* ddsd)
        {
            long flags = Define.DDLOCK_WAIT | Define.DDLOCK_SURFACEMEMORYPTR;

            return Library.DirectDraw.LockDDBackSurface(ddsd, (uint)flags);
        }

        private static void CreateFullScreenPalette()
        {
            byte[] colorValues = { 0, 85, 170, 255 };

            PALETTEENTRY[] pal = new PALETTEENTRY[256];

            for (int i = 0; i <= 63; i++)
            {
                pal[i + 128].peBlue = colorValues[(i & 8) >> 2 | (i & 1)];
                pal[i + 128].peGreen = colorValues[(i & 16) >> 3 | (i & 2) >> 1];
                pal[i + 128].peRed = colorValues[(i & 32) >> 4 | (i & 4) >> 2];
                pal[i + 128].peFlags = Define.PC_RESERVED | Define.PC_NOCOLLAPSE;
            }

            uint caps = Define.DDPCAPS_8BIT | Define.DDPCAPS_ALLOW256;

            unsafe
            {
                fixed (PALETTEENTRY* p = pal)
                {
                    IDirectDrawPalette* ddPalette = CreatePalette(caps, p);

                    SurfaceSetPalette(ddPalette); // Set palette for Primary surface
                }
            }
        }

        private static bool HasSurface()
        {
            return Library.DirectDraw.HasDDSurface() == Define.TRUE;
        }

        private static bool SurfaceIsLost()
        {
            return Library.DirectDraw.DDSurfaceIsLost() == Define.TRUE;
        }

        private static bool BackSurfaceIsLost()
        {
            return Library.DirectDraw.DDBackSurfaceIsLost() == Define.TRUE;
        }

        private static void SurfaceRestore()
        {
            Library.DirectDraw.DDSurfaceRestore();
        }

        private static void BackSurfaceRestore()
        {
            Library.DirectDraw.DDBackSurfaceRestore();
        }

        private static unsafe IDirectDrawPalette* CreatePalette(uint caps, PALETTEENTRY* pal)
        {
            return Library.DirectDraw.DDCreatePalette(caps, pal);
        }

        private static unsafe void SurfaceSetPalette(IDirectDrawPalette* ddPalette)
        {
            Library.DirectDraw.DDSurfaceSetPalette(ddPalette);
        }

        private static unsafe void SurfaceGetAttachedSurface(DDSCAPS* ddsCaps)
        {
            Library.DirectDraw.DDSurfaceGetAttachedSurface(ddsCaps);
        }

        private static unsafe void SetBackBufferCount(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwBackBufferCount(ddsd, value);
        }

        private static unsafe DDSCAPS GetSurfaceCapabilities(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetddsCaps(ddsd);
        }

        private static int SetDisplayMode(uint x, uint y, uint depth)
        {
            return Library.DirectDraw.DDSetDisplayMode(x, y, depth);
        }

        private static unsafe uint GetRgbBitCount(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetRGBBitCount(ddsd);
        }

        private static unsafe void SetRgbBitCount(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetRGBBitCount(ddsd, value);
        }

        private static unsafe uint GetPitch(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetPitch(ddsd);
        }

        private static unsafe void SetPitch(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetPitch(ddsd, value);
        }

        private static unsafe bool HasSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDHasSurface(ddsd) == Define.TRUE;
        }

        private static unsafe void SurfaceBlt(RECT* rcDest, RECT* rcSrc)
        {
            Library.DirectDraw.DDSurfaceBlt(rcDest, rcSrc);
        }

        private static bool HasBackSurface()
        {
            return Library.DirectDraw.HasDDBackSurface() != Define.FALSE;
        }

        private static unsafe DDSURFACEDESC* CreateSurface()
        {
            return Library.DirectDraw.DDSDCreate();
        }

        //--TODO: I don't know what HDC is.
        private static unsafe void GetBackSurface(void** pHdc)
        {
            Library.DirectDraw.GetDDBackSurfaceDC(pHdc);
        }

        //--TODO: I don't know what HDC is.
        private static unsafe void ReleaseBackSurface(void* hdc)
        {
            Library.DirectDraw.ReleaseDDBackSurfaceDC(hdc);
        }

        private static void SurfaceFlip()
        {
            Library.DirectDraw.DDSurfaceFlip();
        }

        private static void UnregisterClass()
        {
            Library.DirectDraw.DDUnregisterClass();
        }

        private static unsafe void* GetSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetSurface(ddsd);
        }

        private static int SetSurfaceClipper()
        {
            return Library.DirectDraw.DDSurfaceSetClipper();
        }

        private static int SetClipper(HWND hWnd)
        {
            return Library.DirectDraw.DDClipperSetHWnd(hWnd);
        }

        private static int CreateClipper()
        {
            return Library.DirectDraw.DDCreateClipper();
        }

        private static unsafe int GetDisplayMode(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDGetDisplayMode(ddsd);
        }

        private static unsafe int CreateBackSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDCreateBackSurface(ddsd);
        }

        private static unsafe void SetSurfaceCapabilities(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwCaps(ddsd, value);
        }

        private static unsafe void SetWidth(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwWidth(ddsd, value);
        }

        private static unsafe void SetHeight(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwHeight(ddsd, value);
        }

        private static unsafe void SetSurfaceFlags(DDSURFACEDESC* ddsd, uint value)
        {
            Library.DirectDraw.DDSDSetdwFlags(ddsd, value);
        }

        private static unsafe int CreateSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDCreateSurface(ddsd);
        }

        private static int SetCooperativeLevel(HWND hWnd, uint value)
        {
            return Library.DirectDraw.DDSetCooperativeLevel(hWnd, value);
        }

        private static int Create()
        {
            return Library.DirectDraw.DDCreate();
        }

        private static int UnlockBackSurface()
        {
            return Library.DirectDraw.UnlockDDBackSurface();
        }

        private static void Release()
        {
            Library.DirectDraw.DDRelease();
        }
    }
}
