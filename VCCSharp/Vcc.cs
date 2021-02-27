using System;
using System.Diagnostics;

namespace VCCSharp
{
    public class Vcc
    {
        public void Startup(IntPtr hInstance, string commandLine)
        {
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
