using VCCSharp.Libraries;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IResource
    {
        string ResourceAppTitle(HINSTANCE hResources);
    }

    public class Resource : IResource
    {
        public string ResourceAppTitle(HINSTANCE hResources)
        {
            byte[] buffer = new byte[Define.MAX_LOADSTRING];

            Library.Resource.ResourceAppTitle(hResources, buffer);

            return Converter.ToString(buffer);
        }
    }
}
