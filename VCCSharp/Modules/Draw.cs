using System;
using System.Windows;
using VCCSharp.DX8;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Libraries.Models;
using VCCSharp.Models;
using static System.IntPtr;
using Point = System.Drawing.Point;

namespace VCCSharp.Modules
{
    public interface IDraw
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

    public class Draw : IDraw
    {
        #region Properties

        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IGdi32 _gdi32;

        private readonly IDxDraw _draw;

        private static int _textX, _textY;
        private static byte _counter, _counter1 = 32, _phase = 1;

        private Point _windowSize;

        private bool _forceAspect;

        private uint _color;

        private string _statusText;

        public bool InfoBand { get; set; }

        #endregion

        public Draw(IModules modules, IUser32 user32, IGdi32 gdi32, IDxDraw draw)
        {
            _modules = modules;
            _user32 = user32;
            _gdi32 = gdi32;
            _draw = draw;
        }

        public void ClearScreen()
        {
            _color = 0;
        }

        public bool CreateDirectDrawWindow()
        {
            var pp = new Point(Define.DEFAULT_WIDTH, Define.DEFAULT_HEIGHT);

            if (_modules.Config.GetRememberSize())
            {
                pp = _modules.Config.GetIniWindowSize();
            }

            _windowSize.X = pp.X;
            _windowSize.Y = pp.Y;

            //TODO: Add full screen mode back some day...

            if (!CreateDirectDrawWindowedMode())
            {
                return false;
            }

            _modules.Emu.WindowSize = new System.Windows.Point(_windowSize.X, _windowSize.Y);

            return true;
        }

        public void DoCls()
        {
            if (!LockScreen())
            {
                return;
            }

            unsafe
            {
                uint* pSurface32 = _modules.Graphics.GetGraphicsSurface();

                for (int y = 0; y < _windowSize.Y; y++)
                {
                    long yy = y * _modules.Emu.SurfacePitch;

                    for (int x = 0; x < _windowSize.X; x++)
                    {
                        pSurface32[x + yy] = _color;
                    }
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

        public bool LockScreen()
        {
            CheckSurfaces();

            // Lock entire surface, wait if it is busy, return surface memory pointer
            if (!_draw.LockSurface()) return false;

            _modules.Emu.SurfacePitch = _draw.SurfacePitch;

            if (_draw.Surface == Zero)
            {
                MessageBox.Show("Failed to return surface", "LockScreen");
            }

            _modules.Graphics.SetGraphicsSurface(_draw.Surface);

            return true;
        }

        public void UnlockScreen()
        {
            if (_modules.Emu.FullScreen && InfoBand)
            {
                WriteStatusText(_statusText);
            }

            _draw.UnlockSurface();

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
                _draw.GetBackSurface(&hdc);
            }

            _gdi32.SetBkColor(hdc, 0);
            _gdi32.SetTextColor(hdc, color);
            _gdi32.TextOut(hdc, x, y, message, message.Length);

            _draw.ReleaseBackSurface(hdc);
        }

        private void WriteStatusText(string message)
        {
            IntPtr hdc;

            unsafe
            {
                _draw.GetBackSurface(&hdc);
            }

            _gdi32.SetBkColor(hdc, 0); //RGB(0, 0, 0)
            _gdi32.SetTextColor(hdc, 0xFFFFFF); //RGB(255, 255, 255)
            _gdi32.TextOut(hdc, 0, 0, message, message.Length);

            _draw.ReleaseBackSurface(hdc);
        }

        private unsafe void DisplayFlip()
        {
            if (_modules.Emu.FullScreen)
            {	// if we're windowed do the blit, else just Flip
                _draw.SurfaceFlip();
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

                if (!_draw.HasBackSurface())
                {
                    MessageBox.Show("Odd", "Error"); // yes, odd error indeed!! (??) especially since we go ahead and use it below!
                }

                _draw.SurfaceBlt(rcDest.left, rcDest.top, rcDest.right, rcDest.bottom, rcSrc.left, rcSrc.top, rcSrc.right, rcSrc.bottom);
            }

            //--Store the updated WindowSizeX/Y for configuration, later.
            RECT windowSize;

            _user32.GetClientRect(_modules.Emu.WindowHandle, &windowSize);

            _modules.Emu.WindowSize = new System.Windows.Point(windowSize.right, windowSize.bottom);
        }

        private bool CreateDirectDrawWindowedMode()
        {
            RECT size;

            unsafe
            {
                _user32.GetWindowRect(_modules.Emu.WindowHandle, &size); // And save the Final size of the Window 
            }

            int width = size.right - size.left;
            int height = size.bottom - size.top;

            //TODO: Figure out how to replace magic # 175
            _user32.MoveWindow(_modules.Emu.WindowHandle, size.left, size.top, width, height + 175, 1);

            _user32.ShowWindow(_modules.Emu.WindowHandle, Define.SW_SHOWDEFAULT);
            _user32.UpdateWindow(_modules.Emu.WindowHandle);

            return _draw.CreateDirectDraw(_windowSize) // Initialize the DirectDraw object
                   && _draw.SetCooperativeLevel(_modules.Emu.WindowHandle, Define.DDSCL_NORMAL) // Set to use windowed mode
                   && _draw.CreatePrimarySurface()  // Create our primary surface
                   && _draw.CreateBackSurface() // Create our back surface
                   && _draw.CreateClipper()
                   && _draw.SetClipper(_modules.Emu.WindowHandle)   // Assign your window's HWND to the clipper
                   && _draw.SetSurfaceClipper() // Attach the clipper to the primary surface
                   && _draw.LockSurface() // Make sure lock works
                   && _draw.UnlockSurface() // And unlock
                ;
        }

        // Checks if the memory associated with surfaces is lost and restores if necessary.
        private void CheckSurfaces()
        {
            if (_draw.HasSurface())
            {	// Check the primary surface
                if (_draw.SurfaceIsLost())
                {
                    _draw.SurfaceRestore();
                }
            }

            if (_draw.HasBackSurface())
            {	// Check the back buffer
                if (_draw.BackSurfaceIsLost())
                {
                    _draw.BackSurfaceRestore();
                }
            }
        }
    }
}
