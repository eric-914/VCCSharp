using System;
using System.Collections.Generic;
using System.Drawing;
using DX8;
using VCCSharp.Models;

namespace VCCSharp.DX8
{
    /// <summary>
    /// Final wrapper of the DX8 objects, with no unsafe in interface
    /// </summary>
    public class Dx : IDxDraw, IDxSound, IDxInput
    {
        private readonly IDxDraw _draw;
        private readonly IDxSound _sound;
        private readonly IDxInput _input;

        private Dx(IDxDraw draw, IDxSound sound, IDxInput input)
        {
            _draw = draw;
            _sound = sound;
            _input = input;
        }

        public Dx() : this(new DxDraw(), new DxSound(), new DxInput()) { }

        #region DxDraw

        public bool CreateDirectDraw(Point windowSize) => _draw.CreateDirectDraw(windowSize);
        public bool SetCooperativeLevel(IntPtr hWnd, uint value) => _draw.SetCooperativeLevel(hWnd, value);
        public bool CreatePrimarySurface() => _draw.CreatePrimarySurface();
        public bool CreateBackSurface() => _draw.CreateBackSurface();
        public bool HasSurface() => _draw.HasSurface();
        public bool SurfaceIsLost() => _draw.SurfaceIsLost();
        public bool BackSurfaceIsLost() => _draw.BackSurfaceIsLost();
        public void SurfaceRestore() => _draw.SurfaceRestore();
        public void BackSurfaceRestore() => _draw.BackSurfaceRestore();
        public void SurfaceBlt(int dl, int dt, int dr, int db, int sl, int st, int sr, int sb) => _draw.SurfaceBlt(dl, dt, dr, db, sl, st, sr, sb);
        public bool HasBackSurface() => _draw.HasBackSurface();
        public IntPtr GetBackSurface() => _draw.GetBackSurface();
        public void ReleaseBackSurface(IntPtr hdc) => _draw.ReleaseBackSurface(hdc);
        public void SurfaceFlip() => _draw.SurfaceFlip();
        public bool LockSurface() => _draw.LockSurface();
        public bool UnlockSurface() => _draw.UnlockSurface();
        public bool CreateClipper() => _draw.CreateClipper();
        public bool SetSurfaceClipper() => _draw.SetSurfaceClipper();
        public bool SetClipper(IntPtr hWnd) => _draw.SetClipper(hWnd);
        public long SurfacePitch => _draw.SurfacePitch;
        public IntPtr Surface => _draw.Surface;

        #endregion

        #region DxSound

        public bool CreateDirectSound(int index) => _sound.CreateDirectSound(index);
        public bool SetCooperativeLevel(IntPtr hWnd) => _sound.SetCooperativeLevel(hWnd);
        public List<string> EnumerateSoundCards() => _sound.EnumerateSoundCards();
        public bool CreateDirectSoundBuffer(ushort bitRate, int length) => _sound.CreateDirectSoundBuffer(bitRate, length);
        public void Stop() => _sound.Stop();
        public void Play() => _sound.Play();
        public void Reset() => _sound.Reset();
        public int ReadPlayCursor() => _sound.ReadPlayCursor();
        public void CopyBuffer(int[] buffer) => _sound.CopyBuffer(buffer);
        public bool Lock(int offset, int length) => _sound.Lock(offset, length);
        public bool Unlock() => _sound.Unlock();

        #endregion

        #region DxInput

        public void CreateDirectInput(IntPtr handle) => _input.CreateDirectInput(handle);
        public List<string> EnumerateDevices() => _input.EnumerateDevices();
        public IDxJoystickState JoystickPoll(int id) => _input.JoystickPoll(id);

        #endregion
    }
}
