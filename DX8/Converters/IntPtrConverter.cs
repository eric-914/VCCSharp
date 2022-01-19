﻿using System;
using System.Runtime.InteropServices;

namespace DX8.Converters
{
    public static class IntPtrConverter
    {
        /// <summary>
        /// Get the IntPtr for the given array
        /// </summary>
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

        /// <summary>
        /// Get the IntPtr for the given instance
        /// </summary>
        public static IntPtr Convert<T>(T instance)
        {
            GCHandle handle = GCHandle.Alloc(instance, GCHandleType.Pinned);

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
