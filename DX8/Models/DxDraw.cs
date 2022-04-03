using DX8.Internal;
using DX8.Internal.Interfaces;
using DX8.Internal.Libraries;
using DX8.Internal.Models;
using static System.IntPtr;
using Point = System.Drawing.Point;

namespace DX8.Models
{
    internal class DxDraw : IDxDraw
    {
        private readonly IDDraw _dDraw;
        private readonly IDxFactoryInternal _factory;

        private IDirectDraw? _dd;      // The DirectDraw object
        private IDirectDrawClipper? _clipper; // Clipper for primary surface
        private IDirectDrawSurface? _surface;    // Primary surface
        private IDirectDrawSurface? _back;    // Back surface

        private DDSURFACEDESC _primarySurface;
        private DDSURFACEDESC _backSurface;
        private DDSURFACEDESC _lockSurface;

        private Point _windowSize;

        public long SurfacePitch { get; private set; }
        public IntPtr Surface { get; private set; }

        internal DxDraw(IDDraw dDraw, IDxFactoryInternal factory)
        {
            _dDraw = dDraw;
            _factory = factory;
        }

        public DxDraw() : this(new DDraw(), new DxFactoryInternal()) { }

        public bool CreateDirectDraw(Point windowSize)
        {
            _windowSize = windowSize;

            _dd = _factory.CreateDirectDraw(_dDraw);

            return _dd != null;
        }

        public bool CreatePrimarySurface()
        {
            _primarySurface = CreatePrimarySurfaceDescription();
            _surface = _factory.CreateSurface(_dd, ref _primarySurface);

            return _surface != null;
        }

        public bool CreateBackSurface()
        {
            _backSurface = CreateBackSurfaceDescription(_windowSize);
            _back = _factory.CreateSurface(_dd, ref _backSurface);

            return _back != null;
        }

        public bool CreateClipper()
        {
            _clipper = _factory.CreateClipper(_dd); // Create the clipper using the DirectDraw object

            return _clipper != null;
        }

        public bool SetCooperativeLevel(IntPtr hWnd, uint value)
        {
            return _dd!.SetCooperativeLevel(hWnd, value) == DxDefine.S_OK;
        }

        public bool HasSurface()
        {
            return _surface != null;
        }

        public bool SurfaceIsLost()
        {
            return _surface!.IsLost() != 0;
        }

        public bool BackSurfaceIsLost()
        {
            return _back!.IsLost() != 0;
        }

        public void SurfaceRestore()
        {
            _surface!.Restore();
        }

        public void BackSurfaceRestore()
        {
            _back!.Restore();
        }

        public void SurfaceBlt(int dl, int dt, int dr, int db, int sl, int st, int sr, int sb)
        {
            var rcDest = new DXRECT { left = dl, top = dt, right = dr, bottom = db };
            var rcSrc = new DXRECT { left = sl, top = st, right = sr, bottom = sb };

            _surface!.Blt(ref rcDest, _back!, ref rcSrc, DxDefine.DDBLT_WAIT, Zero);
        }

        public bool HasBackSurface()
        {
            return _back != null;
        }

        public IntPtr GetBackSurface()
        {
            IntPtr p = Zero;

            _back!.GetDC(ref p);

            return p;
        }

        public void ReleaseBackSurface(IntPtr hdc)
        {
            _back!.ReleaseDC(hdc);
        }

        public void SurfaceFlip()
        {
            _surface!.Flip(Zero, DxDefine.DDFLIP_NOVSYNC | DxDefine.DDFLIP_DONOTWAIT);
        }

        public bool LockSurface()
        {
            long flags = DxDefine.DDLOCK_WAIT | DxDefine.DDLOCK_SURFACEMEMORYPTR;

            _lockSurface = CreateSurfaceDescription();

            var result = _back!.Lock(Zero, ref _lockSurface, (uint)flags, Zero) == DxDefine.S_OK;

            SurfacePitch = _lockSurface.lPitch >> 2;
            Surface = _lockSurface.lpSurface;

            return result;
        }

        public bool UnlockSurface()
        {
            return _back!.Unlock(Zero) == DxDefine.S_OK;
        }

        public bool SetSurfaceClipper()
        {
            return _surface!.SetClipper(_clipper!) == DxDefine.S_OK;
        }

        public bool SetClipper(IntPtr hWnd)
        {
            return _clipper!.SetHWnd(0, hWnd) == DxDefine.S_OK;
        }

        private static DDSURFACEDESC CreateSurfaceDescription()
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)DDSURFACEDESC.Size
            };
        }

        private static DDSURFACEDESC CreatePrimarySurfaceDescription()
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)DDSURFACEDESC.Size,
                dwFlags = DxDefine.DDSD_CAPS,
                ddsCaps = new DDSCAPS
                {
                    dwCaps = DxDefine.DDSCAPS_PRIMARYSURFACE
                }
            };
        }

        private static DDSURFACEDESC CreateBackSurfaceDescription(Point windowSize)
        {
            return new DDSURFACEDESC
            {
                dwSize = (uint)DDSURFACEDESC.Size,
                dwFlags = DxDefine.DDSD_WIDTH | DxDefine.DDSD_HEIGHT | DxDefine.DDSD_CAPS,
                dwWidth = (uint)windowSize.X,
                dwHeight = (uint)windowSize.Y,
                ddsCaps = new DDSCAPS
                {
                    dwCaps = DxDefine.DDSCAPS_VIDEOMEMORY // Try to create back buffer in video RAM
                }
            };
        }
    }
}
