using System.Windows.Input;

namespace VCCSharp.Configuration.Models;

public interface IKeySelectConfiguration
{
    Key Value { get; set; }
    string Selected { get; set; }
}
