using System.Windows.Input;

namespace VCCSharp.Shared.Configuration;

public interface IKeySelect
{
    Key Value { get; set; }
    string Selected { get; set; }
}
