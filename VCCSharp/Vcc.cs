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

            _emuState.Resources = _hResources;

            Library.DirectDraw.InitDirectDraw(hInstance, _hResources);
            
            Library.Vcc.CheckQuickLoad(cmdLineArgs.QLoadFile);
            Library.CoCo.SetClockSpeed(1);  //Default clock speed .89 MHZ	

            Library.Vcc.VccStartup(hInstance, ref cmdLineArgs, ref _emuState);

            Library.Vcc.VccStartupThreading();
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
