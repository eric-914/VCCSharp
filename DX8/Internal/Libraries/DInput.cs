using System;
using DX8.Internal.Models;

namespace DX8.Internal.Libraries
{
    internal interface IDInput
    {
        int DirectInputCreate(IntPtr handle, uint version, _GUID pGuid, ref IntPtr pInstance);
    }

    internal class DInput : IDInput
    {
        public int DirectInputCreate(IntPtr handle, uint version, _GUID pGuid, ref IntPtr pInstance)
        {
            return DInputDLL.DirectInput8Create(handle, version, pGuid, ref pInstance, IntPtr.Zero);
        }
    }
}
