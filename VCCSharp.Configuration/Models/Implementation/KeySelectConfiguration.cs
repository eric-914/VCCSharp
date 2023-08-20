using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

/// <summary>
/// Special range select for Input.Key
/// </summary>
public class KeySelectConfiguration : RangeSelect<Key>, IKeySelectConfiguration
{
    [JsonIgnore]
    public override string Options => "See System.Windows.Input.Key"; //--Very large range, won't list.  So give help where to look.
}
