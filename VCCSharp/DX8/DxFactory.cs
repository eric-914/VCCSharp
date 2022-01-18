using DX8.Interfaces;
using DX8.Libraries;
using DX8.Models;
using System;
using System.Runtime.InteropServices;
using VCCSharp.Models;
using static System.IntPtr;

namespace VCCSharp.DX8
{
    public interface IDxFactory
    {
        IDirectDraw CreateDirectDraw(IDDraw d);
        unsafe IDirectDrawSurface CreateSurface(IDirectDraw d, DDSURFACEDESC* pSurfaceDescription);
        IDirectDrawClipper CreateClipper(IDirectDraw d);
        unsafe IDirectSound CreateDirectSound(IDSound d, _GUID* guid);
        unsafe IDirectSoundBuffer CreateSoundBuffer(IDirectSound d, DSBUFFERDESC* pBufferDescription);
        IDirectInput CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid);
    }

    /// <summary>
    /// Isolation of the Dx8 instances (which use Marshalling)
    /// </summary>
    public class DxFactory : IDxFactory
    {
        private delegate long CreateDelegate(ref IntPtr p);

        private static T Create<T>(CreateDelegate fn) where T : class
        {
            IntPtr p = Zero;

            return fn(ref p) != Define.DS_OK ? null : (T)Marshal.GetObjectForIUnknown(p);
        }

        public IDirectDraw CreateDirectDraw(IDDraw d)
        {
            return Create<IDirectDraw>((ref IntPtr p) => d.DirectDrawCreate(Zero, ref p, Zero));
        }

        public unsafe IDirectSoundBuffer CreateSoundBuffer(IDirectSound d, DSBUFFERDESC* pBufferDescription)
        {
            return Create<IDirectSoundBuffer>((ref IntPtr p) => d.CreateSoundBuffer(pBufferDescription, ref p, Zero));
        }

        public unsafe IDirectSound CreateDirectSound(IDSound d, _GUID* guid)
        {
            return Create<IDirectSound>((ref IntPtr p) => d.DirectSoundCreate(guid, ref p, Zero));
        }

        public IDirectDrawClipper CreateClipper(IDirectDraw d)
        {
            return Create<IDirectDrawClipper>((ref IntPtr p) => d.CreateClipper(0, ref p, Zero));
        }

        public unsafe IDirectDrawSurface CreateSurface(IDirectDraw d, DDSURFACEDESC* pSurfaceDescription)
        {
            return Create<IDirectDrawSurface>((ref IntPtr p) => d.CreateSurface(pSurfaceDescription, ref p, Zero));
        }

        public IDirectInput CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid)
        {
            return Create<IDirectInput>((ref IntPtr p) => d.DirectInputCreate(handle, version, guid, ref p));
        }
    }
}
