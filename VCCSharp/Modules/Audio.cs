using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        unsafe AudioState* GetAudioState();
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* buffer, ushort length);
        unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate);
        void PurgeAuxBuffer();
        int GetFreeBlockCount();
        AudioSpectrum Spectrum { get; set; }
        void UpdateSoundBar(ushort left, ushort right);
        byte PauseAudio(byte pause);
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;

        public AudioSpectrum Spectrum { get; set; }

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

                    _modules.DirectSound.DirectSoundStopAndRelease();
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
                    _modules.DirectSound.DirectSoundSetCurrentPosition(0);
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

            _modules.Audio.UpdateSoundBar(leftAverage, rightAverage);

            if ((audioState->InitPassed == Define.FALSE) || (audioState->AudioPause != Define.FALSE))
            {
                return;
            }

            if (GetFreeBlockCount() <= 0)   //this should only kick in when frame skipping or un-throttled
            {
                HandleSlowAudio(byteBuffer, length);

                return;
            }

            audioState->hr = _modules.DirectSound.DirectSoundLock(audioState->BuffOffset, length, &(audioState->SndPointer1), &(audioState->SndLength1), &(audioState->SndPointer2), &(audioState->SndLength2));

            if (audioState->hr != Define.DS_OK)
            {
                return;
            }

            //memcpy(instance->SndPointer1, byteBuffer, instance->SndLength1);	// copy first section of circular buffer
            byte* sourceBuffer = (byte*)audioState->SndPointer1;
            for (int index = 0; index < audioState->SndLength1; index++)
            {
                sourceBuffer[index] = byteBuffer[index];
            }

            if (audioState->SndPointer2 != null)
            { // copy last section of circular buffer if wrapped
              //memcpy(audioState->SndPointer2, byteBuffer + audioState->SndLength1, audioState->SndLength2);
                sourceBuffer = (byte*)audioState->SndPointer2;
                for (int index = 0; index < audioState->SndLength2; index++)
                {
                    sourceBuffer[index] = byteBuffer[index + audioState->SndLength1];
                }
            }

            audioState->hr = _modules.DirectSound.DirectSoundUnlock(audioState->SndPointer1, audioState->SndLength1, audioState->SndPointer2, audioState->SndLength2);// unlock the buffer

            audioState->BuffOffset = (audioState->BuffOffset + length) % audioState->SndBuffLength; //Where to write next

        }

        public unsafe void HandleSlowAudio(byte* buffer, ushort length)
        {
            AudioState* instance = GetAudioState();

            //memcpy(void* _Dst, void const* _Src, size_t _Size);
            //memcpy(audioState->AuxBuffer[audioState->AuxBufferPointer], buffer, length);	

            //Saving buffer to aux stack
            //for (ushort index = 0; index < length; index++)
            //{
            //    instance->AuxBuffer[instance->AuxBufferPointer][index] = buffer[index];
            //}

            Library.Audio.HandleSlowAudio(buffer, length);

            instance->AuxBufferPointer++;		//and chase your own tail
            instance->AuxBufferPointer %= 5;	//At this point we are so far behind we may as well drop the buffer
        }

        public int GetFreeBlockCount()
        {
            unsafe
            {
                ulong playCursor = 0, writeCursor = 0;
                long maxSize;

                AudioState* instance = GetAudioState();

                if (instance->InitPassed == Define.FALSE || instance->AudioPause == Define.TRUE)
                {
                    return Define.AUDIOBUFFERS;
                }

                _modules.DirectSound.DirectSoundGetCurrentPosition(&playCursor, &writeCursor);

                if (instance->BuffOffset <= playCursor)
                {
                    maxSize = (long)(playCursor - instance->BuffOffset);
                }
                else
                {
                    maxSize = (long)(instance->SndBuffLength - instance->BuffOffset + playCursor);
                }

                return (int)(maxSize / instance->BlockSize);
            }
        }

        public void PurgeAuxBuffer()
        {
            unsafe
            {
                AudioState* instance = GetAudioState();

                if (instance->InitPassed == Define.FALSE || instance->AudioPause == Define.TRUE)
                {
                    return;
                }

                return; //TODO: Why?

                //instance->AuxBufferPointer--;			//Normally points to next free block Point to last used block

                //if (instance->AuxBufferPointer >= 0) //zero is a valid data block
                //{
                //    while (GetFreeBlockCount() <= 0) { }

                //    instance->hr = _modules.DirectSound.DirectSoundLock(instance->BuffOffset, instance->BlockSize, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2));

                //    if (instance->hr != Define.DS_OK) {
                //        return;
                //    }

                //    Library.Audio.PurgeAuxBuffer();

                //    instance->BuffOffset = (instance->BuffOffset + instance->BlockSize) % instance->SndBuffLength;

                //    instance->hr = _modules.DirectSound.DirectSoundUnlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);

                //    instance->AuxBufferPointer--;
                //}

                //instance->AuxBufferPointer = 0;
            }
        }

        /*****************************************************************
        * TODO: This has been ported, but is still being used by ConfigDialogCallbacks.CheckAudioChange(...)
        ******************************************************************/
        public unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate)
        {
            rate = (ushort)(rate & 3);

            if (rate != 0)
            {   //Force 44100 or Mute
                rate = 3;
            }

            AudioState* instance = GetAudioState();

            instance->CurrentRate = rate;

            if (instance->InitPassed == Define.TRUE)
            {
                instance->InitPassed = 0;
                _modules.DirectSound.DirectSoundStop();

                if (_modules.DirectSound.DirectSoundHasBuffer() == Define.TRUE)
                {
                    instance->hr = _modules.DirectSound.DirectSoundBufferRelease();
                }

                if (_modules.DirectSound.DirectSoundHasInterface() == Define.TRUE)
                {
                    instance->hr = _modules.DirectSound.DirectSoundInterfaceRelease();
                }
            }

            instance->SndLength1 = 0;
            instance->SndLength2 = 0;
            instance->BuffOffset = 0;
            instance->AuxBufferPointer = 0;
            instance->BitRate = instance->iRateList[rate];
            instance->BlockSize = (ushort)(instance->BitRate * 4 / Define.TARGETFRAMERATE);
            instance->SndBuffLength = (ushort)(instance->BlockSize * Define.AUDIOBUFFERS);

            int result = 0;

            if (rate != 0)
            {
                instance->hr = _modules.DirectSound.DirectSoundInitialize(guid);    // create a directsound object

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                instance->hr = _modules.DirectSound.DirectSoundSetCooperativeLevel(hWnd);

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                _modules.DirectSound.DirectSoundSetupFormatDataStructure(instance->BitRate);

                _modules.DirectSound.DirectSoundSetupSecondaryBuffer(instance->SndBuffLength);

                instance->hr = _modules.DirectSound.DirectSoundCreateSoundBuffer();

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                // Clear out sound buffers
                instance->hr = _modules.DirectSound.DirectSoundLock(0, (ushort)instance->SndBuffLength, &(instance->SndPointer1), &(instance->SndLength1), &(instance->SndPointer2), &(instance->SndLength2));

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                //memset(instance->SndPointer1, 0, instance->SndBuffLength);
                for (int index = 0; index < instance->SndBuffLength; index++)
                {
                    ((byte*)(instance->SndPointer1))[index] = 0;
                }

                instance->hr = _modules.DirectSound.DirectSoundUnlock(instance->SndPointer1, instance->SndLength1, instance->SndPointer2, instance->SndLength2);

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                _modules.DirectSound.DirectSoundSetCurrentPosition(0);

                instance->hr = _modules.DirectSound.DirectSoundPlay();

                if (instance->hr != Define.DS_OK)
                {
                    return (1);
                }

                instance->InitPassed = 1;
                instance->AudioPause = 0;

                _modules.CoCo.SetAudioRate(instance->iRateList[rate]);
            }

            return result;
        }

        public void UpdateSoundBar(ushort left, ushort right)
        {
            Spectrum?.UpdateSoundBar(left, right);
        }

        public byte PauseAudio(byte pause)
        {
            unsafe
            {
                AudioState* instance = GetAudioState();

                instance->AudioPause = pause;

                if (instance->InitPassed == Define.TRUE)
                {
                    if (instance->AudioPause == Define.TRUE)
                    {
                        instance->hr = _modules.DirectSound.DirectSoundStop();
                    }
                    else
                    {
                        instance->hr = _modules.DirectSound.DirectSoundPlay();
                    }
                }

                return instance->AudioPause;
            }
        }
    }
}
