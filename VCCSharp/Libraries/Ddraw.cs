using System;

namespace VCCSharp.Libraries
{
    // ReSharper disable once IdentifierTypo
    public interface IDdraw
    {
        unsafe int DirectDrawCreate(IntPtr lpGuid, IntPtr* pDirectDraw, IntPtr pUnkOuter);
    }

    // ReSharper disable once IdentifierTypo
    public class Ddraw : IDdraw
    {
        public unsafe int DirectDrawCreate(IntPtr lpGuid, IntPtr* pDirectDraw, IntPtr pUnkOuter)
        {
            return DdrawDLL.DirectDrawCreate(lpGuid, pDirectDraw, pUnkOuter);
        }
    }
}
