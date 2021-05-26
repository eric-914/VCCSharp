using VCCSharp.Libraries;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IFileOperations
    {
        unsafe HANDLE FileCreateFile(byte* filename, long desiredAccess);
        long FileSetFilePointer(HANDLE handle, long moveMethod);
    }

    public class FileOperations : IFileOperations
    {
        public unsafe HANDLE FileCreateFile(byte* filename, long desiredAccess)
        {
            return Library.FileOperations.FileCreateFile(filename, desiredAccess);
        }

        public long FileSetFilePointer(HANDLE handle, long moveMethod)
        {
            return Library.FileOperations.FileSetFilePointer(handle, moveMethod);
        }
    }
}
