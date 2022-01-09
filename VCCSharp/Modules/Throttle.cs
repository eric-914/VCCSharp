using System.Threading;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IThrottle
    {
        void CalibrateThrottle();
        void FrameWait();
        void StartRender();
        void EndRender(byte skip);
        float CalculateFps();
    }

    public class Throttle : IThrottle
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;
        private readonly IWinmm _win_mm;

        private LARGE_INTEGER _currentTime;
        private LARGE_INTEGER _startTime;
        private LARGE_INTEGER _masterClock;
        private LARGE_INTEGER _now;
        private LARGE_INTEGER _targetTime;
        private LARGE_INTEGER _oneMs;
        private LARGE_INTEGER _oneFrame;

        private static ushort _frameCount;
        private static float _fps, _fNow, _fLast;
        private float _fMasterClock;
        private byte _frameSkip;

        public Throttle(IModules modules, IKernel kernel, IWinmm winMm)
        {
            _modules = modules;
            _kernel = kernel;
            _win_mm = winMm;
        }

        public void CalibrateThrottle()
        {
            unsafe
            {
                //Needed to get max resolution from the timer normally its 10Ms
                _win_mm.TimeBeginPeriod(1);	
                fixed (LARGE_INTEGER* p = &_masterClock)
                {
                    _kernel.QueryPerformanceFrequency(p);
                }
                _win_mm.TimeEndPeriod(1);

                _oneFrame.QuadPart = _masterClock.QuadPart / (Define.TARGETFRAMERATE);
                _oneMs.QuadPart = _masterClock.QuadPart / 1000;
                _fMasterClock = _masterClock.QuadPart;
            }
        }

        public float CalculateFps()
        {
            unsafe
            {
                if (++_frameCount != Define.FRAMEINTERVAL)
                {
                    return (_fps);
                }

                fixed (LARGE_INTEGER* p = &_now)
                {
                    _kernel.QueryPerformanceCounter(p);
                }

                _fNow = _now.QuadPart;
                _fps = (_fNow - _fLast) / _fMasterClock;
                _fLast = _fNow;
                _frameCount = 0;
                _fps = Define.FRAMEINTERVAL / _fps;

                return _fps;
            }
        }

        public void FrameWait()
        {
            IAudio audio = _modules.Audio;

            UpdateCurrentTime();

            //If we have more that 2Ms till the end of the frame
            while (_targetTime.QuadPart - _currentTime.QuadPart > (_oneMs.QuadPart * 2))
            {
                //TODO: I suspect this doesn't really return us back in 1ms.
                //--LARGE_INTEGER SleepRes;
                Thread.Sleep(1);	//Give about 1Ms back to the system

                UpdateCurrentTime();
            }

            if (audio.CurrentRate != 0)
            {
                //_modules.Audio.PurgeAuxBuffer();

                if (_frameSkip == 1)
                {
                    int half = Define.AUDIOBUFFERS / 2;

                    //Don't let the buffer get less than half full
                    if (audio.GetFreeBlockCount() > half)
                    {
                        return;
                    }

                    // Don't let it fill up either;
                    while (audio.GetFreeBlockCount() < 1)
                    {
                        //--wait
                    }
                }
            }

            //Poll until frame end.
            while (_currentTime.QuadPart < _targetTime.QuadPart)
            {	
                UpdateCurrentTime();
            }
        }

        private void UpdateCurrentTime()
        {
            unsafe
            {
                fixed (LARGE_INTEGER* p = &_currentTime)
                {
                    _kernel.QueryPerformanceCounter(p);
                }
            }
        }

        public void StartRender()
        {
            unsafe
            {
                fixed (LARGE_INTEGER* p = &_startTime)
                {
                    _kernel.QueryPerformanceCounter(p);
                }
            }
        }

        public void EndRender(byte skip)
        {
            _frameSkip = skip;
            _targetTime.QuadPart = _startTime.QuadPart + _oneFrame.QuadPart * _frameSkip;
        }
    }
}
