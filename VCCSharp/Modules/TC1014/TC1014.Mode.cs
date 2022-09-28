using VCCSharp.Modules.TC1014.Modes;

namespace VCCSharp.Modules.TC1014;

public delegate void Mode(ModeModel model, int start, int yStride);

// ReSharper disable once InconsistentNaming
public partial class TC1014
{
    private readonly Masks.Modes _modes = new();

    public void SwitchMasterMode(byte masterMode, int start, int yStride)
    {
        var model = new ModeModel(_ram, _modules);

        // (GraphicsMode << 7) | (CompatibilityMode << 6)  | ((Bpp & 3) << 4) | (Stretch & 15);
        _modes[masterMode](model, start, yStride);
    }
}
