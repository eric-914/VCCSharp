using System;
using VCCSharp.Models;

namespace VCCSharp
{
    public interface IVccThread
    {
        void Run(IntPtr hWnd);
    }

    public class VccThread : IVccThread
    {
        private readonly ICommandLineParser _commandLineParser;
        private readonly IVccApp _vccApp;

        public VccThread(IVccApp vccApp, ICommandLineParser commandLineParser)
        {
            _vccApp = vccApp;
            _commandLineParser = commandLineParser;
        }

        public void Run(IntPtr hWnd)
        {
            CmdLineArguments args = _commandLineParser.Parse();
            if (args == null)
            {
                return;
            }

            _vccApp.SetWindow(hWnd);

            _vccApp.Startup(args);

            _vccApp.Threading();

            _vccApp.Run();

            _vccApp.Shutdown();
        }
    }
}
