using System;
using System.Drawing;

namespace DX8
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
        IntPtr GetBackSurface();
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
}