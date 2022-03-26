using System.Threading;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Libraries.Models;
using VCCSharp.Models;

namespace VCCSharp.Modules;

public interface IThrottle : IModule
{
    void CalibrateThrottle();
    void FrameWait();
    void StartRender();
    void EndRender(int skip);
    float CalculateFps();
}

public class Throttle : IThrottle
{
    private readonly IModules _modules;
    private readonly IKernel _kernel;
    // ReSharper disable once InconsistentNaming
    private readonly IWinmm _win_mm;

    private LARGE_INTEGER _currentTime;
    private LARGE_INTEGER _startTime;
    private LARGE_INTEGER _masterClock;
    private LARGE_INTEGER _now;
    private LARGE_INTEGER _targetTime;
    private LARGE_INTEGER _oneMs;
    private LARGE_INTEGER _oneFrame;

    private ushort _frameCount;
    private float _fps, _fNow, _fLast;
    private float _fMasterClock;
    private int _frameSkip;

    public Throttle(IModules modules, IKernel kernel, IWinmm winMm)
    {
        _modules = modules;
        _kernel = kernel;
        _win_mm = winMm;
    }

    public void CalibrateThrottle()
    {
        //Needed to get max resolution from the timer normally its 10Ms
        _win_mm.TimeBeginPeriod(1);
        _kernel.QueryPerformanceFrequency(ref _masterClock);
        _win_mm.TimeEndPeriod(1);

        _oneFrame.QuadPart = _masterClock.QuadPart / (Define.TARGETFRAMERATE);
        _oneMs.QuadPart = _masterClock.QuadPart / 1000;
        _fMasterClock = _masterClock.QuadPart;
    }

    public float CalculateFps()
    {
        if (++_frameCount != Define.FRAMEINTERVAL)
        {
            return (_fps);
        }

        _kernel.QueryPerformanceCounter(ref _now);

        _fNow = _now.QuadPart;
        _fps = (_fNow - _fLast) / _fMasterClock;
        _fLast = _fNow;
        _frameCount = 0;
        _fps = Define.FRAMEINTERVAL / _fps;

        return _fps;
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
        _kernel.QueryPerformanceCounter(ref _currentTime);
    }

    public void StartRender()
    {
        _kernel.QueryPerformanceCounter(ref _startTime);
    }

    public void EndRender(int skip)
    {
        _frameSkip = skip;
        _targetTime.QuadPart = _startTime.QuadPart + _oneFrame.QuadPart * _frameSkip;
    }

    public void Reset()
    {
        _currentTime = new LARGE_INTEGER();
        _startTime = new LARGE_INTEGER();
        _masterClock = new LARGE_INTEGER();
        _now = new LARGE_INTEGER();
        _targetTime = new LARGE_INTEGER();
        _oneMs = new LARGE_INTEGER();
        _oneFrame = new LARGE_INTEGER();

        _frameCount = 0;
        _fps = 0;
        _fNow = 0;
        _fLast = 0;
        _fMasterClock = 0;
        _frameSkip = 0;
    }
}
