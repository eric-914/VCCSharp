using VCCSharp.IoC;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration;

public interface IConfigurationWindowManager
{
    void ShowDialog(IConfiguration model);
}

public class ConfigurationWindowManager : IConfigurationWindowManager
{
    private readonly IModules _modules;
    private readonly IViewModelFactory _factory;

    public ConfigurationWindowManager(IModules modules, IViewModelFactory factory)
    {
        _modules = modules;
        _factory = factory;
    }

    public void ShowDialog(IConfiguration model)
    {
        var viewModel = _factory.CreateConfigurationViewModel(model);

        var view = new ConfigurationWindow(viewModel) { Apply = ApplyChanges };

        view.Show();
    }

    public void ApplyChanges()
    {
        _modules.Vcc.ApplyConfigurationChanges();
    }
}
