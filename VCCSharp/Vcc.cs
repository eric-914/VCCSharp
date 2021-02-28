using System;

namespace VCCSharp
{
    public class Vcc
    {
        public void Startup(IntPtr hInstance, string commandLine)
        {
            var cmdLineArgs = Library.Vcc.GetCmdLineArgs(commandLine);

            Library.Vcc.VccStartup(hInstance, commandLine);
        }

        public void Run()
        {
            Library.Vcc.VccRun();
        }

        public int Shutdown()
        {
            return Library.Vcc.VccShutdown();
        }
    }
}
