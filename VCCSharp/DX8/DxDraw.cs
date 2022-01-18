using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using System;
using DX8;
using VCCSharp.Models;
using static System.IntPtr;
using Point = System.Drawing.Point;

namespace VCCSharp.DX8
{
    public interface IDxDraw
    {
        #region DirectDraw

        bool CreateDirectDraw(Point windowSize);
        bool SetCooperativeLevel(IntPtr hWnd, uint value);

        #endregion

        #region Surfaces

        bool CreatePrimarySurface();
        bool CreateBackSurface();
        bool HasSurface();
        bool SurfaceIsLost();
        bool BackSurfaceIsLost();
        void SurfaceRestore();
        void BackSurfaceRestore();
        void SurfaceBlt(int dl, int dt, int dr, int db, int sl, int st, int sr, int sb);
        bool HasBackSurface();
        unsafe void GetBackSurface(IntPtr* pHdc);
        void ReleaseBackSurface(IntPtr hdc);
        void SurfaceFlip();
        bool LockSurface();
        bool UnlockSurface();

        #endregion

        #region Clipper

        bool CreateClipper();
        bool SetSurfaceClipper();
        bool SetClipper(IntPtr hWnd);

        #endregion

        long SurfacePitch { get; }
        IntPtr Surface { get; }
    }

    public class DxDraw : IDxDraw
    {
        private readonly IDDraw _dDraw;
        private readonly IDxFactory _factory;

        private IDirectDraw _dd;      // The DirectDraw object
        private IDirectDrawClipper _clipper; // Clipper for primary surface
        private IDirectDrawSurface _surface;    // Primary surface
        private IDirectDrawSurface _back;    // Back surface

        private DDSURFACEDESC _primarySurface;
        private DDSURFACEDESC _backSurface;
        private DDSURFACEDESC _lockSurface;

        private Point _windowSize;

        public long SurfacePitch { get; private set; }
        public IntPtr Surface { get; private set; }

        public DxDraw(IDDraw dDraw, IDxFactory factory)
        {
            _dDraw = dDraw;
            _factory = factory;
        }

        public DxDraw() : this(new DDraw(), new DxFactory()) { }

        public bool CreateDirectDraw(Point windowSize)
        {
            _windowSize = windowSize;

            _dd = _factory.CreateDirectDraw(_dDraw);

            return _dd != null;
        }

        public unsafe bool CreatePrimarySurface()
        {
            _primarySurface = CreatePrimarySurfaceDescription();

            fixed (DDSURFACEDESC* p = &_primarySurface)
            {
                _surface = _factory.CreateSurface(_dd, p);
            }

            return _surface != null;
        }

        public unsafe bool CreateBackSurface()
        {
            _backSurface = CreateBackSurfaceDescription(_windowSize);

            fixed (DDSURFACEDESC* p = &_backSurface)
            {
                _back = _factory.CreateSurface(_dd, p);
            }

            return _back != null;
        }

        public bool CreateClipper()
        {
            _clipper = _factory.CreateClipper(_dd); // Create the clipper using the DirectDraw object

            return _clipper != null;
        }

        public bool SetCooperativeLevel(IntPtr hWnd, uint value)
        {
            return _dd.SetCooperativeLevel(hWnd, value) == Define.S_OK;
        }

        public bool HasSurface()
        {
            return _surface != null;
        }

        public bool SurfaceIsLost()
        {
            return _surface.IsLost() != 0;
        }

        public bool BackSurfaceIsLost()
        {
            return _back.IsLost() != 0;
        }

        public void SurfaceRestore()
        {
            _surface.Restore();
        }

        public void BackSurfaceRestore()
        {
            _back.Restore();
        }

        public unsafe void SurfaceBlt(int dl, int dt, int dr, int db, int sl, int st, int sr, int sb)
        {
            DXRECT rcDest = new DXRECT { left = dl, top = dt, right = dr, bottom = db};
            DXRECT rcSrc = new DXRECT { left = sl, top = st, right = sr, bottom = sb};

            _surface.Blt(&rcDest, _back, &rcSrc, Define.DDBLT_WAIT, Zero);
        }

        //public unsafe void SurfaceBlt(DXRECT * rcDest, DXRECT * rcSrc)
        //{
        //    _surface.Blt(rcDest, _back, rcSrc, Define.DDBLT_WAIT, Zero);
        //}

        public bool HasBackSurface()
        {
            return _back != null;
        }

        public unsafe void GetBackSurface(IntPtr* pHdc)
        {
            _back.GetDC(pHdc);
        }

        public void ReleaseBackSurface(IntPtr hdc)
        {
            _back.ReleaseDC(hdc);
        }

        public void SurfaceFlip()
        {
            _surface.Flip(Zero, Define.DDFLIP_NOVSYNC | Define.DDFLIP_DONOTWAIT);
        }

        public bool LockSurface()
        {
            long flags = Define.DDLOCK_WAIT | Define.DDLOCK_SURFACEMEMORYPTR;

            _lockSurface = CreateSurfaceDescription();

            unsafe
            {
                fixed (DDSURFACEDESC* p = &_lockSurface)
                {
                    var result = _back.Lock(Zero, p, (uint)flags, Zero) == Define.S_OK;

                    SurfacePitch = _lockSurface.lPitch >> 2;
                    Surface = _lockSurface.lpSurface;

                    return result;
                }
            }
        }

        public bool UnlockSurface()
        {
            return _back.Unlock(Zero) == Define.S_OK;
        }

        public bool SetSurfaceClipper()
        {
            return _surface.SetClipper(_clipper) == Define.S_OK;
        }

        public bool SetClipper(IntPtr hWnd)
        {
            return _clipper.SetHWnd(0, hWnd) == Define.S_OK;
        }

        public bool SetWindowSize(Point windowSize)
        {
            _windowSize = windowSize;

            return true;
        }

        private static DDSURFACEDESC CreateSurfaceDescription()
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)SizeOfSurfaceDescription()
            };
        }

        private static DDSURFACEDESC CreatePrimarySurfaceDescription()
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)SizeOfSurfaceDescription(),
                dwFlags = Define.DDSD_CAPS,
                ddsCaps = new DDSCAPS
                {
                    dwCaps = Define.DDSCAPS_PRIMARYSURFACE
                }
            };
        }

        private static DDSURFACEDESC CreateBackSurfaceDescription(Point windowSize)
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)SizeOfSurfaceDescription(),
                dwFlags = Define.DDSD_WIDTH | Define.DDSD_HEIGHT | Define.DDSD_CAPS,
                dwWidth = (uint)windowSize.X,
                dwHeight = (uint)windowSize.Y,
                ddsCaps = new DDSCAPS
                {
                    dwCaps = Define.DDSCAPS_VIDEOMEMORY // Try to create back buffer in video RAM
                }
            };
        }

        private static unsafe int SizeOfSurfaceDescription() => sizeof(DDSURFACEDESC);
    }
}
