using System.ComponentModel;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        unsafe AudioState* GetAudioState();
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* buffer, ushort length);
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;

        public Audio(IModules modules)
        {
            _modules = modules;
        }

        public unsafe AudioState* GetAudioState()
        {
            return Library.Audio.GetAudioState();
        }

        public short SoundDeInit()
        {
            unsafe
            {
                AudioState* audioState = GetAudioState();

                if (audioState->InitPassed != Define.FALSE)
                {
                    audioState->InitPassed = 0;

                    _modules.DirectSound.StopAndRelease();
                }

                return 0;
            }
        }

        public void ResetAudio()
        {
            unsafe
            {
                AudioState* audioState = GetAudioState();

                _modules.CoCo.SetAudioRate(audioState->iRateList[audioState->CurrentRate]);

                if (audioState->InitPassed == Define.TRUE)
                {
                    _modules.DirectSound.SetCurrentPosition(0);
                }

                audioState->BuffOffset = 0;
                audioState->AuxBufferPointer = 0;
            }
        }

        public unsafe void FlushAudioBuffer(uint* buffer, ushort length)
        {
            AudioState* audioState = GetAudioState();

            byte* byteBuffer = (byte*)buffer;

            ushort leftAverage = (ushort)(buffer[0] >> 16);
            ushort rightAverage = (ushort)(buffer[0] & 0xFFFF);

            _modules.Config.UpdateSoundBar(leftAverage, rightAverage);

            if ((audioState->InitPassed == Define.FALSE) || (audioState->AudioPause != Define.FALSE))
            {
                return;
            }

            if (GetFreeBlockCount() <= 0)	//this should only kick in when frame skipping or un-throttled
            {
                HandleSlowAudio(byteBuffer, length);

                return;
            }

            audioState->hr = _modules.DirectSound.DirectSoundLock(audioState->BuffOffset, length, &(audioState->SndPointer1), &(audioState->SndLength1), &(audioState->SndPointer2), &(audioState->SndLength2));

            if (audioState->hr != Define.DS_OK) {
                return;
            }

            //memcpy(instance->SndPointer1, byteBuffer, instance->SndLength1);	// copy first section of circular buffer
            byte* sourceBuffer = (byte*)audioState->SndPointer1;
            for (int index = 0; index < audioState->SndLength1; index++)
            {
                sourceBuffer[index] = byteBuffer[index];
            }

            if (audioState->SndPointer2 != null) { // copy last section of circular buffer if wrapped
                //memcpy(audioState->SndPointer2, byteBuffer + audioState->SndLength1, audioState->SndLength2);
                sourceBuffer = (byte*)audioState->SndPointer2;
                for (int index = 0; index < audioState->SndLength2; index++)
                {
                    sourceBuffer[index] = byteBuffer[index + audioState->SndLength1];
                }
            }

            audioState->hr = _modules.DirectSound.DirectSoundUnlock(audioState->SndPointer1, audioState->SndLength1, audioState->SndPointer2, audioState->SndLength2);// unlock the buffer

            audioState->BuffOffset = (audioState->BuffOffset + length) % audioState->SndBuffLength;	//Where to write next

        }

        public int GetFreeBlockCount()
        {
            return Library.Audio.GetFreeBlockCount();
        }

        public unsafe void HandleSlowAudio(byte* buffer, ushort length)
        {
            //memcpy(void* _Dst, void const* _Src, size_t _Size);
            //memcpy(audioState->AuxBuffer[audioState->AuxBufferPointer], buffer, length);	

            ////Saving buffer to aux stack
            //for (ushort index = 0; index < length; index++)
            //{
            //    audioState->AuxBuffer[audioState->AuxBufferPointer][index] = buffer[index];
            //}

            //audioState->AuxBufferPointer++;		//and chase your own tail
            //audioState->AuxBufferPointer %= 5;	//At this point we are so far behind we may as well drop the buffer

            Library.Audio.HandleSlowAudio(buffer, length);
        }
    }
}
