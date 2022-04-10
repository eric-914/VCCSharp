using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //ShutdownMode = ShutdownMode.OnExplicitShutdown;

        Binding.Initialize(Factory.Instance);
    }
}
