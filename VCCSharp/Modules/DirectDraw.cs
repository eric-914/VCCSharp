using System;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;
using Point = System.Drawing.Point;
using static System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        void ClearScreen();
        bool CreateDirectDrawWindow();
        void DoCls();
        void FullScreenToggle();
        void SetAspect(bool forceAspect);
        void SetStatusBarText(string textBuffer);
        float Static();

        bool LockScreen();
        void UnlockScreen();

        bool InfoBand { get; set; }
    }

    public class DirectDraw : IDirectDraw
    {
        #region Properties

        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IGdi32 _gdi32;

        private static int _textX, _textY;
        private static byte _counter, _counter1 = 32, _phase = 1;

        private Point _windowSize;

        private bool _forceAspect;

        private uint _color;
        
        private string _statusText;

        public bool InfoBand { get; set; }

        #endregion

        private IntPtr _dd = Zero;      // The DirectDraw object
        private IntPtr _clipper = Zero; // Clipper for primary surface
        private IntPtr _surf = Zero;    // Primary surface
        private IntPtr _back = Zero;    // Back surface
        
        public DirectDraw(IModules modules, IUser32 user32, IGdi32 gdi32)
        {
            _modules = modules;
            _user32 = user32;
            _gdi32 = gdi32;
        }

        public void ClearScreen()
        {
            _color = 0;
        }

        public unsafe bool CreateDirectDrawWindow()
        {
            var pp = new Point(Define.DEFAULT_WIDTH, Define.DEFAULT_HEIGHT);
            
            if (_modules.Config.GetRememberSize())
            {
                pp = _modules.Config.GetIniWindowSize();
            }

            _windowSize.X = pp.X;
            _windowSize.Y = pp.Y;

            DDSURFACEDESC* ddsd = CreateSurface();

            //TODO: Add full screen mode back some day...

            if (!CreateDirectDrawWindowedMode(ddsd))
            {
                return false;
            }

            _modules.Emu.WindowSize = new System.Windows.Point(_windowSize.X, _windowSize.Y);

            return true;
        }

        public unsafe void DoCls()
        {
            if (LockScreen())
            {
                return;
            }

            uint* pSurface32 = _modules.Graphics.GetGraphicsSurface();

            for (int y = 0; y < _windowSize.Y; y++)
            {
                long yy = y * _modules.Emu.SurfacePitch;

                for (int x = 0; x < _windowSize.X; x++)
                {
                    pSurface32[x + yy] = _color;
                }
            }

            UnlockScreen();
        }

        public void FullScreenToggle()
        {
            _modules.Audio.PauseAudio(true);

            if (!CreateDirectDrawWindow())
            {
                MessageBox.Show("Can't rebuild primary Window", "Error");

                Environment.Exit(0);
            }

            _modules.Graphics.InvalidateBorder();

            _modules.Audio.PauseAudio(false);
        }

        public void SetAspect(bool forceAspect)
        {
            _forceAspect = forceAspect;
        }

        public void SetStatusBarText(string text)
        {
            _statusText = text;
        }

        public float Static()
        {
            unsafe
            {
                Static(_modules.Graphics.GetGraphicsSurface());
            }


            return _modules.Throttle.CalculateFps();
        }

        public unsafe bool LockScreen()
        {
            DDSURFACEDESC* ddsd = CreateSurface();  // A structure to describe the surfaces we want

            CheckSurfaces();

            // Lock entire surface, wait if it is busy, return surface memory pointer
            int hr = LockSurface(ddsd);

            if (hr < 0)
            {
                return true;
            }

            uint pitch = GetPitch(ddsd);

            _modules.Emu.SurfacePitch = pitch / 4;

            if (!HasSurface(ddsd))
            {
                MessageBox.Show("Returning NULL!!", "ok");
            }

            SetSurface(ddsd);

            return false;
        }

        public void UnlockScreen()
        {
            if (_modules.Emu.FullScreen && InfoBand)
            {
                WriteStatusText(_statusText);
            }

            UnlockBackSurface();

            DisplayFlip();
        }

        private unsafe void Static(uint* pSurface32)
        {
            var random = new Random();

            LockScreen();

            if (pSurface32 == null)
            {
                return; //TODO: Seems bad to exit w/out unlocking first
            }

            for (int y = 0; y < _windowSize.Y; y++)
            {
                long yy = y * _modules.Emu.SurfacePitch;

                for (int x = 0; x < _windowSize.X; x++)
                {
                    int temp = random.Next() & 255;

                    pSurface32[x + yy] = (uint)(temp | (temp << 8) | (temp << 16));
                }
            }

            byte color = (byte)(_counter1 << 2);

            ShowStaticMessage((ushort)_textX, (ushort)_textY, (uint)(color << 16 | color << 8 | color));

            _counter++;
            _counter1 += _phase;

            if (_counter1 == 60 || _counter1 == 20)
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

        private void ShowStaticMessage(ushort x, ushort y, uint color)
        {
            const string message = " Signal Missing! Press F9 ";

            IntPtr hdc;

            unsafe
            {
                GetBackSurface(&hdc);
            }

            _gdi32.SetBkColor(hdc, 0);
            _gdi32.SetTextColor(hdc, color);
            _gdi32.TextOut(hdc, x, y, message, message.Length);

            ReleaseBackSurface(hdc);
        }

        private void WriteStatusText(string message)
        {
            IntPtr hdc;

            unsafe
            {
                GetBackSurface(&hdc);
            }

            _gdi32.SetBkColor(hdc, 0); //RGB(0, 0, 0)
            _gdi32.SetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
            _gdi32.TextOut(hdc, 0, 0, message, message.Length);

            ReleaseBackSurface(hdc);
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

                if (_forceAspect) // Adjust the Aspect Ratio if window is resized
                {
                    float srcWidth = _windowSize.X;
                    float srcHeight = _windowSize.Y;
                    float srcRatio = srcWidth / srcHeight;

                    // change this to use the existing rcDest and the calc, w = right-left & h = bottom-top, 
                    //                         because rcDest has already been converted to screen cords, right?   
                    RECT rcClient;

                    _user32.GetClientRect(_modules.Emu.WindowHandle, &rcClient);  // x,y is always 0,0 so right, bottom is w,h

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

        private unsafe void SetSurface(DDSURFACEDESC* ddsd)
        {
            _modules.Graphics.SetGraphicsSurface(GetSurface(ddsd));
        }

        private unsafe bool CreateDirectDrawWindowedMode(DDSURFACEDESC* ddsd)
        {
            RECT size;

            _user32.GetWindowRect(_modules.Emu.WindowHandle, &size);	// And save the Final size of the Window 

            int width = size.right - size.left;
            int height = size.bottom - size.top;

            //TODO: Figure out how to replace magic # 175
            _user32.MoveWindow(_modules.Emu.WindowHandle, size.left, size.top, width, height + 175, 1);

            _user32.ShowWindow(_modules.Emu.WindowHandle, Define.SW_SHOWDEFAULT);
            _user32.UpdateWindow(_modules.Emu.WindowHandle);

            long hr = DirectDrawCreate();

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

        // Checks if the memory associated with surfaces is lost and restores if necessary.
        private void CheckSurfaces()
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

        private unsafe int LockSurface(DDSURFACEDESC* ddsd)
        {
            long flags = Define.DDLOCK_WAIT | Define.DDLOCK_SURFACEMEMORYPTR;

            return Library.DirectDraw.LockDDBackSurface(_back, ddsd, (uint)flags);
        }

        private bool HasSurface()
        {
            return _surf != Zero;
        }

        private bool SurfaceIsLost()
        {
            return Library.DirectDraw.DDSurfaceIsLost(_surf) != 0;
        }

        private bool BackSurfaceIsLost()
        {
            return Library.DirectDraw.DDBackSurfaceIsLost(_back) != 0;
        }

        private void SurfaceRestore()
        {
            Library.DirectDraw.DDSurfaceRestore(_surf);
        }

        private void BackSurfaceRestore()
        {
            Library.DirectDraw.DDBackSurfaceRestore(_back);
        }

        private static unsafe uint GetPitch(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetPitch(ddsd);
        }

        private static unsafe bool HasSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDHasSurface(ddsd) != 0;
        }

        private unsafe void SurfaceBlt(RECT* rcDest, RECT* rcSrc)
        {
            Library.DirectDraw.DDSurfaceBlt(_surf, _back, rcDest, rcSrc);
        }

        private bool HasBackSurface()
        {
            return _back != Zero;
        }

        private static unsafe DDSURFACEDESC* CreateSurface()
        {
            return Library.DirectDraw.DDSDCreate();
        }

        private unsafe void GetBackSurface(IntPtr* pHdc)
        {
            Library.DirectDraw.GetDDBackSurfaceDC(_back, pHdc);
        }

        private void ReleaseBackSurface(IntPtr hdc)
        {
            Library.DirectDraw.ReleaseDDBackSurfaceDC(_back, hdc);
        }

        private void SurfaceFlip()
        {
            Library.DirectDraw.DDSurfaceFlip(_surf);
        }

        private static unsafe void* GetSurface(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDSDGetSurface(ddsd);
        }

        private int SetSurfaceClipper()
        {
            return Library.DirectDraw.DDSurfaceSetClipper(_surf, _clipper);
        }

        private int SetClipper(HWND hWnd)
        {
            return Library.DirectDraw.DDClipperSetHWnd(_clipper, hWnd);
        }

        private unsafe int CreateClipper()
        {
            fixed (IntPtr* p = &_clipper)
            {
                return Library.DirectDraw.DDCreateClipper(_dd, p);
            }
        }

        private unsafe int GetDisplayMode(DDSURFACEDESC* ddsd)
        {
            return Library.DirectDraw.DDGetDisplayMode(_dd, ddsd);
        }

        private unsafe int CreateBackSurface(DDSURFACEDESC* ddsd)
        {
            fixed (IntPtr* p = &_back)
            {
                return Library.DirectDraw.DDCreateBackSurface(_dd, p, ddsd);
            }
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

        private unsafe int CreateSurface(DDSURFACEDESC* ddsd)
        {
            fixed (IntPtr* p = &_surf)
            {
                return Library.DirectDraw.DDCreateSurface(_dd, p, ddsd);
            }
        }

        private int SetCooperativeLevel(HWND hWnd, uint value)
        {
            return Library.DirectDraw.DDSetCooperativeLevel(_dd, hWnd, value);
        }

        private unsafe int DirectDrawCreate()
        {
            fixed (IntPtr* p = &_dd)
            {
                return Library.DirectDraw.DDCreate(p);
            }
        }

        private int UnlockBackSurface()
        {
            return Library.DirectDraw.UnlockDDBackSurface(_back);
        }
    }
}
