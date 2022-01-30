using System.IO;
using Newtonsoft.Json;

namespace VCCSharp.Models.Configuration.Support
{
    public interface IPersistence
    {
        Root Load(string path);
        void Save(string path, Root model);
    }

    public class Persistence : IPersistence
    {
        public Root Load(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);

                return JsonConvert.DeserializeObject<Root>(json);
            }

            return new Root(); //TODO: Define defaults
        }

        public void Save(string path, Root model)
        {
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);

            File.WriteAllText(path, json);
        }
    }
}
