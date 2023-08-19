using VCCSharp.Configuration.Support;

namespace VCCSharp.Shared.Configuration;

public class NullRangeSelect<T> : IRangeSelect<T> where T : struct
{
    public T Value { get; set; }
}