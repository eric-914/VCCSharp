using VCCSharp.Modules.TCC1014.Masks;
using VCCSharp.Modules.TCC1014.Modes;

namespace VCCSharp.Modules;

public interface IScreen
{
    void DrawTopBorder(int lineCounter);
    void DrawBottomBorder(int lineCounter);
    void DrawScreen(int lineCounter, ModeModel mode);
}

public class Screen : IScreen
{
    private readonly IGraphics _graphics;
    private readonly IEmu _emu;

    private readonly Modes _modes = new();

    public Screen(IGraphics graphics, IEmu emu)
    {
        _graphics = graphics;
        _emu = emu;
    }

    public void DrawTopBorder(int lineCounter)
    {
        if (_graphics.BorderChange == 0)
        {
            return;
        }

        uint bc = _graphics.BorderColor;
        ushort wsx = (ushort)_emu.WindowSize.X;
        long sp = _emu.SurfacePitch;
        long lc = lineCounter * 2 * sp;

        var surface = _graphics.GetGraphicsSurface();

        for (ushort x = 0; x < wsx; x++)
        {
            surface[x + lc] = bc;

            if (!_emu.ScanLines)
            {
                surface[x + lc + sp] = bc;
            }
        }
    }

    public void DrawBottomBorder(int lineCounter)
    {
        if (_graphics.BorderChange == 0)
        {
            return;
        }

        uint bc = _graphics.BorderColor;
        ushort wsx = (ushort)_emu.WindowSize.X;
        long sp = _emu.SurfacePitch;
        long lc = (lineCounter + _graphics.LinesPerScreen + _graphics.VerticalCenter) * 2 * sp;

        var surface = _graphics.GetGraphicsSurface();

        for (ushort x = 0; x < wsx; x++)
        {
            surface[x + lc] = bc;

            if (!_emu.ScanLines)
            {
                surface[x + lc + sp] = bc;
            }
        }
    }

    public void DrawScreen(int lineCounter, ModeModel mode)
    {
        ushort y = (ushort) lineCounter;
        long xPitch = _emu.SurfacePitch;

        int vy = (y + _graphics.VerticalCenter) * 2;
        long vyx = vy * xPitch;

        if (_graphics.HorizontalCenter != 0 && _graphics.BorderChange > 0)
        {
            uint bc = _graphics.BorderColor;
            int hx = _graphics.PixelsPerLine * (_graphics.Stretch + 1) + _graphics.HorizontalCenter;

            var surface = _graphics.GetGraphicsSurface();

            for (ushort x = 0; x < _graphics.HorizontalCenter; x++)
            {
                //--Left border
                surface[x + vyx] = bc;

                if (!_emu.ScanLines)
                {
                    surface[x + vyx + xPitch] = bc;
                }

                //--Right border
                surface[x + hx + vyx] = bc;

                if (!_emu.ScanLines)
                {
                    surface[x + hx + vyx + xPitch] = bc;
                }
            }
        }

        if (_graphics.LinesPerRow < 13)
        {
            _graphics.TagY++;
        }

        if (y == 0)
        {
            _graphics.StartOfVidRam = _graphics.NewStartOfVidRam;
            _graphics.TagY = 0;
        }

        int start = (int)(_graphics.StartOfVidRam + (_graphics.TagY / _graphics.LinesPerRow) * (_graphics.VPitch * _graphics.ExtendedText));
        int yStride = (int)(vy * xPitch + _graphics.HorizontalCenter - 1);

        _emu.LineCounter = (short) lineCounter;

        _modes[_graphics.MasterMode](mode, start, yStride);
    }
}
