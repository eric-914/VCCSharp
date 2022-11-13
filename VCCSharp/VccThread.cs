using VCCSharp.Models;

namespace VCCSharp;

public interface IVccThread
{
    void Run(IntPtr hWnd, CmdLineArguments args);
}

public class VccThread : IVccThread
{
    private readonly IVccApp _vccApp;

    public VccThread(IVccApp vccApp)
    {
        _vccApp = vccApp;
    }

    public void Run(IntPtr hWnd, CmdLineArguments args)
    {
        _vccApp.SetWindow(hWnd);

        //_vccApp.LoadConfiguration(args.IniFile);

        //--The emulation runs on a different thread
        Task.Run(() => Run(args));
    }

    private void Run(CmdLineArguments args)
    {
        _vccApp.LoadConfiguration(args.IniFile);

        _vccApp.Startup(args.QLoadFile);

        _vccApp.Run();
    }
}
