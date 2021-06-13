using System.Runtime.InteropServices;

namespace VCCSharp.Models.DirectX
{
    //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6c14db85-a733-11ce-a521-0020af0be560")]
    //GUID_ENTRY(0x6c14db85,0xa733,0x11ce,0xa5,0x21,0x00,0x20,0xaf,0x0b,0xe5,0x60,IID_IDirectDrawClipper)
    public interface IDirectDrawClipper
    {
    }
}
