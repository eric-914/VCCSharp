using System.Windows.Input;

namespace VCCSharp.Configuration.Models;

public interface IKeySelect
{
    Key Value { get; set; }
    string Selected { get; set; }
}
