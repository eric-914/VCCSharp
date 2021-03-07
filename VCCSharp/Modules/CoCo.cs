using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICoCo
    {
        unsafe CoCoState* GetCoCoState();
        void SetClockSpeed(ushort cycles);
        unsafe float RenderFrame(EmuState* emuState);
        void CocoReset();
        unsafe byte RenderVideoFrame(EmuState* emuState);
        void RenderAudioFrame();
    }

    public class CoCo : ICoCo
    {
        private readonly IThrottle _throttle;
        private readonly IAudio _audio;
        private readonly ICassette _cassette;

        public CoCo(IModules modules)
        {
            _throttle = modules.Throttle;
            _audio = modules.Audio;
            _cassette = modules.Cassette;
        }

        public unsafe CoCoState* GetCoCoState()
        {
            return Library.CoCo.GetCoCoState();
        }

        public void SetClockSpeed(ushort cycles)
        {
            unsafe
            {
                GetCoCoState()->OverClock = cycles;
            }
        }

        public unsafe float RenderFrame(EmuState* emuState)
        {
            if (RenderVideoFrame(emuState) == 1) {
                return 0;
            }

            RenderAudioFrame();

            return _throttle.CalculateFPS();
        }

        public void CocoReset()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                cocoState->HorzInterruptEnabled = 0;
                cocoState->VertInterruptEnabled = 0;
                cocoState->TimerInterruptEnabled = 0;
                cocoState->MasterTimer = 0;
                cocoState->TimerClockRate = 0;
                cocoState->MasterTickCounter = 0;
                cocoState->UnxlatedTickCounter = 0;
                cocoState->OldMaster = 0;

                cocoState->SoundInterrupt = 0;
                cocoState->PicosToSoundSample = 0;
                cocoState->CycleDrift = 0;
                cocoState->CyclesThisLine = 0;
                cocoState->PicosThisLine = 0;
                cocoState->IntEnable = 0;
                cocoState->AudioIndex = 0;
            }
        }

        public unsafe byte RenderVideoFrame(EmuState* emuState)
        {
            return Library.CoCo.RenderVideoFrame(emuState);
        }

        public void RenderAudioFrame()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                switch (cocoState->SoundOutputMode)
                {
                    case 0:
                        _audio.FlushAudioBuffer(cocoState->AudioBuffer, (ushort)(cocoState->AudioIndex << 2));
                        break;

                    case 1:
                        _cassette.FlushCassetteBuffer(cocoState->CassBuffer, cocoState->AudioIndex);
                        break;

                    case 2:
                        _cassette.LoadCassetteBuffer(cocoState->CassBuffer);
                        break;
                }

                cocoState->AudioIndex = 0;
            }
        }
    }
}
