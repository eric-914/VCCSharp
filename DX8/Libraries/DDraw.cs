using System;

namespace DX8.Libraries
{
    // ReSharper disable once IdentifierTypo
    public interface IDDraw
    {
        int DirectDrawCreate(IntPtr pGuid, ref IntPtr pInstance, IntPtr pUnknown);
    }

    // ReSharper disable once IdentifierTypo
    public class DDraw : IDDraw
    {
        public int DirectDrawCreate(IntPtr pGuid, ref IntPtr pInstance, IntPtr pUnknown)
        {
            return DDrawDLL.DirectDrawCreate(pGuid, ref pInstance, pUnknown);
        }
    }
}
