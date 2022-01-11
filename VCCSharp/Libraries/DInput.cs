using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using LPVOID = System.IntPtr;

namespace VCCSharp.Libraries
{
    public interface IDInput
    {
        int DirectInputCreate(HINSTANCE handle, uint version, _GUID pId, ref LPVOID di);
    }

    public class DInput : IDInput
    {
        public int DirectInputCreate(HINSTANCE handle, uint version, _GUID pId, ref LPVOID di)
        {
            return DInputDLL.DirectInput8Create(handle, version, pId, ref di, HINSTANCE.Zero);
        }
    }
}
