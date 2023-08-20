using VCCSharp.Configuration.Options;

namespace VCCSharp.Models.Graphics;

/// <summary>
/// Manages the palette array for the different monitor types
/// </summary>
public class PaletteLookup32
{
    private readonly UintArray _composite = new(64);
    private readonly UintArray _rgb = new(64);

    public UintArray this[MonitorTypes monitorType]
    {
        get
        {
            return monitorType switch
            {
                MonitorTypes.Composite => _composite,
                MonitorTypes.RGB => _rgb,

#pragma warning disable CA2208
                _ => throw new ArgumentException(nameof(monitorType))
            };
        }
    }
}