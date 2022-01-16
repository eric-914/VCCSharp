using System;

namespace VCCSharp.DX8.Libraries
{
    // ReSharper disable once IdentifierTypo
    public interface IDDraw
    {
        unsafe int DirectDrawCreate(IntPtr lpGuid, IntPtr* pDirectDraw, IntPtr pUnkOuter);
    }

    // ReSharper disable once IdentifierTypo
    public class DDraw : IDDraw
    {
        public unsafe int DirectDrawCreate(IntPtr lpGuid, IntPtr* pDirectDraw, IntPtr pUnkOuter)
        {
            return DDrawDLL.DirectDrawCreate(lpGuid, pDirectDraw, pUnkOuter);
        }
    }
}
