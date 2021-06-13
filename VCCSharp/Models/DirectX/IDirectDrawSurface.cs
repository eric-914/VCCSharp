using System.Runtime.InteropServices;

namespace VCCSharp.Models.DirectX
{
    [ComImport]
    [Guid("6c14db81-a733-11ce-a521-0020af0be560")]
    public interface IDirectDrawSurface
    {
        long IsLost();
    }
}
