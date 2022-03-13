namespace VCCSharp.Libraries
{
    // ReSharper disable once IdentifierTypo
    public interface IWinmm
    {
        ushort TimeBeginPeriod(ushort uPeriod);
        ushort TimeEndPeriod(ushort uPeriod);
    }

    // ReSharper disable once IdentifierTypo
    public class Winmm : IWinmm
    {
        public ushort TimeBeginPeriod(ushort uPeriod)
            => WinmmDLL.timeBeginPeriod(uPeriod);

        public ushort TimeEndPeriod(ushort uPeriod)
            => WinmmDLL.timeEndPeriod(uPeriod);
    }
}
