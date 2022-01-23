using System;
using System.Runtime.InteropServices;
using DX8.Internal;
using DX8.Internal.Interfaces;
using DX8.Internal.Libraries;
using DX8.Internal.Models;
using static System.IntPtr;

namespace DX8
{
    internal interface IDxFactory
    {
        IDirectDraw CreateDirectDraw(IDDraw d);
        IDirectDrawSurface CreateSurface(IDirectDraw d, ref DDSURFACEDESC pSurfaceDescription);
        IDirectDrawClipper CreateClipper(IDirectDraw d);
        IDirectSound CreateDirectSound(IDSound d, _GUID guid);
        IDirectSoundBuffer CreateSoundBuffer(IDirectSound d, DSBUFFERDESC pBufferDescription);
        IDirectInput CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid);
    }

    /// <summary>
    /// Isolation of the Dx8 instances (which use Marshalling)
    /// </summary>
    internal class DxFactory : IDxFactory
    {
        private delegate long CreateDelegate(ref IntPtr p);

        private static T Create<T>(CreateDelegate fn) where T : class
        {
            IntPtr p = Zero;

            return fn(ref p) != DxDefine.S_OK ? null : (T)Marshal.GetObjectForIUnknown(p);
        }

        public IDirectDraw CreateDirectDraw(IDDraw d)
        {
            return Create<IDirectDraw>((ref IntPtr p) => d.DirectDrawCreate(Zero, ref p, Zero));
        }

        public IDirectSoundBuffer CreateSoundBuffer(IDirectSound d, DSBUFFERDESC pBufferDescription)
        {
            return Create<IDirectSoundBuffer>((ref IntPtr p) => d.CreateSoundBuffer(ref pBufferDescription, ref p, Zero));
        }

        public IDirectSound CreateDirectSound(IDSound d, _GUID guid)
        {
            return Create<IDirectSound>((ref IntPtr p) => d.DirectSoundCreate(guid, ref p, Zero));
        }

        public IDirectDrawClipper CreateClipper(IDirectDraw d)
        {
            return Create<IDirectDrawClipper>((ref IntPtr p) => d.CreateClipper(0, ref p, Zero));
        }

        public IDirectDrawSurface CreateSurface(IDirectDraw d, ref DDSURFACEDESC pSurfaceDescription)
        {
            IntPtr p = Zero;

            long hr = d.CreateSurface(ref pSurfaceDescription, ref p, Zero);

            return hr != DxDefine.S_OK ? null : (IDirectDrawSurface)Marshal.GetObjectForIUnknown(p);
        }

        public IDirectInput CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid)
        {
            return Create<IDirectInput>((ref IntPtr p) => d.DirectInputCreate(handle, version, guid, ref p));
        }
    }
}
