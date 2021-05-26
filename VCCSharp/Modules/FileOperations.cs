using VCCSharp.Libraries;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IFileOperations
    {
        unsafe HANDLE FileCreateFile(byte* filename, long desiredAccess);
    }

    public class FileOperations : IFileOperations
    {
        public unsafe HANDLE FileCreateFile(byte* filename, long desiredAccess)
        {
            return Library.FileOperations.FileCreateFile(filename, desiredAccess);
        }
    }
}
