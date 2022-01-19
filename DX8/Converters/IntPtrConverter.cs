using System;
using System.Runtime.InteropServices;

namespace DX8.Converters
{
    public static class IntPtrConverter
    {
        /// <summary>
        /// Get the IntPtr for the given array
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">Target array</param>
        /// <returns>IntPtr</returns>
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
