using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IThrottle
    {
        void CalibrateThrottle();
    }

    public class Throttle : IThrottle
    {
        public void CalibrateThrottle()
        {
            Library.Throttle.CalibrateThrottle();
        }
    }
}
