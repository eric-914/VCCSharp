namespace VCCSharp.Shared.Models;

public interface IInterval
{
    int Interval { get; set; }
}

public class NullInterval : IInterval
{
    public int Interval { get; set; } = 100;
}