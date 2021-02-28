using System;

namespace VCCSharp
{
    public class Vcc
    {
        public void Startup(IntPtr hInstance, CmdLineArguments cmdLineArgs)
        {
            Library.Vcc.VccStartup(hInstance, cmdLineArgs);
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
