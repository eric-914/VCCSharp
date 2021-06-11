using VCCSharp.Libraries;
using HANDLE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IFileOperations
    {
        HANDLE FileOpenFile(string filename, long desiredAccess);
        HANDLE FileCreateFile(string filename, long desiredAccess);
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
