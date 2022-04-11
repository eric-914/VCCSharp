using VCCSharp.Configuration.ViewModel;
using VCCSharp.IoC;

namespace VCCSharp.Configuration;

public interface IConfigurationWindowManager
{
    void ShowDialog();
}

public class ConfigurationWindowManager : IConfigurationWindowManager
{
    private readonly IModules _modules;
    private readonly IFactory _factory;

    public ConfigurationWindowManager(IModules modules, IFactory factory)
    {
        _modules = modules;
        _factory = factory;
    }

    public void ShowDialog()
    {
        var viewModel = _factory.Get<ConfigurationViewModel>();

        var view = new ConfigurationWindow(viewModel) { Apply = ApplyChanges };

        view.Show();
    }

    public void ApplyChanges()
    {
        _modules.Vcc.ApplyConfigurationChanges();
    }
}
