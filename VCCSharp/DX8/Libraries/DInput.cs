using System;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Libraries
{
    public interface IDInput
    {
        int DirectInputCreate(IntPtr handle, uint version, _GUID pId, ref IntPtr di);
    }

    public class DInput : IDInput
    {
        public int DirectInputCreate(IntPtr handle, uint version, _GUID pId, ref IntPtr di)
        {
            return DInputDLL.DirectInput8Create(handle, version, pId, ref di, IntPtr.Zero);
        }
    }
}
