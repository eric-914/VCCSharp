using Newtonsoft.Json;
using System.Windows.Input;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Configuration;

namespace VCCSharp.Models.Configuration.Support;

/// <summary>
/// Special range select for Input.Key
/// </summary>
public class KeySelect : RangeSelect<Key>, IKeySelect
{
    [JsonIgnore]
    public override string Options => "See System.Windows.Input.Key"; //--Very large range, won't list.  So give help where to look.
}
