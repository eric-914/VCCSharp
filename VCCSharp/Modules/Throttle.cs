using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IThrottle
    {
        void CalibrateThrottle();
        void FrameWait();
        void StartRender();
        void EndRender(byte skip);
        float CalculateFPS();
    }

    public class Throttle : IThrottle
    {
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
            return Library.Throttle.CalculateFPS();
        }
    }
}
