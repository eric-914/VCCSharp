using VCCSharp.Libraries;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IFileOperations
    {
        unsafe HANDLE FileCreateFile(byte* filename, long desiredAccess);
        long FileSetFilePointer(HANDLE handle, long moveMethod);
        unsafe int FileReadFile(HANDLE handle, byte* buffer, ulong size, ulong* moved);
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

        public unsafe int FileReadFile(HANDLE handle, byte* buffer, ulong size, ulong* moved)
        {
            return Library.FileOperations.FileReadFile(handle, buffer, size, moved);
        }
    }
}
