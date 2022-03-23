using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support;

public interface IConfigurationPersistence : IPersistence<IConfigurationRoot> { }

public class ConfigurationPersistence : Persistence<ConfigurationRoot>, IConfigurationPersistence
{
    public ConfigurationPersistence(IFactory factory) : base(factory) { }

    public new IConfigurationRoot Load(string path) => base.Load(path);
}
