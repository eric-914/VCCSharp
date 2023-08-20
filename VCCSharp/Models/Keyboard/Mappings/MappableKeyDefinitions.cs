using System.Windows.Input;

namespace VCCSharp.Models.Keyboard.Mappings;

/// <summary>
/// A filter wrapper around KeyDefinitions
/// </summary>
public class MappableKeyDefinitions
{
    private static List<IKey> _mappable = new KeyDefinitions().Where(x => x.IsMappable).ToList();

    public IList<Key> Keys => _mappable.Select(x => x.Key).ToList();
    public IList<string> Values => _mappable.Select(x => x.Text).ToList();
}
