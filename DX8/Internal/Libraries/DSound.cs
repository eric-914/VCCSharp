using System;
using DX8.Internal.Models;

namespace DX8.Internal.Libraries
{
    internal interface IDSound
    {
        long DirectSoundCreate(_GUID pGuid, ref IntPtr pInstance, IntPtr pUnknown);
        long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext);
    }

    /// <summary>
    /// Wrapper around the static DSoundDLL class
    /// </summary>
    internal class DSound : IDSound
    {
        public long DirectSoundCreate(_GUID pGuid, ref IntPtr pInstance, IntPtr pUnknown)
        {
            return DSoundDLL.DirectSoundCreate(ref pGuid, ref pInstance, pUnknown);
        }

        public long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext)
        {
            return DSoundDLL.DirectSoundEnumerate(pCallback, pContext);
        }
    }
}
