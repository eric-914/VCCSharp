using System;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Libraries
{
    public interface IDInput
    {
        int DirectInputCreate(IntPtr handle, uint version, _GUID pGuid, ref IntPtr pInstance);
    }

    public class DInput : IDInput
    {
        public int DirectInputCreate(IntPtr handle, uint version, _GUID pGuid, ref IntPtr pInstance)
        {
            return DInputDLL.DirectInput8Create(handle, version, pGuid, ref pInstance, IntPtr.Zero);
        }
    }
}
