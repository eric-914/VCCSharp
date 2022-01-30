using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support
{
    public interface IConfigurationPersistence : IPersistence<IConfiguration> { }

    public class ConfigurationPersistence : Persistence<Root>, IConfigurationPersistence
    {
        public ConfigurationPersistence(IFactory factory) : base(factory) { }

        public new IConfiguration Load(string path) => base.Load(path);
    }
}
