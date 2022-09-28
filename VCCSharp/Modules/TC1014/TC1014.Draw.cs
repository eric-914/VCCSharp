namespace VCCSharp.Modules.TC1014;

// ReSharper disable InconsistentNaming
public partial class TC1014
{
    public void DrawTopBorder32(int lineCounter)
    {
        if (Graphics.BorderChange == 0)
        {
            return;
        }

        IEmu emu = _modules.Emu;

        uint bc = Graphics.BorderColor;
        ushort wsx = (ushort)emu.WindowSize.X;
        long sp = emu.SurfacePitch;
        long lc = lineCounter * 2 * sp;

        var surface = Graphics.GetGraphicsSurface();

        for (ushort x = 0; x < wsx; x++)
        {
            surface[x + lc] = bc;

            if (!emu.ScanLines)
            {
                surface[x + lc + sp] = bc;
            }
        }
    }

    public void DrawBottomBorder32(int lineCounter)
    {
        if (Graphics.BorderChange == 0)
        {
            return;
        }

        IEmu emu = _modules.Emu;

        uint bc = Graphics.BorderColor;
        ushort wsx = (ushort)emu.WindowSize.X;
        long sp = emu.SurfacePitch;
        long lc = (lineCounter + Graphics.LinesPerScreen + Graphics.VerticalCenter) * 2 * sp;

        var surface = Graphics.GetGraphicsSurface();

        for (ushort x = 0; x < wsx; x++)
        {
            surface[x + lc] = bc;

            if (!_modules.Emu.ScanLines)
            {
                surface[x + lc + sp] = bc;
            }
        }
    }

    public void UpdateScreen(int lineCounter)
    {
        ushort y = (ushort) lineCounter;
        long xPitch = _modules.Emu.SurfacePitch;

        int vy = (y + Graphics.VerticalCenter) * 2;
        long vyx = vy * xPitch;

        if ((Graphics.HorizontalCenter != 0) && (Graphics.BorderChange > 0))
        {
            uint bc = Graphics.BorderColor;
            int hx = Graphics.PixelsPerLine * (Graphics.Stretch + 1) + Graphics.HorizontalCenter;

            var surface = Graphics.GetGraphicsSurface();

            for (ushort x = 0; x < Graphics.HorizontalCenter; x++)
            {
                //--Left border
                surface[x + vyx] = bc;

                if (!_modules.Emu.ScanLines)
                {
                    surface[x + vyx + xPitch] = bc;
                }

                //--Right border
                surface[x + hx + vyx] = bc;

                if (!_modules.Emu.ScanLines)
                {
                    surface[x + hx + vyx + xPitch] = bc;
                }
            }
        }

        if (Graphics.LinesPerRow < 13)
        {
            Graphics.TagY++;
        }

        if (y == 0)
        {
            Graphics.StartOfVidRam = Graphics.NewStartOfVidRam;
            Graphics.TagY = 0;
        }

        int start = (int)(Graphics.StartOfVidRam + (Graphics.TagY / Graphics.LinesPerRow) * (Graphics.VPitch * Graphics.ExtendedText));
        int yStride = (int)(vy * xPitch + Graphics.HorizontalCenter - 1);

        _modules.Emu.LineCounter = (short) lineCounter;

        SwitchMasterMode(Graphics.MasterMode, start, yStride);
    }
}
