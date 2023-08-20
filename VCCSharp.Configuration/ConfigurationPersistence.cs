using VCCSharp.Configuration.Support;
using VCCSharp.IoC;

namespace VCCSharp.Configuration;

public interface IConfigurationPersistence : IPersistence<IConfiguration> { }

public class ConfigurationPersistence : Persistence<IConfiguration>, IConfigurationPersistence
{
    public ConfigurationPersistence(IFactory factory) : base(factory) { }
}
