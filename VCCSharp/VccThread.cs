using System;
using System.Threading.Tasks;
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

        Task.Run(() => Run(args));
    }

    public void Run(CmdLineArguments args)
    {
        _vccApp.LoadConfiguration(args.IniFile);
        _vccApp.Startup();

        //--The emulation runs on a different thread
        _vccApp.Threading();

        _vccApp.Run(args.QLoadFile);
    }
}
