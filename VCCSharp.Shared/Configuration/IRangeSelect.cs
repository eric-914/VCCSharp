namespace VCCSharp.Shared.Configuration;

public interface IRangeSelect<T> where T : struct
{
    T Value { get; set; }
}

public interface IRangeSelect
{
    int Value { get; set; }
}

public class NullRangeSelect<T> : IRangeSelect<T> where T : struct
{
    public T Value { get; set; }
}