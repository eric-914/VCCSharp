using System.Diagnostics;
using VCCSharp.Models;

namespace VCCSharp
{
    public interface IVccThread
    {
        void Run();
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

        public void Run()
        {
            CmdLineArguments? args = _commandLineParser.Parse();
            if (args == null)
            {
                return;
            }

            _vccApp.Startup(Process.GetCurrentProcess().Handle, args.Value);

            _vccApp.Threading();

            _vccApp.Run();

            _vccApp.Shutdown();
        }
    }
}
