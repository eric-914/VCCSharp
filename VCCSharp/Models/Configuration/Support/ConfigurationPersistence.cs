using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support;

public interface IConfigurationPersistence : IPersistence<IConfiguration> { }

public class ConfigurationPersistence : Persistence<IConfiguration>, IConfigurationPersistence
{
    public ConfigurationPersistence(IFactory factory) : base(factory) { }
}
