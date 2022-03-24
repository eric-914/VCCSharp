using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support;

public interface IConfigurationPersistence : IPersistence<IConfigurationRoot> { }

public class ConfigurationPersistence : Persistence<IConfigurationRoot>, IConfigurationPersistence
{
    public ConfigurationPersistence(IFactory factory) : base(factory) { }
}
