using VCCSharp.Libraries;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IFileOperations
    {
        HANDLE FileOpenFile(string filename, long desiredAccess);
        HANDLE FileCreateFile(string filename, long desiredAccess);
        long FileSetFilePointer(HANDLE handle, long moveMethod, long offset = 0);
        unsafe int FileReadFile(HANDLE handle, byte* buffer, ulong size, ulong* moved);
        int FileCloseHandle(HANDLE handle);
        int FileFlushFileBuffers(HANDLE handle);
        unsafe int FileWriteFile(HANDLE handle, byte* buffer, int size);
        int FileWriteFile(HANDLE handle, string text);
    }

    public class FileOperations : IFileOperations
    {
        public HANDLE FileOpenFile(string filename, long desiredAccess)
        {
            return Library.FileOperations.FileOpenFile(filename, desiredAccess);
        }

        public HANDLE FileCreateFile(string filename, long desiredAccess)
        {
            return Library.FileOperations.FileCreateFile(filename, desiredAccess);
        }

        public long FileSetFilePointer(HANDLE handle, long moveMethod, long offset = 0)
        {
            return Library.FileOperations.FileSetFilePointer(handle, moveMethod, offset);
        }

        public unsafe int FileReadFile(HANDLE handle, byte* buffer, ulong size, ulong* moved)
        {
            return Library.FileOperations.FileReadFile(handle, buffer, size, moved);
        }

        public int FileCloseHandle(HANDLE handle)
        {
            return Library.FileOperations.FileCloseHandle(handle);
        }

        public int FileFlushFileBuffers(HANDLE handle)
        {
            return Library.FileOperations.FileFlushFileBuffers(handle);
        }

        public unsafe int FileWriteFile(HANDLE handle, byte* buffer, int size)
        {
            return Library.FileOperations.FileWriteFile(handle, buffer, size);
        }

        public unsafe int FileWriteFile(HANDLE handle, string text)
        {
            fixed (byte* buffer = Converter.ToByteArray(text))
            {
                return Library.FileOperations.FileWriteFile(handle, buffer, text.Length);
            }
        }
    }
}
