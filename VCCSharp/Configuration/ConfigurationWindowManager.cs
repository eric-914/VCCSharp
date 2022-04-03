using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.Dx;

namespace VCCSharp.Configuration;

public interface IConfigurationWindowManager
{
    void ShowDialog(IConfiguration model);
}

public class ConfigurationWindowManager : IConfigurationWindowManager
{
    private readonly IModules _modules;
    private readonly IViewModelFactory _factory;
    private readonly IDxManager _manager;

    public ConfigurationWindowManager(IModules modules, IViewModelFactory factory, IDxManager manager)
    {
        _modules = modules;
        _factory = factory;
        _manager = manager;
    }

    public void ShowDialog(IConfiguration model)
    {
        var viewModel = _factory.CreateConfigurationViewModel(model, _manager);

        var view = new ConfigurationWindow(viewModel) { Apply = ApplyChanges };

        view.Show();
    }

    public void ApplyChanges()
    {
        _modules.Vcc.ApplyConfigurationChanges();
    }
}
