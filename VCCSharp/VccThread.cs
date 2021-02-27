using System.Diagnostics;

namespace VCCSharp
{
    public class VccThread
    {
        private readonly Vcc _vcc = new Vcc();

        public void Run()
        {
            const string commandLine = "\"c:\\CoCo\\Mega-Bug (1982) (26-3076) (Tandy).ccc\" ";

            _vcc.Startup(Process.GetCurrentProcess().Handle, commandLine);

            _vcc.Run();

            var retValue = _vcc.Shutdown();
        }
    }
}
