using System;
using System.Runtime.InteropServices;

namespace DX8.Converters
{
    public static class IntPtrConverter
    {
        public static IntPtr Convert<T>(T[] array)
        {
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);

            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
    }
}
