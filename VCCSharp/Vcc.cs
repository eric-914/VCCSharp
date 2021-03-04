using System;
using VCCSharp.Models;

namespace VCCSharp
{
    public class Vcc
    {
        private IntPtr _hResources;
        private EmuState _emuState;

        public void Startup(IntPtr hInstance, CmdLineArguments cmdLineArgs)
        {
            _hResources = Library.LoadLibrary("resources.dll");

            Library.Vcc.VccStartup(hInstance, _hResources, cmdLineArgs, _emuState);
        }

        public void Run()
        {
            Library.Vcc.VccRun();
        }

        public int Shutdown()
        {
            var code = Library.Vcc.VccShutdown();

            Library.FreeLibrary(_hResources);

            return code;
        }
    }
}
