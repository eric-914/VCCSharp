﻿using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Libraries
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int DirectSoundEnumerateCallbackTemplate(IntPtr pGuid, IntPtr pDescription, IntPtr pModule, IntPtr pContext);

    public interface IDSound
    {
        unsafe long DirectSoundCreate(_GUID* pGuid, ref IntPtr pInstance, IntPtr pUnknown);
        long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext);
    }

    public class DSound : IDSound
    {
        public unsafe long DirectSoundCreate(_GUID* pGuid, ref IntPtr pInstance, IntPtr pUnknown)
        {
            return DSoundDLL.DirectSoundCreate(pGuid, ref pInstance, pUnknown);
        }

        public long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext)
        {
            return DSoundDLL.DirectSoundEnumerate(pCallback, pContext);
        }
    }
}
