using System.Windows.Input;

namespace VCCSharp.Models.Keyboard.Mappings;

/// <summary>
/// A filter wrapper around KeyDefinitions
/// Used by the Configuration U/I only.
/// </summary>
public class MappableKeyDefinitions
{
    private static List<IKey> Mappable => new KeyDefinitions().Where(x => x.IsMappable).ToList();

    private static IList<Key> _keys = Mappable.Select(x => x.Key).ToList();
    private static IList<string> _values = Mappable.Select(x => x.Text).ToList();

    public static IList<Key> Keys => _keys;
    public static IList<string> Values => _values;
}
