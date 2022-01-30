using System.IO;
using Newtonsoft.Json;
using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support
{
    /// <summary>
    /// Load/Save an object via the given path.
    /// </summary>
    public interface IPersistence<out T>
    {
        T Load(string path);
        void Save(string path, IConfiguration model);
    }

    public abstract class Persistence<T> : IPersistence<T>
    {
        private readonly IFactory _factory;

        protected Persistence(IFactory factory)
        {
            _factory = factory;
        }

        public T Load(string path) 
            => Load(_factory.Get<T>(), path);

        private static T Load(T instance, string path) =>
            File.Exists(path)
                ? JsonConvert.DeserializeAnonymousType(File.ReadAllText(path), instance)
                : instance;

        public void Save(string path, IConfiguration model) 
            => File.WriteAllText(path, JsonConvert.SerializeObject(model, Formatting.Indented));
    }
}
