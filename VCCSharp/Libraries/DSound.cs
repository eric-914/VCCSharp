﻿using System;
using VCCSharp.Models;

namespace VCCSharp.Libraries
{
    public interface IDSound
    {
        unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter);
        long DirectSoundEnumerate(IntPtr lpDSEnumCallback, IntPtr lpContext);
    }

    public class DSound : IDSound
    {
        public unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter)
        {
            return DSoundDLL.DirectSoundCreate(pcGuidDevice, ppDS, pUnkOuter);
        }

        public long DirectSoundEnumerate(IntPtr lpDSEnumCallback, IntPtr lpContext)
        {
            return DSoundDLL.DirectSoundEnumerate(lpDSEnumCallback, lpContext);
        }
    }
}
