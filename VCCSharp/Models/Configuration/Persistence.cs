using System.IO;
using Newtonsoft.Json;

namespace VCCSharp.Models.Configuration
{
    public interface IPersistence
    {
        ConfigurationModel Load(string path);
        void Save(string path, ConfigurationModel model);
    }

    public class Persistence : IPersistence
    {
        public ConfigurationModel Load(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);

                return JsonConvert.DeserializeObject<ConfigurationModel>(json);
            }

            return new ConfigurationModel(); //TODO: Define defaults
        }

        public void Save(string path, ConfigurationModel model)
        {
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);

            File.WriteAllText(path, json);
        }
    }
}
