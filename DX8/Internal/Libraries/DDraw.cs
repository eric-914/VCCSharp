using System;

namespace DX8.Internal.Libraries
{
    // ReSharper disable once IdentifierTypo
    internal interface IDDraw
    {
        int DirectDrawCreate(IntPtr pGuid, ref IntPtr pInstance, IntPtr pUnknown);
    }

    // ReSharper disable once IdentifierTypo
    internal class DDraw : IDDraw
    {
        public int DirectDrawCreate(IntPtr pGuid, ref IntPtr pInstance, IntPtr pUnknown)
        {
            return DDrawDLL.DirectDrawCreate(pGuid, ref pInstance, pUnknown);
        }
    }
}
