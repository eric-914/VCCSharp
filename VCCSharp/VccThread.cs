using System.Diagnostics;
using VCCSharp.Models;

namespace VCCSharp
{
    public class VccThread
    {
        private readonly CommandLineParser _commandLineParser = new CommandLineParser();
        private readonly VccApp _vccApp = new VccApp();

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
