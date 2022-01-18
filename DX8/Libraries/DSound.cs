using System;
using DX8.Models;

namespace DX8.Libraries
{
    public interface IDSound
    {
        unsafe long DirectSoundCreate(_GUID* pGuid, ref IntPtr pInstance, IntPtr pUnknown);
        long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext);
    }

    public class DSound : IDSound
    {
        public unsafe long DirectSoundCreate(_GUID* pGuid, ref IntPtr pInstance, IntPtr pUnknown)
        {
            _GUID p = *pGuid;
            return DSoundDLL.DirectSoundCreate(ref p, ref pInstance, pUnknown);
        }

        public long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext)
        {
            return DSoundDLL.DirectSoundEnumerate(pCallback, pContext);
        }
    }
}
