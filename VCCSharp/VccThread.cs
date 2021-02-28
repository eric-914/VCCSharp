using System.Diagnostics;

namespace VCCSharp
{
    public class VccThread
    {
        private readonly CommandLineParser _commandLineParser = new CommandLineParser();
        private readonly Vcc _vcc = new Vcc();

        public void Run()
        {
            CmdLineArguments? args = _commandLineParser.Parse();
            if (args == null)
            {
                return;
            }

            _vcc.Startup(Process.GetCurrentProcess().Handle, args.Value);

            _vcc.Run();

            var retValue = _vcc.Shutdown();
        }
    }
}
