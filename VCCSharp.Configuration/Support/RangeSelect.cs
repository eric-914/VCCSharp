using Newtonsoft.Json;

namespace VCCSharp.Configuration.Support;

public interface IRangeSelect<T> where T : struct
{
    T Value { get; set; }
}

public interface IRangeSelect
{
    int Value { get; set; }
}

/// <summary>
/// The purpose of the RangeSelect class is to convert a range the app uses into something readable when stored in the configuration file.
/// </summary>
/// <typeparam name="T">The range type (enumerable)</typeparam>
public class RangeSelect<T> : IRangeSelect<T> where T : struct
{
    private readonly bool _usesUnderscore;
    private readonly T[] _range;

    /// <summary>
    /// Creates a selectable range based off the given type (enumerable)
    /// </summary>
    /// <param name="usesUnderscore">For when a enumerable range is prefixed with "_" as in those that represent number values</param>
    public RangeSelect(bool usesUnderscore = false)
    {
        _usesUnderscore = usesUnderscore;
        _range = (T[])Enum.GetValues(typeof(T));
        Value = _range.First();
    }

    /// <summary>
    /// This will list the available range for the given enumerable when written to the configuration file.
    /// This makes it easier to manually edit the configuration file.
    /// </summary>
    [JsonProperty("(Options)")]
    public virtual string Options => $"{{{string.Join(',', _range.Select(Clean))}}}";

    /// <summary>
    /// This is what the app actually uses.
    /// </summary>
    [JsonIgnore]
    public T Value { get; set; }

    /// <summary>
    /// This is what gets read/written in the configuration file.
    /// This makes it easier to manually edit the configuration file.
    /// </summary>
    public virtual string Selected
    {
        get => _usesUnderscore ? Clean(Value) : Value.ToString() ?? string.Empty;
        set => Value = Enum.Parse<T>(_usesUnderscore ? $"_{value}" : value);
    }

    //--Convert range value (enum) to string.
    private static string Clean(T value) => Clean(value.ToString() ?? string.Empty);
    //--Remove any "_" prefix.
    private static string Clean(string value) => value.StartsWith("_") ? value[1..] : value;
}

/// <summary>
/// Same type of class but for a given fixed number range
/// </summary>
public class RangeSelect : IRangeSelect
{
    private readonly int[] _range;

    public RangeSelect(params int[] range)
    {
        _range = range;
        Value = _range.First();
    }

    [JsonProperty("(Options)")]
    public string Comments => $"{{{_range.First()}-{_range.Last()}}}";

    [JsonIgnore]
    public int Value { get; set; }

    public virtual string Selected
    {
        get => Value.ToString();
        set => Value = int.Parse(value);
    }
}
