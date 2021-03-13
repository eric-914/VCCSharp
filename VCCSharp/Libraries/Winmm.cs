namespace VCCSharp.Libraries
{
    public interface IWinmm
    {
        ushort timeBeginPeriod(ushort uPeriod);
        ushort timeEndPeriod(ushort uPeriod);
    }

    public class Winmm : IWinmm
    {
        public ushort timeBeginPeriod(ushort uPeriod)
            => WinmmDLL.timeBeginPeriod(uPeriod);

        public ushort timeEndPeriod(ushort uPeriod)
            => WinmmDLL.timeEndPeriod(uPeriod);
    }
}
