using System;
using DX8.Models;

namespace DX8.Libraries
{
    public interface IDSound
    {
        long DirectSoundCreate(_GUID pGuid, ref IntPtr pInstance, IntPtr pUnknown);
        long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext);
    }

    /// <summary>
    /// Wrapper around the static DSoundDLL class
    /// </summary>
    public class DSound : IDSound
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
