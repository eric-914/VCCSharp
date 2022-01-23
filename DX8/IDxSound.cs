using System;
using System.Collections.Generic;

namespace DX8
{
    public interface IDxSound
    {
        bool CreateDirectSound(int index);
        bool SetCooperativeLevel(IntPtr hWnd);

        List<string> EnumerateSoundCards();

        bool CreateDirectSoundBuffer(ushort bitRate, int length);

        void Stop();
        void Play();
        void Reset();
        int ReadPlayCursor();

        void CopyBuffer(int[] buffer);

        bool Lock(int offset, int length);
        bool Unlock();
    }
}