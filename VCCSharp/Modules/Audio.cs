using VCCSharp.IoC;
using VCCSharp.Models;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IAudio
    {
        short SoundDeInit();
        void ResetAudio();
        unsafe void FlushAudioBuffer(uint* buffer, ushort length);
        unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate);
        void PurgeAuxBuffer();
        int GetFreeBlockCount();
        AudioSpectrum Spectrum { get; set; }
        bool PauseAudio(bool pause);
        ushort CurrentRate { get; set; }
    }

    public class Audio : IAudio
    {
        private readonly IModules _modules;

        public AudioSpectrum Spectrum { get; set; }
        public ushort CurrentRate { get; set; }

        private bool _initPassed;
        private bool _audioPause;

        private ushort _bitRate;
        private ushort _blockSize;

        private readonly ushort[] _rateList = { 0, 11025, 22050, 44100 };

        private int _hr;

        private byte _auxBufferPointer;

        private uint _sndLength1;
        private uint _sndLength2;
        private uint _sndBuffLength;
        private uint _buffOffset;

        public unsafe void* SndPointer1;
        public unsafe void* SndPointer2;

        public Audio(IModules modules)
        {
            _modules = modules;
        }

        public short SoundDeInit()
        {
            if (_initPassed)
            {
                _initPassed = false;

                _modules.DirectSound.DirectSoundStopAndRelease();
            }

            return 0;
        }

        public void ResetAudio()
        {
            _modules.CoCo.SetAudioRate(_rateList[CurrentRate]);

            if (_initPassed)
            {
                _modules.DirectSound.DirectSoundSetCurrentPosition(0);
            }

            _buffOffset = 0;
            _auxBufferPointer = 0;
        }

        private unsafe int DirectSoundLock(ushort length)
        {
            int Invoke(void** sp1, uint* sl1, void** sp2, uint* sl2)
            {
                return _modules.DirectSound.DirectSoundLock(_buffOffset, length, sp1, sl1, sp2, sl2);
            }

            fixed (uint* sl1 = &_sndLength1)
            {
                fixed (uint* sl2 = &_sndLength2)
                {
                    void* s1 = SndPointer1;
                    void* s2 = SndPointer2;

                    var result = Invoke(&s1, sl1, &s2, sl2);

                    SndPointer1 = s1;
                    SndPointer2 = s2;

                    return result;
                }
            }
        }

        private unsafe int DirectSoundUnlock()
        {
            void* s1 = SndPointer1;
            void* s2 = SndPointer2;

            var result = _modules.DirectSound.DirectSoundUnlock(s1, _sndLength1, s2, _sndLength2);

            SndPointer1 = s1;
            SndPointer2 = s2;

            return result;
        }

        public unsafe void FlushAudioBuffer(uint* buffer, ushort length)
        {
            byte* byteBuffer = (byte*)buffer;

            ushort leftAverage = (ushort)(buffer[0] >> 16);
            ushort rightAverage = (ushort)(buffer[0] & 0xFFFF);

            _modules.Audio.Spectrum?.UpdateSoundBar(leftAverage, rightAverage);

            if (!_initPassed || _audioPause)
            {
                return;
            }

            if (GetFreeBlockCount() <= 0)   //this should only kick in when frame skipping or un-throttled
            {
                HandleSlowAudio(byteBuffer, length);

                return;
            }

            //_hr = _modules.DirectSound.DirectSoundLock(audioState->BuffOffset, length, &(audioState->SndPointer1), &(SndLength1), &(audioState->SndPointer2), &(SndLength2));
            _hr = DirectSoundLock(length);

            if (_hr != Define.DS_OK)
            {
                return;
            }

            //memcpy(instance->SndPointer1, byteBuffer, instance->SndLength1);	// copy first section of circular buffer
            byte* sourceBuffer = (byte*)SndPointer1;
            for (int index = 0; index < _sndLength1; index++)
            {
                sourceBuffer[index] = byteBuffer[index];
            }

            if (SndPointer2 != null)
            { // copy last section of circular buffer if wrapped
              //memcpy(audioState->SndPointer2, byteBuffer + audioState->SndLength1, audioState->SndLength2);
                sourceBuffer = (byte*)SndPointer2;
                for (int index = 0; index < _sndLength2; index++)
                {
                    sourceBuffer[index] = byteBuffer[index + _sndLength1];
                }
            }

            _hr = DirectSoundUnlock();// unlock the buffer

            _buffOffset = (_buffOffset + length) % _sndBuffLength; //Where to write next
        }

        public unsafe void HandleSlowAudio(byte* buffer, ushort length)
        {
            //memcpy(void* _Dst, void const* _Src, size_t _Size);
            //memcpy(audioState->AuxBuffer[audioState->AuxBufferPointer], buffer, length);	

            //Saving buffer to aux stack
            //for (ushort index = 0; index < length; index++)
            //{
            //    instance->AuxBuffer[instance->AuxBufferPointer][index] = buffer[index];
            //}

            //Library.Audio.HandleSlowAudio(instance->AuxBufferPointer, buffer, length);

            //HandleSlowAudio was this:
            //memcpy(instance->AuxBuffer[index], buffer, length);	//Saving buffer to aux stack

            _auxBufferPointer++;		//and chase your own tail
            _auxBufferPointer %= 5;	//At this point we are so far behind we may as well drop the buffer
        }

        public int GetFreeBlockCount()
        {
            unsafe
            {
                ulong playCursor = 0, writeCursor = 0;
                long maxSize;

                if (!_initPassed || _audioPause)
                {
                    return Define.AUDIOBUFFERS;
                }

                _modules.DirectSound.DirectSoundGetCurrentPosition(&playCursor, &writeCursor);

                if (_buffOffset <= playCursor)
                {
                    maxSize = (long)(playCursor - _buffOffset);
                }
                else
                {
                    maxSize = (long)(_sndBuffLength - _buffOffset + playCursor);
                }

                return (int)(maxSize / _blockSize);
            }
        }

        public void PurgeAuxBuffer()
        {
            if (!_initPassed || _audioPause)
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

            CurrentRate = rate;

            if (_initPassed)
            {
                _initPassed = false;
                _modules.DirectSound.DirectSoundStop();

                if (_modules.DirectSound.DirectSoundHasBuffer())
                {
                    _hr = _modules.DirectSound.DirectSoundBufferRelease();
                }

                if (_modules.DirectSound.DirectSoundHasInterface())
                {
                    _hr = _modules.DirectSound.DirectSoundInterfaceRelease();
                }
            }

            _sndLength1 = 0;
            _sndLength2 = 0;
            _buffOffset = 0;
            _auxBufferPointer = 0;
            _bitRate = _rateList[rate];
            _blockSize = (ushort)(_bitRate * 4 / Define.TARGETFRAMERATE);
            _sndBuffLength = (ushort)(_blockSize * Define.AUDIOBUFFERS);

            int result = 0;

            if (rate != 0)
            {
                _hr = _modules.DirectSound.DirectSoundInitialize(guid);    // create a direct sound object

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                // set cooperation level normal
                _hr = _modules.DirectSound.DirectSoundSetCooperativeLevel(hWnd);

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                _modules.DirectSound.DirectSoundSetupFormatDataStructure(_bitRate);

                _modules.DirectSound.DirectSoundSetupSecondaryBuffer(_sndBuffLength);

                _hr = _modules.DirectSound.DirectSoundCreateSoundBuffer();

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                // Clear out sound buffers
                //_hr = _modules.DirectSound.DirectSoundLock(0, (ushort)instance->SndBuffLength, &(instance->SndPointer1), &(SndLength1), &(instance->SndPointer2), &(SndLength2));
                _hr = DirectSoundLock((ushort)_sndBuffLength);

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                //memset(instance->SndPointer1, 0, instance->SndBuffLength);
                for (int index = 0; index < _sndBuffLength; index++)
                {
                    ((byte*)(SndPointer1))[index] = 0;
                }

                _hr = DirectSoundUnlock();

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                _modules.DirectSound.DirectSoundSetCurrentPosition(0);

                _hr = _modules.DirectSound.DirectSoundPlay();

                if (_hr != Define.DS_OK)
                {
                    return 1;
                }

                _initPassed = true;
                _audioPause = false;

                _modules.CoCo.SetAudioRate(_rateList[rate]);
            }

            return result;
        }

        public bool PauseAudio(bool pause)
        {
            _audioPause = pause;

            if (_initPassed)
            {
                _hr = _audioPause
                    ? _modules.DirectSound.DirectSoundStop()
                    : _modules.DirectSound.DirectSoundPlay();
            }

            return _audioPause;
        }

        #region This is what was in Audio.c

        void Library_Audio_PurgeAuxBuffer()
        {
            //        memcpy(instance->SndPointer1, instance->AuxBuffer[instance->AuxBufferPointer], instance->SndLength1);

            //        if (instance->SndPointer2 != NULL) {
            //            memcpy(instance->SndPointer2, (instance->AuxBuffer[instance->AuxBufferPointer] + (instance->SndLength1 >> 2)), instance->SndLength2);
            //        }
        }

        #endregion

    }
}
