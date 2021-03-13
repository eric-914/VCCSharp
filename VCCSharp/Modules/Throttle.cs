using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IThrottle
    {
        unsafe ThrottleState* GetThrottleState();
        void CalibrateThrottle();
        void FrameWait();
        void StartRender();
        void EndRender(byte skip);
        float CalculateFPS();
    }

    public class Throttle : IThrottle
    {
        private readonly IModules _modeModules;
        private readonly IKernel _kernel;
        private static ushort frameCount = 0;
        private static float fps = 0, fNow = 0, fLast = 0;

        public Throttle(IModules modeModules, IKernel kernel)
        {
            _modeModules = modeModules;
            _kernel = kernel;
        }

        public unsafe ThrottleState* GetThrottleState()
        {
            return Library.Throttle.GetThrottleState();
        }

        public void CalibrateThrottle()
        {
            Library.Throttle.CalibrateThrottle();
        }

        public void FrameWait()
        {
            Library.Throttle.FrameWait();
        }

        public void StartRender()
        {
            Library.Throttle.StartRender();
        }

        public void EndRender(byte skip)
        {
            Library.Throttle.EndRender(skip);
        }

        public float CalculateFPS()
        {
            unsafe
            {
                ThrottleState* throttleState = GetThrottleState();

                if (++frameCount != Define.FRAMEINTERVAL) {
                    return(fps);
                }

                _kernel.QueryPerformanceCounter(&(throttleState->Now));

                fNow = throttleState->Now.QuadPart;
                fps = (fNow - fLast) / throttleState->fMasterClock;
                fLast = fNow;
                frameCount = 0;
                fps = Define.FRAMEINTERVAL / fps;

                return fps;
            }
        }
    }
}
