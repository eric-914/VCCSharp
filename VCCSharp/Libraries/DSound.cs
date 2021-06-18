using System;
using VCCSharp.Models;

namespace VCCSharp.Libraries
{
    public interface IDSound
    {
        unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter);
    }

    public class DSound : IDSound
    {
        public unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter)
        {
            return DSoundDLL.DirectSoundCreate(pcGuidDevice, ppDS, pUnkOuter);
        }
    }
}
