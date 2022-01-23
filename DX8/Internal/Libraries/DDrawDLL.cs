// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

using System.Runtime.InteropServices;
using GUID = System.IntPtr;
using IUnknown = System.IntPtr;
using LPDIRECTDRAW = System.IntPtr;

namespace DX8.Internal.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DDrawDLL
    {
        private const string Dll = "ddraw.dll";

        /// <summary>
        /// Creates an instance of a DirectDraw object. A DirectDraw object that is created by using this function does not support the complete set of Direct3D interfaces in DirectX 7.0. To create a DirectDraw object that is capable of exposing all of the features of Direct3D in DirectX 7.0, use the DirectDrawCreateEx function.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/nf-ddraw-directdrawcreate"/>
        /// <param name="lpGUID">A pointer to the globally unique identifier (GUID) that represents the driver to be created.</param>
        /// <param name="lplpDD">A pointer to a variable to be set to a valid IDirectDraw interface pointer if the call succeeds.</param>
        /// <param name="pUnkOuter">Allows for future compatibility with COM aggregation features. Presently, however, this function returns an error if this parameter is anything but NULL.</param>
        /// <returns>
        /// If the function succeeds, the return value is DD_OK.
        /// If it fails, the function can return one of the following error values:
        /// DDERR_DIRECTDRAWALREADYCREATED | DDERR_GENERIC | DDERR_INVALIDDIRECTDRAWGUID | DDERR_INVALIDPARAMS | DDERR_NODIRECTDRAWHW |DDERR_OUTOFMEMORY
        /// </returns>
        [DllImport(Dll)]
        public static extern int DirectDrawCreate(GUID lpGUID, ref LPDIRECTDRAW lplpDD, IUnknown pUnkOuter);
    }
}
