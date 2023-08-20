using DX8.Internal.Interfaces;
using DX8.Internal.Libraries;
using DX8.Internal.Models;
using System.Runtime.InteropServices;
using static System.IntPtr;

namespace DX8.Internal
{
    internal interface IDxFactoryInternal
    {
        IDirectDraw? CreateDirectDraw(IDDraw d);
        IDirectDrawSurface? CreateSurface(IDirectDraw? d, ref DDSURFACEDESC pSurfaceDescription);
        IDirectDrawClipper? CreateClipper(IDirectDraw? d);
        IDirectSound? CreateDirectSound(IDSound d, _GUID guid);
        IDirectSoundBuffer? CreateSoundBuffer(IDirectSound d, DSBUFFERDESC pBufferDescription);
        IDirectInput? CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid);
    }

    /// <summary>
    /// Isolation of the Dx8 instances (which use Marshalling)
    /// </summary>
    internal class DxFactoryInternal : IDxFactoryInternal
    {
        private delegate long CreateDelegate(ref IntPtr p);

        /// <summary>
        /// Warning	CA1416
        /// This call site is reachable on all platforms.
        /// 'Marshal.GetObjectForIUnknown(IntPtr)' is only supported on: 'windows'.
        /// </summary>
        /// <remarks>
        /// NOTE: we're already dependent on DX8 COM objects, so we are kind of stuck with Windows-specific for the time being.
        /// </remarks>
        private static T GetObjectForIUnknown<T>(IntPtr p) where T : class
        {
#pragma warning disable CA1416 // Validate platform compatibility
            return (T)Marshal.GetObjectForIUnknown(p);
#pragma warning restore CA1416 // Validate platform compatibility
        }

        private static T? Create<T>(CreateDelegate fn) where T : class
        {
            IntPtr p = Zero;

            return fn(ref p) != DxDefine.S_OK ? null : GetObjectForIUnknown<T>(p);
        }

        public IDirectDraw? CreateDirectDraw(IDDraw d)
        {
            return Create<IDirectDraw>((ref IntPtr p) => d.DirectDrawCreate(Zero, ref p, Zero));
        }

        public IDirectSoundBuffer? CreateSoundBuffer(IDirectSound d, DSBUFFERDESC pBufferDescription)
        {
            return Create<IDirectSoundBuffer>((ref IntPtr p) => d.CreateSoundBuffer(ref pBufferDescription, ref p, Zero));
        }

        public IDirectSound? CreateDirectSound(IDSound d, _GUID guid)
        {
            return Create<IDirectSound>((ref IntPtr p) => d.DirectSoundCreate(guid, ref p, Zero));
        }

        public IDirectDrawClipper? CreateClipper(IDirectDraw? d)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));

            return Create<IDirectDrawClipper>((ref IntPtr p) => d.CreateClipper(0, ref p, Zero));
        }

        public IDirectDrawSurface? CreateSurface(IDirectDraw? d, ref DDSURFACEDESC pSurfaceDescription)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));

            IntPtr p = Zero;

            long hr = d.CreateSurface(ref pSurfaceDescription, ref p, Zero);

            return hr != DxDefine.S_OK ? null : GetObjectForIUnknown<IDirectDrawSurface>(p);
        }

        public IDirectInput? CreateDirectInput(IDInput d, IntPtr handle, uint version, _GUID guid)
        {
            return Create<IDirectInput>((ref IntPtr p) => d.DirectInputCreate(handle, version, guid, ref p));
        }
    }
}
